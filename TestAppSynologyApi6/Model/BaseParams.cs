using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model {
    public class BaseParams {
        public string url { get; set; }
        public string api { get; set; }
        public string method { get; set; }
        public string version { get; set; }
        public BaseParams(string url, 
                          string api, 
                          string method, 
                          string version) {
            this.url = url;
            this.api = api;
            this.method = method;
            this.version = version;
        }
        public override string ToString() {
            return $"{url}?api={api}&version={version}&method={method}";
        }
    }
}
