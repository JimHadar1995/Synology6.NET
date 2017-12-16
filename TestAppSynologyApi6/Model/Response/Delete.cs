using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Response {
    /// <summary>
    /// Здесь содержится все и в случае, когда вызван метод start (будет заполнен только taskid) и когда вызван метод status
    /// </summary>
    public class DataDelete {
        public string taskid { get; set; }
        public bool finished { get; set; }
        public string path { get; set; }
        public double processed_num { get; set; }
        public string processing_path { get; set; }
        public double progress { get; set; }
        public double total { get; set; }
    }
    public class Delete : BaseResponse {
        public DataDelete data { get; set; }
    }
}
