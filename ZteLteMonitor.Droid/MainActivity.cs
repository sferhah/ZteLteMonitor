using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ZteLteMonitor.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = false)]
    public partial class MainActivity : Activity
    {
        MainBroadcastReceiver receiver;
        EditText ipEditText;
        EditText passwordEditText;
        TextView resultsTextView;
        Button launchJobButton;
        Spinner spinner;

        static bool firstAppear = true;
        const string default_ip = "192.168.0.1";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            receiver = new MainBroadcastReceiver(this);
            SharedPref.Init(ApplicationContext);

            SetContentView(Resource.Layout.Main);

            ipEditText = FindViewById<EditText>(Resource.Id.main_activity_edit_text_ipaddress);

            passwordEditText = FindViewById<EditText>(Resource.Id.main_activity_edit_text_password);
            passwordEditText.Text = SharedPref.Password ?? passwordEditText.Text;

            resultsTextView = FindViewById<TextView>(Resource.Id.results_textview);

            spinner = FindViewById<Spinner>(Resource.Id.main_activity_spinner);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, new string[] { "DefaultGateway", "DefaultIp", "Custom" });

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.SetSelection(SharedPref.SpinnerSelectedItemPosition);
            spinner.ItemSelected += Spinner_ItemSelected;


            launchJobButton = FindViewById<Button>(Resource.Id.launch_job_button);
            launchJobButton.Click += ScheduleMainJob;
            RefreshUi();

            FindViewById<Button>(Resource.Id.gotologs_button).Click += (s, a) => StartActivity(new Intent(this, typeof(LogListActivity)));

            if (firstAppear)
            {
                firstAppear = false;

                if (!this.IsJobServiceOn())
                {
                    resultsTextView.Text = "First start... job is not running";
                    //tiny patch
                    if (spinner.SelectedItemPosition == 2)
                    {
                        ipEditText.Text = SharedPref.IpAddress;
                    }

                    ScheduleMainJob(null, null);
                }
                else
                {
                    resultsTextView.Text = "First start... job is already running";
                }
            }
            else
            {
                resultsTextView.Text = "Restarted..." + (this.IsJobServiceOn() ? "job is already running." : "job is not running.");
            }
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            SharedPref.SpinnerSelectedItemPosition = e.Position;

            switch (e.Position)
            {
                case 0:
                    ipEditText.Text = NetworkUtils.GetDefaultGateway(this) ?? default_ip;
                    break;
                case 1:
                    ipEditText.Text = default_ip;
                    break;
                case 2:
                    ipEditText.Text = SharedPref.IpAddress ?? NetworkUtils.GetDefaultGateway(this) ?? default_ip;
                    break;
            }
        }     

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(receiver, new IntentFilter(BundleExtenstions.JobActionKey));
        }

        protected override void OnPause()
        {
            BaseContext.UnregisterReceiver(receiver);
            base.OnPause();
        }


        void RefreshUi()
        {
            if (this.IsJobServiceOn())
            {
                launchJobButton.Text = "Stop";
                spinner.Enabled = false;
                ipEditText.Enabled = false;
                passwordEditText.Enabled = false;
            }
            else
            {
                launchJobButton.Text = "Start";
                spinner.Enabled = true;
                ipEditText.Enabled = true;
                passwordEditText.Enabled = true;
            }

        }

        void ScheduleMainJob(object sender, EventArgs eventArgs)
        {
            if (this.IsJobServiceOn())
            {
                this.StopJobService();
                resultsTextView.Text = "Job stopped!";
            }
            else
            {
                if (spinner.SelectedItemPosition == 2)
                {
                    SharedPref.IpAddress = ipEditText.Text;
                }

                SharedPref.Password = passwordEditText.Text;

                resultsTextView.Text = "Requesting job...";

                int result = this.CreateJob(new MainJobOptions
                {
                    IpAddress = ipEditText.Text,
                    Password = passwordEditText.Text,
                    UseDefaultGateway = spinner.SelectedItemPosition == 0,
                });

                resultsTextView.Text = result == JobScheduler.ResultSuccess ? "Job is underway." : "problem.";
            }

            RefreshUi();
        }



        [BroadcastReceiver(Enabled = true, Exported = false)]
        protected internal class MainBroadcastReceiver : BroadcastReceiver
        {
            MainActivity activity;
            public MainBroadcastReceiver() { } // required

            public MainBroadcastReceiver(MainActivity activity) => this.activity = activity;

            public override void OnReceive(Context context, Intent intent)
            {
                if (activity != null)
                {
                    activity.resultsTextView.Text = intent.Extras.ToAppLog().ToString();
                }
            }
        }

    }
}