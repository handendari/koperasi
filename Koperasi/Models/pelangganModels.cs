using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace koperasi.Models
{
    public class pelangganModels
    {
        public int id { get; set; }
        public string no_trans { get; set; }
        public System.Nullable<DateTime> tgl_registrasi { get; set; }
        public string nama { get; set; }
        public string alamat { get; set; }
        public string kd_pelanggan { get; set; }
        public decimal batas_kredit { get; set; }
        public decimal persen_bunga { get; set; }
        public int jangka_waktu { get; set; }
        public decimal bunga_per_bulan { get; set; }
        public decimal pokok_per_bulan { get; set; }
        public decimal sisa_pinjaman { get; set; }
        public decimal angsuran_per_bulan { get; set; }
        public decimal simpanan_wajib { get; set; }
        public decimal simpanan_tmk { get; set; }
    }

    public class anggotaModels
    {
        public int id { get; set; }
        public string no_anggota { get; set; }
        public string nama_anggota { get; set; }
        public DateTime tgl_registrasi { get; set; }
        public string alamat { get; set; }
        public string kota { get; set; }
        public string kantor { get; set; }
        public decimal simpanan_wajib { get; set; }
        public decimal simpanan_tmk { get; set; }
    }
}