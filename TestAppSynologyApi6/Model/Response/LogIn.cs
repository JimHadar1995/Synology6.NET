using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Response {
    public class dataLog {
        public string sid { get; set; }
    }

    public class LogIn : BaseResponse {
        public dataLog data { get; set; }
    }
}
