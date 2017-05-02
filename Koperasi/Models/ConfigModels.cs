using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using log4net;

namespace koperasi.Models
{

    public class ConfigModels
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("ConfigModels");

        public const string eKeyStr = "SsDK0per4s1";


        public static string GetNoAnggota()
        {
            string noAnggota = "";
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 
                        noAnggota = ticket.Name;
                    }
                }
            }
            return noAnggota;
        }//end function GetUserCode
        
        public static string GetNamaAnggota()
        {
            string namaAnggota = "";
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 

                        string[] userDetail = ticket.UserData.Split('#');
                        if (!String.IsNullOrEmpty(userDetail[1]))
                            namaAnggota = userDetail[1];
                    }
                }
            }
            return namaAnggota;
        }//end function GetUserName

        public static string GetCompany()
        {
            string vCompany = "";
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 

                        string[] userDetail = ticket.UserData.Split('#');
                        if (!String.IsNullOrEmpty(userDetail[2]))
                            vCompany = userDetail[2];
                    }
                }
            }
            return vCompany;
        }//end function GetUserName

        public static int GetAsAdmin()
        {
            var vAdmin = 0;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 

                        string[] userDetail = ticket.UserData.Split('#');
                        if (!String.IsNullOrEmpty(userDetail[3]))
                            vAdmin = Convert.ToInt16(userDetail[3]);
                    }
                }
            }
            return vAdmin;
        }//end function GetUserName

    }

    
}