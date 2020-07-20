using System;
using System.Net;

namespace Owleye.Common.Util
{
    public class WebSiteUtil
    {
        public bool IsUrlAlive(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "HEAD";

                if (request.GetResponse() is HttpWebResponse response)
                {
                    var statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400)
                    {
                        return true;
                    }

                    if (statusCode >= 500 && statusCode <= 510)
                    {
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                //TODO, log this.
            }
            return false;
        }

    }
}
