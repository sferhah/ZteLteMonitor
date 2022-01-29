using Android.Content;

namespace ZteLteMonitor.Droid
{
    public class SharedPref
    {
        private static ISharedPreferences mSharedPref;

        public static int SpinnerSelectedItemPosition
        {
            get => Read(nameof(SpinnerSelectedItemPosition), 0);
            set => Write(nameof(SpinnerSelectedItemPosition), value);
        }

        public static string IpAddress
        {
            get => Read(nameof(IpAddress), null);
            set => Write(nameof(IpAddress), value);
        }

        public static string Password
        {
            get => Read(nameof(Password), null);
            set => Write(nameof(Password), value);
        }

        private SharedPref() { }

        public static void Init(Context context)
        {
            if (mSharedPref == null)
            {
                mSharedPref = context.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            }
        }

        private static string Read(string key, string defValue) => mSharedPref.GetString(key, defValue);
        private static void Write(string key, string value) => mSharedPref.Edit().PutString(key, value).Commit();

        private static int Read(string key, int defValue) => mSharedPref.GetInt(key, defValue);
        private static void Write(string key, int value) => mSharedPref.Edit().PutInt(key, value).Commit();

    }
}