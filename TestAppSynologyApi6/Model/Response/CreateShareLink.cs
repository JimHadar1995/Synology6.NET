using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Response {
    public class DataLink {
        public List<link> links { get; set; }
    }
    public class link {
        public string date_available { get; set; }
        public string date_expired { get; set; }
        public bool enable_upload { get; set; }
        public bool expire_times { get; set; }
        public bool has_password { get; set; }
        public string link_owner { get; set; }
        public string id { get; set; }
        public bool isFolder { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string uid { get; set; }
        public string url { get; set; }
    }
    public class CreateShareLink : BaseResponse {
        public DataLink data { get; set; }
    }
}