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
    [Activity(Label = "CopiesAddActivity")]
    public class CopiesAddActivity : Activity
    {
        EditText edtISBN, edtTitle, edtNum;
        ImageButton btnScan, btnCheck;
        Button btnSave;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddCopy);
            // Create your application here

            edtISBN = FindViewById<EditText>(Resource.Id.editText1);
            edtTitle = FindViewById<EditText>(Resource.Id.editText2);
            edtNum = FindViewById<EditText>(Resource.Id.editText3);

            edtTitle.Enabled = false;

            btnScan = FindViewById<ImageButton>(Resource.Id.button1);
            btnCheck = FindViewById<ImageButton>(Resource.Id.button2);
            btnSave = FindViewById<Button>(Resource.Id.button3);

            btnSave.Click += BtnSave_Click;
            btnCheck.Click += BtnCheck_Click;
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            if (edtISBN.Text != "")
            {

            }
            else
                Toast.MakeText(this, "Ingrese un número de copia", ToastLength.Long).Show();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (edtNum.Text != "")
            {

            }
            else
                Toast.MakeText(this, "Ingrese un número de copia", ToastLength.Long).Show();
        }
    }
}