using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Response {
    public class DataCompress {
        public string taskid { get; set; }
        public bool finished { get; set; }
        public string dest_file_path { get; set; }
        public double progress { get; set; }
    }
    public class Compress : BaseResponse {
        public DataCompress data { get; set; }
    }
}
