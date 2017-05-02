using koperasi.Models;
using koperasi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace koperasi.Controllers
{
    public class UserController : Controller
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("UserController");
        private userService _userService;

        public UserController()
        {
            _userService = new userService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(passwordModels pModel)
        {

            var newPass = new passwordModels();
            passwordModels objResult;


            try
            {
                //TODO: validate user password, save to session, etc

                string vPassLama = pModel.password.Trim().ToUpper();
                string vPassBaru = pModel.password2.Trim().ToUpper();
                string vKonfPass = pModel.keterangan.Trim().ToUpper();

                newPass.no_anggota = ConfigModels.GetNoAnggota();
                newPass.password = vPassBaru;
                newPass.keterangan = vKonfPass;

                objResult = _userService.GetPassword(newPass.no_anggota);

                Log.Debug(DateTime.Now + "  ===>>>> USER CONTROLLER No Anggota : " + newPass.no_anggota +
                    ", Pass Lama dari DataBase : " + objResult.password + ", Pass Lama : " + vPassLama + ", Pass Baru : " + vPassBaru + ", Konfirmasi Pass : " + vKonfPass);

                if (vPassLama != objResult.password.Trim().ToUpper())
                {
                    ModelState.AddModelError("2", "**Password Lama Yang Dimasukkan Salah...");
                }
                else 
                {
                    if (vPassBaru != vKonfPass)
                    {
                        ModelState.AddModelError("3", "**Password Baru Tidak Sama...");
                    }
                    else
                    {

                        objResult = _userService.UpdatePassword(newPass);
                    }

                }

                if (ModelState.IsValid)
                {
                    Log.Info(DateTime.Now + " ===>>>> Change Password Success, UserCode: " + newPass.no_anggota);
                    ModelState.AddModelError("1", "CHANGE PASSWORD SUCCESS...");

                    // Redirect to requested URL, or homepage if no previous page requested
                    //string returnUrl = Request.QueryString["ReturnUrl"];
                    //if (!String.IsNullOrEmpty(returnUrl))
                    //    return Redirect(returnUrl);

                    //return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now + " =====>>>> Change Password Failed, No Anggota:" + pModel.no_anggota, ex);
                ModelState.AddModelError("1", "Change Password Failed, Please try Again or Contact Your Administrator.");
            }

            return View(pModel);
        }


        public dynamic UpdatePassword(passwordModels pModel)
        {
            string message = "";

            bool isValid = true;
            object objHasil = null;
            try
            {
                // Update
                //ResponseModel respModel = _userService.UpdatePassword(pModel);
                //isValid = respModel.isValid;
                //message = respModel.message;
            }
            catch (Exception ex)
            {
                Log.Error("Update Data Failed, No Anggota: " + pModel.no_anggota, ex);
                isValid = false;
                message = "Update Data Failed..!!" + "\r" + "Error Message: " + ex.Message;
            }

            return Json(new
            {
                isValid,
                message,
                objHasil
            });
        }

        public dynamic InsertPassword(passwordModels pModel)
        {
            string message = "";

            bool isValid = true;
            object objHasil = null;
            try
            {
                // Update
                ResponseModel respModel = _userService.InsertPassword(pModel);
                isValid = respModel.isValid;
                message = respModel.message;
            }
            catch (Exception ex)
            {
                Log.Error("Insert Data Failed, No Anggota: " + pModel.no_anggota, ex);
                isValid = false;
                message = "Insert Data Failed..!!" + "\r" + "Error Message: " + ex.Message;
            }

            return Json(new
            {
                isValid,
                message,
                objHasil
            });
        }

        public ActionResult InsertAllPassword()
        {
            string message = "";

            try
            {
                _userService.InsertAllPassword();
                message = "INSERT PASSWORD BERHASIL...";
            }
            catch (Exception ex)
            {
                Log.Error("Insert SEMUA Data ANGGOTA Failed...", ex);
                message = "Insert Data Failed..!!" + "\r" + "Error Message: " + ex.Message;
            }

            return Content(message);
        }

    }
}
