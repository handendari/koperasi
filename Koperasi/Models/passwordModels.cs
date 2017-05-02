using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace koperasi.Models
{
    public class passwordModels
    {
        public int id { get; set; }
        public string no_anggota { get; set; }
        public string nama_anggota { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public string keterangan { get; set; }
        public string sektor { get; set; }
        public int as_admin { get; set; }
    }
}