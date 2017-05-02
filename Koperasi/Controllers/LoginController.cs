using koperasi.Models;
using koperasi.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace koperasi.Controllers
{
    public class LoginController : Controller
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("LoginController");
        private userService _userService;

        public LoginController()
        {
            _userService = new userService();
        }

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to requested URL, or homepage if no previous page requested
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Pelanggan");
            }
            else
            {
                var model = new passwordModels();
                //model = InitiateLoginForm(model);

                return View(model);
            }
        }

        public passwordModels InitiateLoginForm(passwordModels model)
        {
            //model.ListCompany = (from b in _companyService.GetCompanies("", "", "")
            //                     where b.Deleted == false
            //                     select new SelectListItem
            //                     {
            //                         Text = b.CompanyName,
            //                         Value = b.Id.ToString()
            //                     }).ToList();
            //model.ListCompany.Insert(0, new SelectListItem { Value = "0", Text = "-- Please Select Company --" });

            return model;
        }

        [HttpPost]
        public ActionResult Index(passwordModels model)
        {
            //string message = "";

            //bool isValid = false;
            //object objHasil = null;

            var objResp = new passwordModels();

            try
            {
                //TODO: validate user password, save to session, etc
                string vPassword = model.password.Trim().ToUpper();
                string no_anggota = model.no_anggota.Trim().ToUpper();

                //Log.Debug(DateTime.Now + "LOGIN CONTROLLER No Anggota : " + no_anggota + ", Password : " + password);

                objResp = _userService.GetPassword(no_anggota);

                //Log.Debug(DateTime.Now + "LOGIN CONTROLLER ==>> ID : " + objResp.id);

                if (objResp.id > 0)
                {
                    var dataLogin = objResp.no_anggota + "#" + objResp.nama_anggota + "#" + objResp.sektor + "#" + objResp.as_admin;

                    Log.Debug(DateTime.Now + " LOGIN No Anggota : " + objResp.id + "/"+ no_anggota + ", nama_anggota : " + objResp.nama_anggota + ", Pass User : " + vPassword +
                        ", Pass Data : " + objResp.password + ", As Admin : " + objResp.as_admin);

                    if (vPassword != objResp.password.Trim().ToUpper())
                    {
                        ModelState.AddModelError("", "Password Yang dimasukkan Salah...");
                    }

                }
                else
                {
                    Log.Error("Login Failed, username:" + model.no_anggota + " Password : " + model.password );
                    ModelState.AddModelError("", "Login Failed, Invalid Nomer Anggota...");
                }

                if (ModelState.IsValid)
                {
                    int SessionTime = 120;
                    string strSessionTime = System.Configuration.ConfigurationManager.AppSettings["SessionTime"];
                    if (!String.IsNullOrEmpty(strSessionTime))
                    {
                        if (!int.TryParse(strSessionTime, out SessionTime))
                            SessionTime = 120;
                    }

                    FormsAuthenticationTicket tkt;
                    string cookiestr;
                    HttpCookie ck;
                    tkt = new FormsAuthenticationTicket(1,
                        objResp.no_anggota,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(SessionTime),
                        false,
                        objResp.no_anggota + "#" + objResp.nama_anggota + "#" + objResp.sektor + "#" + objResp.as_admin);

                    cookiestr = FormsAuthentication.Encrypt(tkt);

                    ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);

                    //if (model.RememberMe)
                    //    ck.Expires = tkt.Expiration;

                    ck.Path = FormsAuthentication.FormsCookiePath;
                    Response.Cookies.Add(ck);

                    // Log
                    Log.Info("Login Success, UserCode: " + model.no_anggota);

                    // Redirect to requested URL, or homepage if no previous page requested
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Pelanggan");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Login Failed, username:" + model.nama_anggota, ex);
                ModelState.AddModelError("", "Login Failed, Please try Again or Contact Your Administrator.");
            }


            // Re-Initiate
            //model = InitiateLoginForm(model);

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        [HttpPost]
        public dynamic GetPassword(string pNoAnggota, string pPass)
        {
            string message = "";

            bool isValid = false;
            object objHasil = null;

            var vList = _userService.GetPassword(pNoAnggota) as passwordModels;
            var json = JsonConvert.SerializeObject(vList);

            if (pPass != vList.password)
            {
                isValid = false;
                message = "Password Yang dimasukkan Salah...";
            }
            else
            {
                isValid = true;
                message = "Password Yang dimasukkan Sesuai...";
            }

            Log.Debug(DateTime.Now + " GetPassword ====>>>>>> Password : " + vList.password);

            return Json(new
            {
                isValid,
                message,
                objHasil
            });
        }

    }
}
