//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.Gms.Common;
//using Android.OS;
//using Android.Runtime;
//using Android.Util;
//using Android.Views;
//using Android.Widget;
//using Firebase.Iid;
//using Firebase.Messaging;

//namespace ManoAmigaApp
//{
//    [Activity(Label = "ActivityNotifications")]
//    public class ActivityNotifications : Activity
//    {
//        TextView txtservice;
//        Button btngettoken;
//        Button btnregister;

//        protected override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            SetContentView(Resource.Layout.notifications);

//            txtservice = FindViewById<TextView>(Resource.Id.textView1);
//            btngettoken = FindViewById<Button>(Resource.Id.button1);
//            btnregister = FindViewById<Button>(Resource.Id.button2);

//            IsPlayServicesAvailable();

//            btngettoken.Click += delegate
//            {
//                Log.Debug("Token", "Instance ID Token:" + FirebaseInstanceId.Instance.Token);
//            };

//            btnregister.Click += delegate
//            {
//                FirebaseMessaging.Instance.SubscribeToTopic("Your_Topic_Name");
//            };

//        }

//        private bool IsPlayServicesAvailable()
//        {
//            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
//            if (resultCode != ConnectionResult.Success)
//            {
//                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
//                    txtservice.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
//                else
//                {
//                    txtservice.Text = "This Device Is not supported";
//                }
//                return false;
//            }
//            else
//            {
//                txtservice.Text = "Google Play Services is Available";
//                return true;
//            }

//        }
//    }
//}