using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web;
using AtmLib;
using AtmLib.Tracing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace AtmLib.Monitor {
    public class ApiConnector {

        private string sessionKey = "i1c172i4iu74l6i1urr4u34pjr2u7ln7c6gs9m4o";
        //	String sessionKey = "qte1tv35egs7f6vic1gsiml3a0t6c88p5avn2bgt";//local
        private string terminalId = "AFNG8846";
        private string machineGuid = "dfdfdfdfdfdffffddfdffd";
        private string login = "atmtestdef@just.cash";
        private string psw = "12345678";

        //String apiUrl = "http://localhost:8085/atm";
        private string apiUrl = "https://secure.just.cash/atm";

        private bool registerd = false;

        //Registration Kiosk (ATM) - olny once, sessionKey need to store for all other requests

        public ApiConnector() 
        {
            machineGuid = Log.GetDeviceGuid();
        }

        async public Task<Boolean> CheckServerStatus()
        {
            using (var log = new Logger(TraceLevel.Verbose, "ApiConnector.CheckServerStatus"))
            {
                string url = apiUrl + "/atm/alive";
                Uri uri = new Uri(url);

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var response = await httpClient.GetAsync(uri);
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<ApiAliveResponse>(jsonContent);

                        if (res != null && res.result == "ok")
                        {
                            log.WriteLine(TraceLevel.Verbose, jsonContent);
                            return true;
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (ArgumentNullException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteException(e);
                    }
                }

                return false;
            }
        }

        async public void RegisterAtm() 
        {
            using (var log = new Logger(TraceLevel.Verbose, "ApiConnector.RegisterAtm"))
            {
                string url = apiUrl + "/atm/atm/login";
                string prms = $"?username={login}&password={psw}&deviceKey={terminalId}";
                Uri uri = new Uri(url + prms);

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var values = new Dictionary<string, string>
                      {
                          { "username", login },
                          { "password", psw },
                          { "deviceKey", terminalId }
                      };

                        HttpContent content = new FormUrlEncodedContent(values);
                        var response = await httpClient.PostAsync(uri, content);

                        // var res = await response.Content.ReadFromJsonAsync<ApiResponse>();
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<ApiResponse>(jsonContent);

                        if (res != null && res.data != null && res.data.ContainsKey("sessionKey"))
                        {
                            sessionKey = res.data["sessionKey"];
                            registerd = true;
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (ArgumentNullException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteException(e);
                    }
                }
            }
        }

        async public void SendLogToServer(string guid, string logs) 
        {
            using (var log = new Logger(TraceLevel.Verbose, "ApiConnector.SendLogToServer"))
            {
                // Can not send log
                if (registerd == false)
                {
                    log.WriteLine(TraceLevel.Warning, "Can not send logs to server.");
                    return;
                }

                machineGuid = guid;
                var uriString = $"{apiUrl}/atm/atm/kiosk/log?sessionKey={sessionKey}";
                Uri uri = new Uri(uriString);

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var values = new Dictionary<string, string>
                        {
                            { "machine_guid", machineGuid },
                            { "terminal_id", terminalId },
                            { "log", logs }
                        };

                        /* TODO: This doesn't crash anymore when the log value is large, but I'm not 
                        sure it's working completely correctly. */
                        var json = JsonConvert.SerializeObject(values);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync(uri, content);

                        var res = await response.Content.ReadAsStringAsync();
                        log.WriteLine(TraceLevel.Verbose, $"Send Log Result: {res}");
                    }
                    catch (HttpRequestException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (ArgumentNullException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteException(e);
                    }
                }
            }
        }
        async public void SendImageToServer(string filename)
        {
            using (var log = new Logger(TraceLevel.Verbose, "ApiConnector.SendImageToServer"))
            {
                // Can not send log
                if (registerd == false)
                {
                    log.WriteLine(TraceLevel.Warning, "ApiConnector not register and can not send image to server.");
                    return;
                }

                var uriString = $"{apiUrl}/atm/atm/kiosk/image?terminal_id={terminalId}&machine_guid={machineGuid}&sessionKey={sessionKey}";
                Uri uri = new Uri(uriString);

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        byte[] imageData = File.ReadAllBytes(filename);

                        var content = new MultipartFormDataContent();
                        var imageContent = new ByteArrayContent(imageData);
                        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); ;
                        content.Add(imageContent, "auto_upload_file", "image.jpg");

                        var response = await httpClient.PostAsync(uri, content);

                        if ( response.IsSuccessStatusCode )
                        {
                            Console.WriteLine("Image uploaded successfully.");
                            var res = await response.Content.ReadAsStringAsync();
                            log.WriteLine(TraceLevel.Verbose, $"Send Image Result: {res}");
                        }
                        else
                        {
                            log.WriteLine( TraceLevel.Error, "Error uploading image. Status code: " + response.StatusCode);
                        }

                    }
                    catch (HttpRequestException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (ArgumentNullException e)
                    {
                        Logger.WriteException(e);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteException(e);
                    }
                }
            }
        }

        public class ApiResponse {
            public string result { get; set; }
            public string error { get; set; }
            public Dictionary<string, string> data { get; set; } = new Dictionary<string, string>();
        }

        // {"result":"ok","error":null,"data":{"items":[{"server_time":"2023-06-27T11:28:44Z","count_registered":"51"}]}}
        public class ApiAliveResponse
        {
            public string result { get; set; }
            public string error { get; set; }
            public Dictionary<string, object> data { get; set; } = new Dictionary<string, object>();
        }
    }
}
