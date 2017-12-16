using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Request {
    public class LogIn : BaseParams {
        public LogIn(string url, string api, string method, string version)
            : base(url, api, method, version) {

        }
        public string account { get; set; }
        public string passwd { get; set; }
        public string session { get; set; }
        public string format { get; set; }
        public override string ToString() {
            return base.ToString() + $"&account={account}&passwd={passwd}&session={session}&format={format}";
        }
    }
}
