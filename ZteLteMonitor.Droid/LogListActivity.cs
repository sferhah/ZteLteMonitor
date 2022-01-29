using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Microsoft.EntityFrameworkCore;
using ZteLteMonitor.Core;

namespace ZteLteMonitor.Droid
{

    [Activity(Label = "@string/app_name", MainLauncher = false)]
    [Obsolete]
    public partial class LogListActivity : ListActivity
    {
        private ArrayAdapter<string> adapter;
        Button refreshButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ListAdapter = adapter = new ArrayAdapter<string>(this, Resource.Layout.LogListItem);
            ListView.TextFilterEnabled = true;
            ListView.ItemClick += (sender, args) => Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();

            SetContentView(Resource.Layout.LogList);

            refreshButton = FindViewById<Button>(Resource.Id.RefreshButton);
            refreshButton.Click += RefreshButton_Click;

            RefreshButton_Click(null, null);
        }


        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            adapter.Clear();

            using LogDbContext context = new LogDbContext();

            var items = await context.Logs.Take(10000).ToListAsync();

            foreach (var item in items)
            {
                adapter.Insert(item.ToString(), 0);
            }

            adapter.SetNotifyOnChange(true);
        }
    }
}