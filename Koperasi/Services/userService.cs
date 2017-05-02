using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using koperasi.Repositories;
using koperasi.Models;
using log4net;
using koperasi.GeneralClasses;
using System.Data.OleDb;

namespace koperasi.Services
{
    public class userService
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("UserService");
        private readonly UserRepo _repoUser;
        private readonly ManageString mString;

        public userService()
        {
            _repoUser = new UserRepo();
            mString = new ManageString();
        }

        public ResponseModel InsertPassword(passwordModels pModel)
        {
            string ePass = mString.Encrypt(pModel.password, ConfigModels.eKeyStr);

            ResponseModel respModel = new ResponseModel();
            respModel.isValid = true;
            respModel.message = "OK";
            respModel.objResult = null;

            try
            {
                var vModel = new passwordModels();
                vModel.no_anggota = pModel.no_anggota;
                vModel.password = ePass;
                vModel.keterangan = pModel.keterangan;

                //Log.Debug(DateTime.Now + " INSERT Pass SERVICE 1 ==>>> No Anggota : " + pModel.no_anggota + " Pass : " + ePass);
                _repoUser.InsertPassword(vModel);
                //Log.Debug(DateTime.Now + " INSERT Pass SERVICE 2 ==>>> No Anggota : " + pModel.no_anggota + " Pass : " + ePass);

            }
            catch (Exception ex)
            {
                respModel.isValid = false;
                respModel.message = "Insert Master Password Tidak Berhasil...\nPesan Error : " + ex.Message;
                
            }
            return respModel;
        }


        public void InsertAllPassword()
        {
            var vListAng = _repoUser.getAllAnggotaList();
            Log.Debug(DateTime.Now + " Get ALL ANGGOTA LIST ====>>>>>> JML: " + vListAng.Count());

            foreach (var item in vListAng)
            {
                var m = new passwordModels();

                m.no_anggota = item.no_anggota;
                m.password = item.no_anggota.Trim().ToUpper();
                m.keterangan = item.keterangan;

                InsertPassword(m);

                Log.Debug(DateTime.Now + " INSERT Pass SERVICE ==>>> No Anggota : " + item.no_anggota + " Pass : " + item.password);
            }
        }

        public passwordModels UpdatePassword(passwordModels pModel)
        {
            string ePass = mString.Encrypt(pModel.password, ConfigModels.eKeyStr);

            passwordModels respModel = new passwordModels();
            //respModel.isValid = true;
            //respModel.message = "OK";
            //respModel.objResult = null;

            try
            {
                var vModel = new passwordModels();
                vModel.no_anggota = pModel.no_anggota;
                vModel.password = ePass;
                vModel.keterangan = pModel.keterangan;

                _repoUser.UpdatePassword(vModel);
            }
            catch (Exception ex)
            {
                //respModel.isValid = false;
                //respModel.message = "Update Master Password Tidak Berhasil...\nPesan Error : " + ex.Message;

            }
            return respModel;
        }

        public passwordModels GetPassword(string pNoAnggota)
        {
            var dPass = new passwordModels();
            var dPass2 = new passwordModels();
            string ePass = "";

            try
            {
                dPass = _repoUser.GetPassword(pNoAnggota);

                ePass = mString.Decrypt(dPass.password, ConfigModels.eKeyStr);

                //Log.Debug(DateTime.Now + " GetPasswordService ===>>> No Anggota : " + pNoAnggota + " Password User: " + dPass.password + " Password Descrypt: " + ePass);

                dPass2.id = dPass.id;
                dPass2.no_anggota = dPass.no_anggota;
                dPass2.nama_anggota = dPass.nama_anggota;
                dPass2.password = ePass;
                dPass2.keterangan = dPass.keterangan;
                dPass2.sektor = dPass.sektor;
                dPass2.as_admin = dPass.as_admin;
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now + " GetPassword Error ====>>>>>> ", ex);
            }
            return dPass2;
        }

    }
}