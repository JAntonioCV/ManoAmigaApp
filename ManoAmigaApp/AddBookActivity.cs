using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    [Activity(Label = "")]
    public class AddBookActivity : Activity
    {
        EditText edtISBN, edtCod, edtTitle, edtAutor, edtPreview;
        ImageButton btnScan, btnCheck, Img;
        Button btnSave;
        MobileBarcodeScanner scanner;
        private byte[] bitmapData;
        Spinner sp_Materias;
        List<CategoryWS> listasp;
        CategoryWS cat;
        Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddCatalog);
            // Create your application here

            edtISBN = FindViewById<EditText>(Resource.Id.editText1);
            edtCod = FindViewById<EditText>(Resource.Id.editText2);
            edtTitle = FindViewById<EditText>(Resource.Id.editText3);
            edtAutor = FindViewById<EditText>(Resource.Id.editText4);
            edtPreview = FindViewById<EditText>(Resource.Id.editText5);
            sp_Materias = FindViewById<Spinner>(Resource.Id.spinner1);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            edtCod.Enabled = false;

            int var = int.Parse(Service.BookCode());
            edtCod.Text = "0"+var.ToString();


            edtTitle.Enabled = false;
            edtPreview.Enabled = false;
            edtAutor.Enabled = false;
            sp_Materias.Enabled = false;

            MobileBarcodeScanner.Initialize(Application);

            scanner = new MobileBarcodeScanner();

            btnScan = FindViewById<ImageButton>(Resource.Id.button1);
            btnCheck = FindViewById<ImageButton>(Resource.Id.button2);
            Img = FindViewById<ImageButton>(Resource.Id.imageButton1);
            btnSave = FindViewById<Button>(Resource.Id.button3);
            btnSave.Click += BtnSave_Click;

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnScan.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnCheck.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            btnCheck.Click += BtnCheck_Click;
            Img.Click += Img_Click;

            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

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

            //BUSCO LAS MATERIAS
            listasp = Service.CategoryList();
            //LLENAR EL ADAPTER CON LA LISTA DEL SERVICIO
            sp_Materias.Adapter = new AdapterSpMaterias(this, listasp);
            sp_Materias.ItemSelected += Sp_Materias_ItemSelected;
        }

        private void Sp_Materias_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var item = listasp[e.Position];
            cat = new CategoryWS();

            cat.Codigo = item.Codigo;
            cat.Descripcion = item.Descripcion;
            cat.Id = item.Id;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (edtISBN.Text != "" && edtTitle.Text != "" && edtCod.Text != "" && edtPreview.Text != "" && edtAutor.Text != "" && Img != null)
            {
                var Guardar = Service.AddBook(edtCod.Text, edtTitle.Text, edtISBN.Text, edtAutor.Text, bitmapData, DateTime.Now, edtPreview.Text, cat.Id);
                if (Guardar.band)
                {
                    Toast.MakeText(this, Guardar.message, ToastLength.Long).Show();

                    //SE CREA EL POP UP DEL LIBRO
                    Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this)
                       .SetTitle("Atención")
                       .SetMessage("¿Desea agregar copias de este libro?")
                       .SetIcon(Android.Resource.Drawable.IcDialogInfo)
                       .SetNeutralButton("No", (IDialogInterfaceOnClickListener)null)
                       .SetPositiveButton("Sí", delegate {
                           Intent i = new Intent(this, typeof(AddCopyActivity));
                           i.PutExtra("libroId", edtISBN.Text);
                           StartActivity(i);
                           this.Finish();
                       })
                       .Create();
                    builder.Show();
                }
                else
                    Toast.MakeText(this, Guardar.message, ToastLength.Long).Show();
            }
            else
                Toast.MakeText(this, "Llene los campos importantes", ToastLength.Long).Show();
        }

        private void Img_Click(object sender, EventArgs e)
        {
            //ABRIR LA CAMARA PARA TOMAR FOTO
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            if (edtISBN.Text != "")
            {
                BuscarLibro();
            }
            else
                Toast.MakeText(this, "Ingrese un código ISBN", ToastLength.Long).Show();
        }

        void HandleScanResult(ZXing.Result result)
        {
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {                
                if (result.Text.Length == 13)
                {
                    var libro = Service.SearchBook(result.Text);
                    if (libro != null)
                    {
                        edtISBN.Text = libro.ISBN;
                        edtTitle.Text = libro.Titulo;
                        edtCod.Text = libro.Codigo;
                        edtPreview.Text = libro.Descripcion;
                        edtAutor.Text = libro.Autor;
                        Bitmap bitmap = BitmapFactory.DecodeByteArray(libro.Portada, 0, libro.Portada.Length);

                        Img.SetImageBitmap(bitmap);

                        edtTitle.Enabled = false;
                        edtPreview.Enabled = false;
                        edtAutor.Enabled = false;
                        edtCod.Enabled = false;
                        Img.Enabled = false;
                        sp_Materias.Enabled = false;

                        sp_Materias.SetSelection(libro.MateriaId);

                        Toast.MakeText(this, "Ya existe un libro con ese ISBN", ToastLength.Long).Show();
                    }
                    else
                    {
                        edtISBN.Enabled = true;
                        edtISBN.Text = result.Text;
                        edtTitle.Enabled = true;
                        edtCod.Enabled = false;
                        edtPreview.Enabled = true;
                        edtAutor.Enabled = true;
                        sp_Materias.Enabled = true;
                    }
                }
                else
                    Toast.MakeText(this, "Error en los datos", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(this, "Vuelva a escanear", ToastLength.Long).Show();
        }

        public void BuscarLibro()
        {
            var libro = Service.SearchBook(edtISBN.Text);
            if (libro != null)
            {
                edtISBN.Text = libro.ISBN;
                edtTitle.Text = libro.Titulo;
                edtCod.Text = libro.Codigo;
                edtPreview.Text = libro.Descripcion;
                edtAutor.Text = libro.Autor;
                Bitmap bitmap = BitmapFactory.DecodeByteArray(libro.Portada, 0, libro.Portada.Length);

                Img.SetImageBitmap(bitmap);

                edtTitle.Enabled = false;
                edtPreview.Enabled = false;
                edtAutor.Enabled = false;
                edtCod.Enabled = false;
                sp_Materias.Enabled = false;

                sp_Materias.SetSelection(libro.MateriaId);

                Toast.MakeText(this, "Ya existe un libro con ese ISBN", ToastLength.Long).Show();
            }
            else
            {
                edtISBN.Enabled = true;
                edtTitle.Enabled = true;
                edtCod.Enabled = false;
                edtPreview.Enabled = true;
                edtAutor.Enabled = true;
                sp_Materias.Enabled = true;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (data != null)
            {
                base.OnActivityResult(requestCode, resultCode, data);
                Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                Img.SetImageBitmap(bitmap);

                //convertimos a Byte para luego almacenarlo en el servidor
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }
            }
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