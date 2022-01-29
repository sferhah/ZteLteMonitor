//using Android.App;
//using Android.Content;
//using Android.OS;

//namespace ZteLteMonitor.Droid
//{    
//    public class ReceiverActivity : Activity
//    {
//        GenericBroadcastReceiver receiver;
//        protected override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            receiver = new GenericBroadcastReceiver(this);            
//        }

//        protected override void OnResume()
//        {
//            base.OnResume();            
//            RegisterReceiver(receiver, new IntentFilter(BundleExtenstions.JobActionKey));
//        }

//        protected override void OnPause()
//        {
//            BaseContext.UnregisterReceiver(receiver);
//            base.OnPause();
//        }

//        virtual public void OnReceiveGenericBroadcast(Bundle extras)
//        {
         
//        }


//        [BroadcastReceiver(Enabled = true, Exported = false)]
//        protected internal class GenericBroadcastReceiver : BroadcastReceiver
//        {
//            ReceiverActivity activity;            
//            public GenericBroadcastReceiver() { } // required
//            public GenericBroadcastReceiver(ReceiverActivity activity) => this.activity = activity;

//            public override void OnReceive(Context context, Intent intent)
//            {
//                if (activity != null)
//                {
//                    activity.OnReceiveGenericBroadcast(intent.Extras);
//                }                
//            }
//        }

//    }
//}