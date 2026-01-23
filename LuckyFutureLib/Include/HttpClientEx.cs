using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFutureLib.Include
{
    public class HttpClientEx
    {
        private HttpClient _httpClient;
        private string _main_url = "";
        // _fx가 null이 되지 않도록 여기서 바로 생성하거나 생성자에서 생성해야 합니다.
        private readonly HttpClientFx _fx = new HttpClientFx();
        // 생성자에서 Fx 인스턴스를 받습니다.
        // 외부에서 주입받는 생성자 (선택 사항)
        public HttpClientEx(HttpClientFx fx)
        {
            _fx = fx ?? new HttpClientFx(); // fx가 null이면 새로 생성
        }
        public virtual void Reset()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
            _httpClient = new HttpClient();
        }

        public string MainUrl
        {
            get => _main_url;
            set => _main_url = value;
        }

        //public bool SendRequest(
        //    out string result,
        //    out HttpHeaders hdrs,
        //    HTTPREQUEST_TYPE req_type,
        //    string url,
        //    Dictionary<string, string> param_list,
        //    string media_type = "")
        //{
        //    string param = "";
        //    if (param_list != null && param_list.Count > 0)
        //    {
        //        switch (req_type)
        //        {
        //            case HTTPREQUEST_TYPE.GET:
        //            case HTTPREQUEST_TYPE.POST:
        //                using (var encodedContent = new FormUrlEncodedContent(param_list))
        //                    param = encodedContent.ReadAsStringAsync().Result;
        //                break;
        //            case HTTPREQUEST_TYPE.JSON:
        //                param = "{" + string.Join(", ", param_list.Select(d => String.Format("\"{0}\": \"{1}\"", d.Key, d.Value))) + "}";
        //                break;
        //        }

        //    }
        //    return SendRequest(out result, out hdrs, req_type, url, param, media_type);
        //}
        public bool SendRequest(
    out string result,
    out HttpHeaders hdrs,
    HTTPREQUEST_TYPE req_type,
    string url,
    Dictionary<string, string> param_list,
    string media_type = "")
        {
            // _fx는 HttpClientFx의 인스턴스라고 가정합니다.
            var task = Task.Run(async () =>
            {
                string param = "";
                if (param_list != null && param_list.Count > 0)
                {
                    switch (req_type)
                    {
                        case HTTPREQUEST_TYPE.GET:
                        case HTTPREQUEST_TYPE.POST:
                            using (var encodedContent = new FormUrlEncodedContent(param_list))
                                // 내부에서도 비동기로 처리하여 데드락 방지
                                param = await encodedContent.ReadAsStringAsync().ConfigureAwait(false);
                            break;
                        case HTTPREQUEST_TYPE.JSON:
                            param = "{" + string.Join(", ", param_list.Select(d => String.Format("\"{0}\": \"{1}\"", d.Key, d.Value))) + "}";
                            break;
                    }
                }

                // HttpClientFx에 있는 비동기 함수를 호출 (토큰은 빈값으로 전달)
                return await _fx.SendRequestInternalAsync(req_type, url, "", param, media_type).ConfigureAwait(false);
            });

            // 기존 호출자(30여곳)를 위해 결과를 동기적으로 반환
            var finalResult = task.GetAwaiter().GetResult();

            result = finalResult.result;
            hdrs = finalResult.hdrs;
            return finalResult.success;
        }
        //public bool SendRequest(
        //    out string result,
        //    out HttpHeaders hdrs,
        //    HTTPREQUEST_TYPE req_type,
        //    string url,
        //    string param = "",
        //    string media_type = "")
        //{
        //    result = "";
        //    hdrs = null;

        //    string stFullUrl = url;
        //    HttpMethod method;
        //    switch (req_type)
        //    {
        //        case HTTPREQUEST_TYPE.GET:
        //            method = HttpMethod.Get;
        //            if (!string.IsNullOrEmpty(param))
        //            {
        //                stFullUrl += "?" + param;
        //                param = "";
        //            }
        //            break;
        //        case HTTPREQUEST_TYPE.POST:
        //        case HTTPREQUEST_TYPE.JSON:
        //            method = HttpMethod.Post;
        //            break;
        //        default:
        //            return false;
        //    }

        //    bool ret = false;
        //    using (var request = new HttpRequestMessage(method, stFullUrl))
        //    {
        //        if (!String.IsNullOrEmpty(param))
        //        {
        //            if (String.IsNullOrEmpty(media_type))
        //            {
        //                switch (req_type)
        //                {
        //                    case HTTPREQUEST_TYPE.POST:
        //                        media_type = "application/x-www-form-urlencoded";
        //                        break;
        //                    case HTTPREQUEST_TYPE.JSON:
        //                        media_type = "application/json";
        //                        break;
        //                    default:
        //                        media_type = "text/plain";
        //                        break;
        //                }
        //            }
        //            request.Content = new StringContent(param, Encoding.UTF8, media_type);
        //        }
        //        try
        //        {
        //            var response = _httpClient.SendAsync(request).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var tmp = response.Content.ReadAsByteArrayAsync().Result;
        //                result = Encoding.UTF8.GetString(tmp);
        //                hdrs = response.Headers;
        //                ret = true;
        //            }
        //        }
        //        catch
        //        {
        //            ret = false;
        //        }
        //    }
        //    return ret;
        //}
        public bool SendRequest(
    out string result,
    out HttpHeaders hdrs,
    HTTPREQUEST_TYPE req_type,
    string url,
    string param = "",
    string media_type = "")
        {
            // 1. Task.Run을 사용하여 별도의 작업 스레드에서 실행함으로써 UI 데드락을 방지합니다.
            // _fx는 HttpClientFx의 인스턴스 멤버 변수입니다.
            var task = Task.Run(async () =>
            {
                // 2. HttpClientFx 클래스에 구현된 비동기 메서드(SendRequestAsync)를 호출합니다.
                // 토큰이 없는 버전이므로 token 인자에 빈 문자열("")을 전달합니다.
                return await _fx.SendRequestInternalAsync(req_type, url, "", param, media_type).ConfigureAwait(false);
            });

            // 3. 기존 호출자(30여 곳)들을 위해 결과를 동기적으로 반환합니다.
            // UI 스레드가 아닌 작업 스레드에서 await가 완료되었으므로 안전하게 결과를 가져옵니다.
            var finalResult = task.GetAwaiter().GetResult();

            result = finalResult.result;
            hdrs = finalResult.hdrs;
            return finalResult.success;
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
    public enum HTTPREQUEST_TYPE : int
    {
        GET,
        POST,
        JSON
    }
}
