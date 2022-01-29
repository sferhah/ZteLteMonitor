using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace ZteLteMonitor.Core
{
    public class NetworkUtils
    {
        public static string? GetDefaultGateway() => NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .SelectMany(n => n.GetIPProperties().GatewayAddresses)
            .Select(g => g.Address)
            .FirstOrDefault()?
            .ToString();

        public static async Task<bool> IsConnectedToInternet(int timeoutMs = 10000, string? url = null)
        {
            try
            {
                url ??= "http://www.gstatic.com/generate_204";

                var httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(timeoutMs)
                };
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);

                return response.StatusCode == HttpStatusCode.NoContent;
            }
            catch
            {
                return false;
            }
        }
    }
}
