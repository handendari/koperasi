using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace koperasi.Models
{
    public class ResponseModel
    {
        public bool isValid { get; set; }
        public string message { get; set; }
        public object objResult { get; set; }
    }
}