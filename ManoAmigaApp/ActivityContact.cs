using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ManoAmigaApp
{
    [Activity(Label = "")]
    public class ActivityContact : Activity
    {
        Toolbar toolbar;
        TextView txtResumen, txtNumber, txtEmail, txtResumen2, txtUbication;
        ImageButton btnNumber, btnEmail, btnMap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ContactActivity);
            //Create your application here

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            txtResumen = FindViewById<TextView>(Resource.Id.textView2);
            txtEmail = FindViewById<TextView>(Resource.Id.textView3);
            txtNumber = FindViewById<TextView>(Resource.Id.textView4);            
            btnEmail = FindViewById<ImageButton>(Resource.Id.imageButton1);
            btnNumber = FindViewById<ImageButton>(Resource.Id.imageButton2);
            txtResumen2 = FindViewById<TextView>(Resource.Id.textView6);
            txtUbication = FindViewById<TextView>(Resource.Id.textView5);
            btnMap = FindViewById<ImageButton>(Resource.Id.imageButton3);

            txtNumber.Text = "82636683";
            txtEmail.Text = "danycordero9@gmail.com";
            txtUbication.Text = "Balgüe, Isla de Ometepe\nNicaragua";
            btnEmail.Click += BtnEmail_Click1;
            btnNumber.Click += BtnNumber_Click;
            btnMap.Click += BtnMap_Click;

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnEmail.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnNumber.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnMap.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            txtResumen.Text = "The Mano Amiga Project (PMA) is a non-profit organization," +
                " founded in 2008. The project was created as an alternative to respond to a " +
                "variety of social and environmental problems of the Balgüe community, where PMA " +
                "is located. Currently, PMA is operated by volunteers, being supported by a local " +
                "committee, which coordinates the activities.";

            txtResumen2.Text = "Similar to the Project Mano Amiga, the Sanito Association was created " +
                "in order to develop and implement sustainable solutions for social, economic and environmental " +
                "problems for the physical, mental and social welfare of the people from Ometepe Island.";

            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        private void BtnMap_Click(object sender, EventArgs e)
        {
            var geoUri = Android.Net.Uri.Parse("geo:" + "11.5141-85.5818");
            var mapIntent = new Intent(Intent.ActionView, geoUri);
            StartActivity(mapIntent);
        }

        private void BtnEmail_Click1(object sender, EventArgs e)
        {
            //var intent = new Intent(Intent.ActionSend);
            //intent.SetData(Android.Net.Uri.Parse("test@gmail.com"));
            //intent.SetType("text/plain");
            //StartActivity(intent);

            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("message/rfc822");
            intent.PutExtra(Intent.ExtraEmail, new String[] { "contact@nic.sanito.org" });
            intent.SetPackage("com.google.android.gm");
            StartActivity(intent);
        }

        private void BtnNumber_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("tel:82636683");
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.shared, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.shared:
                    var i = new Intent(Intent.ActionSend);
                    i.PutExtra(Intent.ExtraText, "Conocé más de esta iniciativa\nhttps://sanito.org/");
                    i.SetType("text/plain");
                    StartActivity(i);
                    return true;
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    return true;
                default:
                    return false;
            }
        }
    }
}