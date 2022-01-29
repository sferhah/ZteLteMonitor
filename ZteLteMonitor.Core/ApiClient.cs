using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace ZteLteMonitor.Core
{
    public class ApiClient
    {
        public static async Task<ModemStatus> GetRouterStatus(string? ipAddress = null)
        {
            try
            {
                var url = $"http://{ipAddress ?? "192.168.0.1"}";

                var httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(1000)
                };

                httpClient.DefaultRequestHeaders.Add("Origin", url);
                httpClient.DefaultRequestHeaders.Add("Referer", $"{url}/index");

                var fields = string.Join(',', typeof(RouterGetResponse).GetProperties().Select(x => x.Name));
                var router_response = await httpClient.GetFromJsonAsync<RouterGetResponse>($"{url}/goform/goform_get_cmd_process?multi_data=1&isTest=false&cmd={fields}").ConfigureAwait(false);

                if (router_response!.modem_main_state == "modem_init_complete")
                {
                    if (router_response.ppp_status == "ppp_connected")
                    {
                        return ModemStatus.Connected;
                    }

                    return ModemStatus.Disconnected;
                }

                return ModemStatus.PoweredOn;
            }
            catch
            {
                return ModemStatus.Unavailable;
            }
        }


        public static async Task<RebootStatus> Reboot(string? ipAddress = null, string? password = null)
        {
            try
            {
                var url = $"http://{ipAddress ?? "192.168.0.1"}";
                password ??= "admin";

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Origin", url);
                httpClient.DefaultRequestHeaders.Add("Referer", $"{url}/index");

                var http_login_response = await httpClient.PostAsync($"{url}/goform/goform_set_cmd_process", new FormUrlEncodedContent(new Dictionary<string, string>
                                                                                                {
                                                                                                    { "isTest", "false"},
                                                                                                    { "goformId", "LOGIN"},
                                                                                                    { "password", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password))}
                                                                                                })).ConfigureAwait(false);


                var login_response = await http_login_response.Content.ReadFromJsonAsync<RouterPostResponse>().ConfigureAwait(false);

                if (login_response!.result != "0")
                {
                    return RebootStatus.InvalidPassword; // invalid password
                }

                
                var http_reboot_response = await httpClient.PostAsync($"{url}/goform/goform_set_cmd_process", new FormUrlEncodedContent(new Dictionary<string, string>
                                                                                            {
                                                                                                { "isTest", "false"},
                                                                                                { "goformId", "REBOOT_DEVICE"},
                                                                                            })).ConfigureAwait(false);

                var reboot_response = await http_reboot_response.Content.ReadFromJsonAsync<RouterPostResponse>().ConfigureAwait(false);

                if (login_response.result != "success")
                {
                    return RebootStatus.Success; // rebooted
                }

                return RebootStatus.Failed; // reboot failed
            }
            catch
            {
                return RebootStatus.Unavailable;
            }
        }


    }
}
