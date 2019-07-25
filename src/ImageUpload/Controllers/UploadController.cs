using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Threading;

namespace ImageUpload
{
    public class UploadController : ApiController
    {
        public async Task<HttpResponseMessage> PostFile()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                ImageProcessor imgProc = new ImageProcessor();

                // MULTIPART REQUESTS
                if (Request.Content.IsMimeMultipartContent())
                {
                    string root = Utils.getWorkDirectory();
                    var provider = new MultipartFormDataStreamProvider(root);

                    await Request.Content.ReadAsMultipartAsync(provider);

                    foreach (var file in provider.FileData)
                    {
                        FileInfo fileInfo = new FileInfo(file.LocalFileName);
                        if (fileInfo.Length > 0)
                        {
                            imgProc.fromFile(root + fileInfo.Name);
                        }
                        try { File.Delete(root + fileInfo.Name); }
                        catch { }
                    }
                }
                else
                // FORMS AND JSON REQUESTS
                {
                    //
                    var readTask = Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var rawResponse = readTask.GetAwaiter().GetResult();
                    var res = rawResponse.ToString();

                    string[] postData = res.Split('&');
                    for(var i=0; i<postData.Length; i++) {
                        string[] vars = postData[i].Split('=');

                        if (vars.Length > 1 && vars[1].Length>0)
                        {
                            vars[0] = System.Web.HttpUtility.UrlDecode(vars[0]);
                            vars[1] = System.Web.HttpUtility.UrlDecode(vars[1]);

                            switch(vars[0].Replace("[]", "")){
                                case "image_url":
                                    imgProc.fromUrl(vars[1]);
                                    break;
                                case "image_base64":
                                    imgProc.from64(vars[1]);
                                    break;
                            }
                        }
                    }

                }

                response.Content = new StringContent("{\"result\":\"success\"}");
                response.StatusCode = HttpStatusCode.OK;

            }
            catch (System.Exception ex)
            {
                response.Content = new StringContent("{\"error\":\"" + System.Web.HttpUtility.JavaScriptStringEncode(ex.Message) + "\",\"details\":\"" + System.Web.HttpUtility.JavaScriptStringEncode(ex.StackTrace) + "\"}");
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            response.Content.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Content.Headers.Add("Access-Control-Allow-Methods", "POST, GET, PUT, DELETE, OPTIONS");
            response.Content.Headers.Add("Access-Control-Allow-Headers", "authorization,timezone,Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");

            return response;
        }
    }
}