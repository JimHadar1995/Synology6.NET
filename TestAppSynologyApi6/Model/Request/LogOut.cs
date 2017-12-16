using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Request {
    public class LogOut : BaseParams {
        public LogOut(BaseParams baseParams)
            : base (baseParams.url, baseParams.api, baseParams.method, baseParams.version) {

        }
        public string session { get; set; }
        public override string ToString() {
            return base.ToString() + $"&session={session}";
        }
    }
}
