using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Media;

namespace ManoAmigaApp
{
    [Activity(Label = "PERFIL")]
    public class ActivityPersonalizacion : Activity
    {
        TextView txtContra;
        ImageView Img;
        private byte[] bitmapData;
        ImageButton button;
        Toolbar toolbar;

        /// <summary>
        /// ACTIVA LOS PERMISOS DE LA APP
        /// </summary>
        readonly string[] permissionGroup =
       {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.personalizacion);
            // Create your application here

            //OBTENER LOS PERMISOS DE LA APP
            RequestPermissions(permissionGroup, 0);

            //PARA OBTENER LA IMAGEN DE PERFIL
            var c = Service.GetCustomerData(Service.Identification);

            //ASIGNACION DE IMAGEN DEL PERFIL, SI NO CONTIENE IMAGEN
            if (c.Foto == null)
            {
                Img = FindViewById<ImageView>(Resource.Id.imageView1);
                Img.SetImageResource(Resource.Drawable.perfil);
            }
            else
            {
                //SI TIENE IMAGEN
                Bitmap bitmap = BitmapFactory.DecodeByteArray(c.Foto, 0, c.Foto.Length);

                Img = FindViewById<ImageView>(Resource.Id.imageView1);
                Img.SetImageBitmap(bitmap);
            }
            //DATOS DEL CLIENTE    
            FindViewById<TextView>(Resource.Id.textView2).Text = Service.Identification;
            FindViewById<TextView>(Resource.Id.textView5).Text = Service.Fullname;
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            FindViewById<TextView>(Resource.Id.textView7).Text = Service.Email;

            txtContra = FindViewById<TextView>(Resource.Id.textView8);

            button = FindViewById<ImageButton>(Resource.Id.button1);
            button.Click += Button_Click;

            //CAMBIAR EL FONDO DEL ICONO DE IMEGEN
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            button.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            //CAMBIAR LA CONTRASEÑA DE LA CUENTA DEL CLIENTE
            txtContra.Click += TxtContra_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this)
               .SetTitle("Seleccione")
               .SetMessage("Seleccione una opción para subir una imagen a su perfil")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo)
               .SetNeutralButton("Cerrar", (IDialogInterfaceOnClickListener)null)
               .SetPositiveButton("Cámara", delegate
               {
                   //ABRIR LA CAMARA PARA TOMAR FOTO
                   Intent intent = new Intent(MediaStore.ActionImageCapture);
                   StartActivityForResult(intent, 0);
               })
               .SetNegativeButton("Galeria", delegate
               {
                   //SUBIR FOTO DESDE LA GALERIA
                   UploadPhoto();

               }).Create();

            builder.Show();
        }      

        /// <summary>
        /// CAMBIAR CONTRASEÑA DEL CLIENTE DE SU SESION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtContra_Click(object sender, EventArgs e)
        {
            //SE INFLA LA VISTA CON EL LAYOUT DETALLE DE LIBRO
            View view = LayoutInflater.Inflate(Resource.Layout.passwordChange, null);

            //SE CREA EL POP UP DEL LIBRO
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this)
               .SetTitle("Cambio de contraseña")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo)
               .Create();
            //ASIGNACION DE TEXTOS :v
            EditText vieja = view.FindViewById<EditText>(Resource.Id.editText1);
            EditText nueva = view.FindViewById<EditText>(Resource.Id.editText2);
            EditText conf = view.FindViewById<EditText>(Resource.Id.editText3);
            view.FindViewById<Button>(Resource.Id.button1).Click += (send, arg) => {

                //VALIDAR CAMPOS NO QUEDEN VACIOS
                if (vieja.Text != "" && nueva.Text != "" && conf.Text != "")
                {
                    if (nueva.Text.Length >= 6)
                    {
                        //VALIDAR CONTRASEÑAS IGUALES
                        if (nueva.Text == conf.Text)
                        {
                            //CONSUMIR EL SERVICIO
                            var change = Service.PasswordChange(Service.Email, vieja.Text, nueva.Text);
                            //SI SE CAMBIO EXITOSAMENTE
                            if (change.band)
                            {
                                Toast.MakeText(this, change.message, ToastLength.Long).Show();
                                //CERRAR EL DIALOG
                                builder.Dismiss();
                            }
                            else
                                Toast.MakeText(this, change.message, ToastLength.Long).Show();
                        }
                        else
                            Toast.MakeText(this, "Las contraseña no coinciden", ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "La longitud de la contraseña debe ser mayor a 6 dígitos", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(this, "Complete los campos necesarios", ToastLength.Long).Show();
            };

            builder.SetView(view);
            builder.Show();
        }

        /// <summary>
        /// CAMBIAR LA IMAGEN DE PERFIL DESDE LA CAMARA
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
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

                //ALMACENAR LA FOTO DEL CLIENTE
                var foto = Service.ImageChange(Service.CustomerId, bitmapData);
                if (foto.band)
                    Toast.MakeText(this, foto.message, ToastLength.Long).Show();
                else
                    Toast.MakeText(this, foto.message, ToastLength.Long).Show();
            }            
        }

        /// <summary>
        /// CAMBIAR LA IMAGEN DE PERFIL CON GALERIA
        /// </summary>
        async void UploadPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Upload not supported on this device", ToastLength.Short).Show();
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40

            });

            if (file != null)
            {
                // Convert file to byre array, to bitmap and set it to our ImageView
                byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                Img.SetImageBitmap(bitmap);

                //convertimos a Byte para luego almacenarlo en el servidor
                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }

                //ALMACENAR LA FOTO DEL CLIENTE
                var foto = Service.ImageChange(Service.CustomerId, bitmapData);
                if (foto.band)
                    Toast.MakeText(this, foto.message, ToastLength.Long).Show();
                else
                    Toast.MakeText(this, foto.message, ToastLength.Long).Show();
            }            
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //Back button pressed -> toggle event
            if (item.ItemId == Android.Resource.Id.Home)
                this.OnBackPressed();

            return base.OnOptionsItemSelected(item);
        }
    }
}