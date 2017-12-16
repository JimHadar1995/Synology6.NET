using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model {
    public class Error {
        public int code { get; set; }
    }
    public class BaseResponse {
        public Error error { get; set; }
        public bool success { get; set; }
    }
}