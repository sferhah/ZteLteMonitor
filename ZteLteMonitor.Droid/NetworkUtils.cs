using Android.Content;
using Android.Net;
using System.Linq;

namespace ZteLteMonitor.Droid
{
    public class NetworkUtils
    {
        public static string? GetDefaultGateway(Context context)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);

            var routeInfo = connectivityManager.GetLinkProperties(connectivityManager.ActiveNetwork)?.Routes?.FirstOrDefault(x => x.IsDefaultRoute);
            var ip = routeInfo?.Gateway?.HostAddress?.ToString();

            return ip;
        }
    }
}
