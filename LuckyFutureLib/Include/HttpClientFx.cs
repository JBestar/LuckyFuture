using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // Stopwatch 사용을 위해 필요


namespace LuckyFutureLib.Include
{
    public class HttpClientFx
    {
        // 외부에서 로그 기능을 주입받습니다.
        // 1. 로그 담당자를 저장할 정적 변수
        public static ILogger Logger { get; set; }

        private HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10) // 10초 내 응답이 없으면 타임아웃 발생
        };

        public virtual void Reset()
        {
            if (_httpClient != null)
                _httpClient.Dispose();
            _httpClient = new HttpClient();
        }


        //public bool SendRequest(
        //    out string result,
        //    out HttpHeaders hdrs,
        //    HTTPREQUEST_TYPE req_type,
        //    string url,
        //    string token,
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
        //    return SendRequest(out result, out hdrs, req_type, url, token, param, media_type);
        //}
        public bool SendRequest(
    out string result,
    out HttpHeaders hdrs,
    HTTPREQUEST_TYPE req_type,
    string url,
    string token,
    Dictionary<string, string> param_list,
    string media_type = "")
        {
            // 1. Task.Run을 사용하여 내부 로직을 비동기로 처리하고 결과를 동기적으로 기다립니다.
            // 이렇게 하면 UI 스레드가 직접 네트워크를 기다리며 발생하는 데드락을 방지할 수 있습니다.
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
                                // .Result 대신 await 사용
                                param = await encodedContent.ReadAsStringAsync().ConfigureAwait(false);
                            break;
                        case HTTPREQUEST_TYPE.JSON:
                            param = "{" + string.Join(", ", param_list.Select(d => String.Format("\"{0}\": \"{1}\"", d.Key, d.Value))) + "}";
                            break;
                    }
                }

                // 2. 위에서 수정한 SendRequestAsync(비동기 버전)를 호출합니다.
                return await SendRequestInternalAsync(req_type, url, token, param, media_type).ConfigureAwait(false);
            });

            // 3. 동기 인터페이스를 유지하기 위해 결과를 기다림
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
        //    string token,
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
        //        if (!String.IsNullOrEmpty(token))
        //        {
        //            _httpClient.DefaultRequestHeaders.Clear();
        //            _httpClient.DefaultRequestHeaders.Add("auth-token", token);
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
        //            } else
        //            {
        //                var tmp = response.Content.ReadAsByteArrayAsync().Result;
        //                result = Encoding.UTF8.GetString(tmp);
        //                hdrs = response.Headers;
        //                ret = false;
        //            }
        //        }
        //        catch
        //        {
        //            ret = false;
        //        }
        //    }
        //    return ret;
        //}

        // 1. 기존 동기 함수 (참조하는 30곳을 수정하지 않아도 됨)
        public bool SendRequest(out string result, out HttpHeaders hdrs, HTTPREQUEST_TYPE req_type, string url, string token, string param = "", string media_type = "")
        {
            // Task.Run을 사용하여 별도 스레드에서 비동기 함수를 실행하고 결과를 기다립니다.
            // .Result를 직접 쓰는 것보다 데드락 위험이 현저히 낮아집니다.
            var task = Task.Run(() => SendRequestInternalAsync(req_type, url, token, param, media_type));

            // 결과를 기다림 (동기 인터페이스 유지)
            var response = task.GetAwaiter().GetResult();

            result = response.result;
            hdrs = response.hdrs;
            return response.success;
        }
        // 2. 실제 비동기 로직을 담은 내부 함수 (기존 로직을 여기로 옮김)
        public async Task<(bool success, string result, HttpHeaders hdrs)> SendRequestInternalAsync(
            HTTPREQUEST_TYPE req_type, string url, string token, string param, string media_type)
        {
            // ... (이전에 작성한 비동기 await 로직을 여기에 그대로 복사) ...
            // try-catch 내부에서 await _httpClient.SendAsync(request).ConfigureAwait(false) 사용
            // ...
            string result = "";
            HttpHeaders hdrs = null;
            string stFullUrl = url;
            HttpMethod method;

            // 1. HTTP 메서드 결정 및 URL 파라미터 처리
            switch (req_type)
            {
                case HTTPREQUEST_TYPE.GET:
                    method = HttpMethod.Get;
                    if (!string.IsNullOrEmpty(param))
                    {
                        // 기존 URL에 ?가 있는지 확인하여 결합
                        stFullUrl += (stFullUrl.Contains("?") ? "&" : "?") + param;
                    }
                    break;
                case HTTPREQUEST_TYPE.POST:
                case HTTPREQUEST_TYPE.JSON:
                    method = HttpMethod.Post;
                    break;
                default:
                    return (false, "Invalid Request Type", null);
            }

            using (var request = new HttpRequestMessage(method, stFullUrl))
            {
                // 2. Content 설정 (POST/JSON 등)
                if (method != HttpMethod.Get && !string.IsNullOrEmpty(param))
                {
                    if (string.IsNullOrEmpty(media_type))
                    {
                        // C# 버전과 관계없이 작동하는 전통적인 switch 문 방식입니다.
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

                // 3. 인증 토큰 설정 (공용 헤더 대신 개별 요청 헤더 사용 권장)
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Add("auth-token", token);
                }
                Stopwatch sw = Stopwatch.StartNew(); // 1. 실행 시간 측정 시작
                string logMsg = "";
                try
                {
                    // 4. 비동기 호출 (핵심: .Result 대신 await 사용)
                    // ConfigureAwait(false)는 UI 스레드 의존성을 끊어 데드락을 방지합니다.
                    using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        sw.Stop(); // 2. 응답 도착 시 측정 중지
                        hdrs = response.Headers;
                        var tmp = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        result = Encoding.UTF8.GetString(tmp);
                        // 성공 로그 기록 (예: [GET] https://... - Success (452ms))
                        logMsg = $"[{req_type}] {url} - Success ({sw.ElapsedMilliseconds}ms)";
                        // LuckyFuture.UI.FrmMain.Default를 통해 접근
                        Logger ?.WriteLog(logMsg);
                        return (response.IsSuccessStatusCode, result, hdrs);
                    }
                }
                catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
                {
                    sw.Stop(); // 타임아웃 시 측정 중지
                    logMsg = $"[{req_type}] {url} - Timeout! ({sw.ElapsedMilliseconds}ms)";
                    // LuckyFuture.UI.FrmMain.Default를 통해 접근
                    Logger?.WriteLog(logMsg);

                    return (false, "서버 응답 시간 초과", null);
                }
                catch (Exception ex)
                {
                    sw.Stop(); // 일반 오류 시 측정 중지
                    logMsg = $"[{req_type}] {url} - Error: {ex.Message} ({sw.ElapsedMilliseconds}ms)";
                    // LuckyFuture.UI.FrmMain.Default를 통해 접근
                    Logger?.WriteLog(logMsg);
                    return (false, ex.Message, null);
                }
            }
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
