using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppSynologyApi6.Model.Response {
    public class DataCreateFolder {
        public List<Folder> folders { get; set; }
    }
    public class CreateFolder : BaseResponse {
        public DataCreateFolder data { get; set; }
    }
}
