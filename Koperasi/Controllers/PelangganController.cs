using koperasi.Models;
using koperasi.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace koperasi.Controllers
{
    public class PelangganController : Controller
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("PelangganController");
        private pelangganService _pelangganService;

        public PelangganController()
        {
            _pelangganService = new pelangganService();
        }


        public ActionResult Index()
        {
            //Log.Debug(DateTime.Now + " ====>>>Index, MASUK CONTROLLER");

            var vHasil = new anggotaModels();

            var kdPelanggan = ConfigModels.GetNoAnggota();
            vHasil = _pelangganService.GetInfoAnggota(kdPelanggan);
            
            //Log.Debug(DateTime.Now + " ====>>> Index, Jml Record Hasil : " + vHasil.nama_anggota);

            return View(vHasil);
        }

        [HttpPost]
        public dynamic GetPelangganList()
        {
            // Get Data
            var kdPelanggan = ConfigModels.GetNoAnggota();
            var vList = _pelangganService.GetListHutang(kdPelanggan) as IEnumerable<pelangganModels>;
            var json = JsonConvert.SerializeObject(vList);

           // Log.Debug(DateTime.Now + " GetPelangganList ====>>>>>> Jumlah Data : " + vList.Count().ToString());

            return json;
        }

        public ActionResult ListPerCompany()
        {
            //Log.Debug(DateTime.Now + " ====>>>Index, MASUK CONTROLLER");

            var vHasil = new anggotaModels();

            var kdPelanggan = ConfigModels.GetNoAnggota();
            Log.Debug(DateTime.Now + " ====>>> No Anggota : " + kdPelanggan + " As Admin " + ConfigModels.GetAsAdmin());

            if (ConfigModels.GetAsAdmin() == 0)
            {
                Response.Redirect("~/error_page.html");
            }

            
            return View();
        }

        [HttpPost]
        public dynamic GetHutangListByCompany()
        {
            // Get Data
            var kdCompany = ConfigModels.GetCompany();
            var vList = _pelangganService.GetListHutangByCompany(kdCompany) as IEnumerable<pelangganModels>;
            var json = JsonConvert.SerializeObject(vList);

             Log.Debug(DateTime.Now + " GetPelangganList ====>>>>>> Jumlah Data : " + vList.Count().ToString());

            return json;
        }
    }
}
