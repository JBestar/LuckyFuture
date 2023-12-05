using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;


namespace LuckyFutureLib.Include
{
    public class WebClientEx
    {

        public WebClientEx()
        {

        }


        public bool UploadFile(string url, string filePath, string sessionKey, string sessionValue)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            
            WebClient _webClient = new WebClient();
            try
            {
                _webClient.Headers.Add(sessionKey, sessionValue);
                _webClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] respArray = _webClient.UploadFile(url, filePath);

                string respStr = Encoding.ASCII.GetString(respArray);
                
            }
            catch (Exception)
            {
                return false;
            }
            if (_webClient != null)
                _webClient.Dispose();
            return true;
        }


    }
}
