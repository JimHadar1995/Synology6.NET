using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppSynologyApi6.Model.Request;
using TestAppSynologyApi6.Model;
using System.Threading;

namespace TestAppSynologyApi6 {
    
    class Program {
        public static string StatusOper(BaseResponse response) {
            return response.success ? "success" : $"error ({response.error.code})";
        }
        static void Main(string[] args) {
            Operations opers = new Operations( "http://192.168.56.102:5000/webapi/" );
            Model.Response.LogIn response = (Model.Response.LogIn)opers.LogIn( "jimhadar", "demon1995" );
            Console.WriteLine( "LogIn is {0}\n sid: {1} ", StatusOper(response), response.data.sid );
            if (response.success) {
                Model.Response.CreateShareLink shareLink = opers.CreateShareLink( "/dsm/xpenology" ); //DateTime.Now );
                Console.WriteLine( $"\n-------------------\nCreate share link is {StatusOper(shareLink)}:" );
                if (shareLink.success) {
                    Console.WriteLine( $"path:{shareLink.data.links[0].path}" );
                    Console.WriteLine( $"link: {shareLink.data.links[0].url}" );
                    Console.WriteLine( $"date_expired: {shareLink.data.links[0].date_expired}" );
                }
                Console.WriteLine( "---------------CreateFolder-------------------" );
                Model.Response.CreateFolder createFolder = opers.CreateFolder( "/dsm", "123" );

                Console.WriteLine( $"Folder create is {StatusOper(createFolder)}" );

                Console.WriteLine( "-------------------DeleteFolder with on blocking--------------------------" );
                Model.Response.Delete delete = opers.Delete( "/dsm/1.zip", false );
                if (delete.success) {
                    Console.WriteLine( $"taskid: {delete.data.taskid}" );
                    string taskid = delete.data.taskid;
                    //отслеживаем статус
                    while (true) {
                        delete = opers.DeleteStatus( taskid );
                        if (delete.success && delete.error == null)
                            Console.WriteLine( $"DeleteStatus: {delete.data.progress}" );
                        else
                            break;
                    }
                }
                Console.WriteLine( "-------------------Rename folder--------------------------" );
                Model.Response.Rename rename = opers.Rename( "/dsm/xpenology", "234" );
                Console.WriteLine( $"Rename folder is {StatusOper(rename)}" );
                Console.WriteLine( "-------------------Rename file--------------------------" );
                rename = opers.Rename( "/dsm/DSM_DS3615xs_5967.pat", "123.pat" );
                Console.WriteLine( $"Rename file is {StatusOper( rename )}" );

                Console.WriteLine( "-------------------Copy folder--------------------------" );
                Model.Response.CopyMove copyMove = opers.CopyMove( "/dsm/xp", "/dsm/1" );
                Console.WriteLine( $"Copy folder is {StatusOper( copyMove )}" );
                if (copyMove.success) {
                    Console.WriteLine( $"taskid: {copyMove.data.taskid}" );
                    string taskid = copyMove.data.taskid;
                    //отслеживаем статус
                    while (true) {
                        copyMove = opers.CopyMoveStatus( taskid );
                        if (copyMove.success) {
                            if (copyMove.data.errors == null) {
                                Console.WriteLine( $"CopyStatus: {copyMove.data.progress}" );
                                Thread.Sleep( 1000 );
                            }
                            else {
                                Console.WriteLine( $"CopyStatus: copy is error" );
                            }
                        }
                        else
                            break;                        
                    }
                }

                Console.WriteLine( "-------------------Copy file--------------------------" );
                copyMove = opers.CopyMove( "/dsm/Шилдт. Java 8 Полное руководство.pdf", "/dsm/1", true, true );
                Console.WriteLine( $"Copy file is {StatusOper( copyMove )}" );
                if (copyMove.success) {
                    Console.WriteLine( $"taskid: {copyMove.data.taskid}" );
                    string taskid = copyMove.data.taskid;
                    //отслеживаем статус
                    while (true) {
                        copyMove = opers.CopyMoveStatus( taskid );
                        if (copyMove.success) {
                            if (copyMove.data.errors == null) {
                                Console.WriteLine( $"CopyStatus: {copyMove.data.progress}" );
                                Thread.Sleep( 1000 );
                            }
                            else {
                                Console.WriteLine( $"CopyStatus: copy is error" );
                            }
                        }
                        else
                            break;
                    }
                }

                Console.WriteLine( "-------------------Compress files--------------------------" );
                Model.Response.Compress compress = opers.CompressFiles( "/dsm/1,/dsm/123,/dsm/DSM_DS3615xs_15217.pat", "/dsm/1.zip" );
                Console.WriteLine( $"Compress file is {StatusOper( compress )}" );
                if (compress.success) {
                    Console.WriteLine( $"taskid: {compress.data.taskid}" );
                    string taskid = compress.data.taskid;
                    //отслеживаем статус
                    while (true) {
                        compress = opers.CompressFilesStatus( taskid );
                        if (compress.success) {
                            if (compress.data != null) {
                                Console.WriteLine( $"Compress is finished: {compress.data.progress}" );
                                Thread.Sleep( 1000 );
                            }
                            else {
                                Console.WriteLine( $"Compress status: Compress is error" );
                            }
                        }
                        else
                            break;
                    }
                }


                Console.WriteLine( "-------------------Upload files--------------------------" );
                BaseResponse baseResponse = opers.UploadFile( new System.IO.FileInfo( @"F:\Шилдт. Java 8 Полное руководство.pdf" ), "/dsm/1234", true );
                Console.WriteLine( $"Upload file is {StatusOper( baseResponse )}" );

                Console.WriteLine( "-------------------LogOut--------------------------" );
                baseResponse = opers.LogOut();
                Console.WriteLine($"LogOut is {StatusOper(baseResponse)}");
            }

            Console.ReadKey();
        }
    }
}