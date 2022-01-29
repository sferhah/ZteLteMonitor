using Android.App;
using Android.Content.PM;
using Android.OS;
using ZteLteMonitor.Core;

namespace ZteLteMonitor.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SplashScreen);
        }

        protected override async void OnResume()
        {
            base.OnResume();

            using (LogDbContext ctx = new LogDbContext())
            {
                await ctx.Init(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), 24);
            }

            StartActivity(typeof(MainActivity));
        }

    }
}