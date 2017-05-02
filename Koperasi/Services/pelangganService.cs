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
    public class pelangganService
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("PelangganService");
        private readonly PelangganRepo _repoPelanggan;
        private readonly ManageString mString;

        public pelangganService()
        {
            _repoPelanggan = new PelangganRepo();
            mString = new ManageString();
        }

        public List<pelangganModels> GetListHutang(string kdPelanggan)
        {
            var listPelanggan = new List<pelangganModels>();

            try
            {
                listPelanggan = _repoPelanggan.GetListHutang(kdPelanggan);
            }
            catch(Exception ex)
            {
                Log.Error(DateTime.Now + " GetPelangganList ====>>>>>> ", ex);
            }
            return listPelanggan;
        }

        public List<pelangganModels> GetListHutangByCompany(string kdCompany)
        {
            var listPelanggan = new List<pelangganModels>();

            try
            {
                listPelanggan = _repoPelanggan.GetListHutangByCompany(kdCompany);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now + " GetListHutangByCompany ", ex);
            }
            return listPelanggan;
        }


        public anggotaModels GetInfoAnggota(string kdPelanggan)
        {
            var infoAng = new anggotaModels();

            try
            {
                infoAng = _repoPelanggan.GetInfoAnggota(kdPelanggan);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now + " GetInfoAnggota ====>>>>>> No Anggota : " + kdPelanggan , ex);
            }
            return infoAng;
        }

    }
}