using System;
using System.Collections.Generic;
using System.Web;


using System.ComponentModel;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

using System.Threading;
using System.Net;


namespace ImageUpload
{
    public class Utils
    {
        static public MemoryStream getStreamFromURL(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                byte[] respRaw = new byte[1024 * 1024];
                int read = 0, pos = 0;

                BinaryReader stream = new BinaryReader(response.GetResponseStream());
                MemoryStream memStream = new MemoryStream();
                while ((read = stream.Read(respRaw, 0, 1024 * 1024)) > 0)
                {
                    memStream.Write(respRaw, 0, read);
                    pos += read;
                }

                stream.Close();
                response.Close();
                response = null;
                request = null;

                memStream.Seek(0, SeekOrigin.Begin);

                return memStream;
            }
            catch (Exception ex) { throw ex; }
        }
        static public string getWorkDirectory()
        {
            try
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
            catch
            {
                return Directory.GetCurrentDirectory() + "/";
            }
        }
    }
}