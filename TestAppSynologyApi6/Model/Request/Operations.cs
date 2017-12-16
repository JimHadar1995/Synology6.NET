using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;

namespace TestAppSynologyApi6.Model.Request {
    public class Operations  {
        private string _url;
        RestClient restClient;
        private string _sid;
        public Operations(string url) {
            this._url = url;        
        }

        private string Execute(string url) {
            //в параметры добавляем sid, который получили при выполнении входа в api с помощью метода LogIn
            //если параметра не указывать, то доступа к функционалу апи не будет
            if (this._sid != string.Empty) {
                url += $"&_sid={this._sid}";
            }
            this.restClient = new RestClient( url );
            RestRequest request = new RestRequest( Method.GET );
            return restClient.Execute( request ).Content;
        }


        public BaseResponse LogIn(string account,
                                  string passwd) {
            Model.Response.LogIn logIn = SimpleJson.SimpleJson.DeserializeObject<Response.LogIn>( Execute( this.GetUrlLogIn( account, passwd ) ) );
            this._sid = logIn.success ? logIn.data.sid : string.Empty;
            return logIn;
        }

        public BaseResponse LogOut() {
            string url = this._url + $"auth.cgi?api=SYNO.API.Auth&version=1&method=logout&session=FileStation&_sid={this._sid}";
            BaseResponse response = SimpleJson.SimpleJson.DeserializeObject<BaseResponse>( Execute( url ) );
            return response;
        }

        public Response.CreateShareLink CreateShareLink(string path, DateTime? dateExpired = null) {
            string url = this.GetUrlCreateShareLink( path, dateExpired );
            return SimpleJson.SimpleJson.DeserializeObject<Response.CreateShareLink>( Execute( url ) );
        }

        public Response.CreateFolder CreateFolder(string folder_path, string name) {
            string url = this.GetUrlCreateFolder( folder_path, name );
            return SimpleJson.SimpleJson.DeserializeObject<Response.CreateFolder>( Execute( url ) );
        }

        public Response.Delete Delete(string path, bool blocking) {
            string url = this.GetUrlDelete( path, blocking );
            return SimpleJson.SimpleJson.DeserializeObject<Response.Delete>( Execute( url ) );
        }

        public Response.Delete DeleteStatus(string taskid) {
            string url = this.GetUrlDeleteStatus( taskid );
            return SimpleJson.SimpleJson.DeserializeObject<Response.Delete>( Execute( url ) );
        }

        public Response.Rename Rename(string path, string name) {
            string url = this.GetUrlRename( path, name );
            return SimpleJson.SimpleJson.DeserializeObject<Response.Rename>( Execute( url ) );
        }
        /// <summary>
        /// ovewrite = true, еслиии нужно перезаписать файлы при совпадении
        /// remove_src - это не копирование, а перемещение файла/каталога
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dest_folder_path"></param>
        /// <param name="overwrite"></param>
        /// <param name="remove_src"></param>
        /// <returns></returns>
        public Response.CopyMove CopyMove(string path,
                                      string dest_folder_path,
                                      bool overwrite = false,
                                      bool remove_src = false) {
            string url = this.GetUrlCopyMove( path, dest_folder_path, overwrite, remove_src );
            return SimpleJson.SimpleJson.DeserializeObject<Response.CopyMove>( Execute(url) );
        }

        public Response.CopyMove CopyMoveStatus(string taskid) {
            string url = this.GetUrlCopyMoveStatus( taskid );
            return SimpleJson.SimpleJson.DeserializeObject<Response.CopyMove>( Execute(url) );
        }

        public Response.Compress CompressFiles(string path,
                                      string dest_file_path,
                                      string level = "moderate",
                                      string mode = "add",
                                      string format = "zip") {
            string url = this.GetUrlCompress( path, dest_file_path, level, mode, format );
            return SimpleJson.SimpleJson.DeserializeObject<Response.Compress>( Execute( url ) );
        }

        public Response.Compress CompressFilesStatus(string taskid) {
            string url = this.GetUrlCompressStatus( taskid );
            return SimpleJson.SimpleJson.DeserializeObject<Response.Compress>( Execute( url ) );
        }

        public BaseResponse UploadFile(FileInfo fileName, 
                                       string destinationFilePath, 
                                       bool createParents = true,
                                       bool overwrite = false) {
            string url = this.GetUrlUploadFile();
            if (this._sid != string.Empty) {
                url += $"&_sid={this._sid}";
            }
            this.restClient = new RestClient( url );
            RestRequest request = new RestRequest( Method.POST );
            request.AddParameter( "path", destinationFilePath );
            request.AddParameter( "create_parents", createParents.ToString().ToLower() );
            request.AddParameter( "overwrite", overwrite.ToString().ToLower() );
            request.AddFile( fileName.Name, fileName.FullName );
            IRestResponse response = this.restClient.Execute( request );
            return SimpleJson.SimpleJson.DeserializeObject<BaseResponse>( response.Content );
        }

        public string GetUrlLogIn(string account, 
                                    string passwd, 
                                    string session = "FileStation", 
                                    string format = "sid") {
            LogIn Params = new LogIn( _url + "auth.cgi", "SYNO.API.Auth", "login", "3" );
            Params.account = account;
            Params.passwd = passwd;
            Params.session = session;
            Params.format = format;
            return Params.ToString();
        }
        /// <summary>
        /// date_expired должен обязательно в запросе обрамляться в кавычки
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dateExpired">Срок действия ссылки: обязательно в формате yyyy-MM-dd</param>
        /// <returns></returns>
        public string GetUrlCreateShareLink(string path, DateTime? dateExpired = null) {
            BaseParams baseParams = new BaseParams(_url + "entry.cgi", "SYNO.FileStation.Sharing", "create", "3" );
            return baseParams.ToString() +
                   $"&path={path}" +
                   ( dateExpired != null ? $"&date_expired=\"{dateExpired.Value.ToString( "yyyy-MM-dd HH:mm:ss" )}\"" : string.Empty);
        }

        public string GetUrlCreateFolder(string folder_path, string name) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.CreateFolder", "create", "2" );
            return baseParams.ToString() +
                   $"&folder_path=\"{folder_path}\"&name=\"{name}\""; ;
        }
        public string GetUrlRename(string path, string name) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.Rename", "rename", "2" );
            return (baseParams.ToString() +
                   $"&path=\"{path}\"&name=\"{name}\"").Replace(@"\", string.Empty);
        }
        public string GetUrlCopyMove(string path, 
                                      string dest_folder_path,
                                      bool overwrite,
                                      bool remove_src) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.CopyMove", "start", "3" );
            return baseParams.ToString() +
                   $"&path=\"{path}\"&dest_folder_path=\"{dest_folder_path}\"" +
                   $"&overwrite={overwrite.ToString().ToLower()}&remove_src={remove_src.ToString().ToLower()}";
        }
        public string GetUrlCopyMoveStatus(string taskId) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.CopyMove", "status", "3" );
            return baseParams.ToString() +
                   $"&taskid=\"{taskId}\"";
        }
        /// <summary>
        /// Если deleteBlocking = true, то это блокирующее удаление, ответ не будет получен, 
        /// пока не произойдет окончательное удаление
        /// Если deleteBlocking = false, то ответ придет сразу и там будет taskid и можно будет с помощью метода  DeleteStatus
        /// отследить состояние удаления
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetUrlDelete(string path,
                                    bool deleteBlocking) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.Delete", (deleteBlocking ? "delete" : "start"), "2" );
            return baseParams.ToString() +
                   $"&path=\"{path}\"";
        }
        public string GetUrlDeleteStatus(string taskid) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.Delete", "status", "2" );
            return baseParams.ToString() +
                   $"&taskid=\"{taskid}\"";
        }        
        public string GetUrlCompress(string path, 
                                      string dest_file_path, 
                                      string level = "moderate",
                                      string mode = "add",
                                      string format = "zip") {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.Compress", "start", "3" );
            return baseParams.ToString() +
                   $"&path=[\"{path.Replace( ",", "\",\"" )}\"]" +
                   $"&dest_file_path=\"{dest_file_path}\"" +
                   $"&level={level}" +
                   $"&mode={mode}" +
                   $"&format={format}";
        }
        public string GetUrlCompressStatus(string taskid) {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.Compress", "status", "3" );
            return baseParams.ToString() +
                   $"&taskid={taskid}";
        }

        public string GetUrlUploadFile() {
            BaseParams baseParams = new BaseParams( _url + "entry.cgi", "SYNO.FileStation.Upload", "upload", "2" );
            return baseParams.ToString();
        }
    }
}
