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
using ZXing.Mobile;
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    [Activity(Label = "")]
    public class AddCopyActivity : Activity
    {
        Toolbar toolbar;
        EditText edtTitulo, edtNumber, edtISBN;
        ImageButton btnScan, btnCheck;
        Button btnSave;
        MobileBarcodeScanner scanner;
        BookWS libro;
        string isbn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddCopy);
            // Create your application here

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            btnScan = FindViewById<ImageButton>(Resource.Id.button1);
            btnCheck = FindViewById<ImageButton>(Resource.Id.button2);
            btnSave = FindViewById<Button>(Resource.Id.button3);
            edtISBN = FindViewById<EditText>(Resource.Id.editText1);
            edtNumber = FindViewById<EditText>(Resource.Id.editText3);
            edtTitulo = FindViewById<EditText>(Resource.Id.editText2);
            btnSave.Click += BtnSave_Click;

            isbn = Intent.GetStringExtra("libroId");


            if (isbn != null)
            {
                edtISBN.Text = isbn;
                libro = Service.SearchBook(edtISBN.Text);
                if (libro != null)
                {
                    edtISBN.Text = libro.ISBN;
                    edtTitulo.Text = libro.Titulo;
                }
                else
                    Toast.MakeText(this, "El libro no existe", ToastLength.Long).Show();


            }


#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnScan.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnCheck.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            btnCheck.Click += BtnCheck_Click;

            MobileBarcodeScanner.Initialize(Application);
            scanner = new MobileBarcodeScanner();

            btnScan.Click += async delegate {

                //Tell our scanner to use the default overlay
                scanner.UseCustomOverlay = false;

                //PERSONALIZAR LOS MENSAJES QUE SE MOSTRARAN EN LA CAMARA DEL SCANNER
                scanner.TopText = "Por favor, no mueva el dispositivo móvil\nMantengalo al menos 10cm de distancia";
                scanner.BottomText = "Espere mientras el scanner lee el código de barra";

                //COMIENZO DEL SCANEO
                var result = await scanner.Scan();

                HandleScanResult(result);
            };
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            if (edtISBN.Text != "")
            {
                libro = Service.SearchBook(edtISBN.Text);
                if (libro != null)
                {
                    edtISBN.Text = libro.ISBN;
                    edtTitulo.Text = libro.Titulo;
                }
                else
                    Toast.MakeText(this, "El libro no existe", ToastLength.Long).Show();
            }
            Toast.MakeText(this, "Ingrese un código ISBN", ToastLength.Long).Show();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (edtNumber.Text != null)
            {
                var save = Service.AddCopy(int.Parse(edtNumber.Text), libro.Id);
                if (save.band)
                {
                    Toast.MakeText(this, save.message, ToastLength.Long).Show();
                    edtISBN.Text = "";
                    edtNumber.Text = "";
                    edtTitulo.Text = "";
                }
                else
                    Toast.MakeText(this, save.message, ToastLength.Long).Show();
            }
            else
                Toast.MakeText(this, "Ingrese el número de copia", ToastLength.Long).Show();
        }

        void HandleScanResult(ZXing.Result result)
        {
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                if (result.Text.Length == 13)
                {
                    libro = Service.SearchBook(result.Text);
                    if (libro != null)
                    {
                        edtISBN.Text = libro.ISBN;
                        edtTitulo.Text = libro.Titulo;                       
                    }
                    else
                        Toast.MakeText(this, "El libro no existe", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(this, "Error en los datos", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(this, "Vuelva a escanear", ToastLength.Long).Show();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    return true;
                default:
                    return false;
            }
        }
    }
}