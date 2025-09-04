using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFutureLib.Include
{
    public class HttpClientFx
    {
        private HttpClient _httpClient;

        public virtual void Reset()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
            _httpClient = new HttpClient();
        }


        public bool SendRequest(
            out string result,
            out HttpHeaders hdrs,
            HTTPREQUEST_TYPE req_type,
            string url,
            string token,
            Dictionary<string, string> param_list,
            string media_type = "")
        {
            string param = "";
            if (param_list != null && param_list.Count > 0)
            {
                switch (req_type)
                {
                    case HTTPREQUEST_TYPE.GET:
                    case HTTPREQUEST_TYPE.POST:
                        using (var encodedContent = new FormUrlEncodedContent(param_list))
                            param = encodedContent.ReadAsStringAsync().Result;
                        break;
                    case HTTPREQUEST_TYPE.JSON:
                        param = "{" + string.Join(", ", param_list.Select(d => String.Format("\"{0}\": \"{1}\"", d.Key, d.Value))) + "}";
                        break;
                }

            }
            return SendRequest(out result, out hdrs, req_type, url, token, param, media_type);
        }

        public bool SendRequest(
            out string result,
            out HttpHeaders hdrs,
            HTTPREQUEST_TYPE req_type,
            string url,
            string token,
            string param = "",
            string media_type = "")
        {
            result = "";
            hdrs = null;

            string stFullUrl = url;
            HttpMethod method;
            switch (req_type)
            {
                case HTTPREQUEST_TYPE.GET:
                    method = HttpMethod.Get;
                    if (!string.IsNullOrEmpty(param))
                    {
                        stFullUrl += "?" + param;
                        param = "";
                    }
                    break;
                case HTTPREQUEST_TYPE.POST:
                case HTTPREQUEST_TYPE.JSON:
                    method = HttpMethod.Post;
                    break;
                default:
                    return false;
            }

            bool ret = false;
            using (var request = new HttpRequestMessage(method, stFullUrl))
            {
                if (!String.IsNullOrEmpty(param))
                {
                    if (String.IsNullOrEmpty(media_type))
                    {
                        switch (req_type)
                        {
                            case HTTPREQUEST_TYPE.POST:
                                media_type = "application/x-www-form-urlencoded";
                                break;
                            case HTTPREQUEST_TYPE.JSON:
                                media_type = "application/json";
                                break;
                            default:
                                media_type = "text/plain";
                                break;
                        }
                    }
                    request.Content = new StringContent(param, Encoding.UTF8, media_type);
                }
                if (!String.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("auth-token", token);
                }
                try
                {
                    var response = _httpClient.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var tmp = response.Content.ReadAsByteArrayAsync().Result;
                        result = Encoding.UTF8.GetString(tmp);
                        hdrs = response.Headers;
                        ret = true;
                    } else
                    {
                        var tmp = response.Content.ReadAsByteArrayAsync().Result;
                        result = Encoding.UTF8.GetString(tmp);
                        hdrs = response.Headers;
                        ret = false;
                    }
                }
                catch
                {
                    ret = false;
                }
            }
            return ret;
        }

        public static string GetHeaderKeyValue(HttpHeaders headers, string header_name, string key_name)
        {
            string res = "";
            IEnumerable<String> value_list;
            if (headers.TryGetValues(header_name, out value_list))
            {
                res = value_list.FirstOrDefault(d => d.IndexOf(key_name + "=") == 0);
                if (!string.IsNullOrEmpty(res))
                {
                    int len = res.IndexOf(";", key_name.Length + 1);
                    if (len < 0)
                        len = res.Length - key_name.Length - 1;
                    else
                        len -= key_name.Length + 1;
                    res = res.Substring(key_name.Length + 1, len);
                }
            }
            return res;
        }
    }
}
