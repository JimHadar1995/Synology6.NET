using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Response {
    public class DataCopyMove {
        #region это при вызове метода start
        public string taskid { get; set; }
        #endregion
        #region это при вызове метода status
        public List<Error> errors { get; set; }
        public string dest_folder_path { get; set; }
        public bool finished { get; set; }
        public string path { get; set; }
        public string processed_size { get; set; }
        public double progress { get; set; }
        public string total { get; set; }
        #endregion
    }
    public class CopyMove : BaseResponse {
        public DataCopyMove data { get; set; }
    }
}
