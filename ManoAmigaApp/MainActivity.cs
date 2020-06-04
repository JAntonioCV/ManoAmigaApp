using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Preferences;

namespace ManoAmigaApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView txtRegister;
        EditText edtUser, edtPass;
        Button btnLogin;
        ISharedPreferences shared;
        ISharedPreferencesEditor editor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            txtRegister = FindViewById<TextView>(Resource.Id.textView1);
            edtUser = FindViewById<EditText>(Resource.Id.editText2);
            edtPass = FindViewById<EditText>(Resource.Id.editText1);
            btnLogin = FindViewById<Button>(Resource.Id.button1);

            btnLogin.Click += BtnLogin_Click;
            txtRegister.Click += TxtRegister_Click;

            shared = PreferenceManager.GetDefaultSharedPreferences(this);
            editor = shared.Edit();

            if (Service.Login(shared.GetString("usua", "").ToString(), shared.GetString("contrase", "").ToString()).IsLogged)
            {
                if (dataCustomer(shared.GetString("usua", "").ToString()))
                {
                    Intent intent = new Intent(this, typeof(ActivityOperationsCustomer));
                    this.Finish();
                    StartActivity(intent);
                }
                else
                {
                    Intent intent = new Intent(this, typeof(ActivityOperationsAdmin));
                    this.Finish();
                    StartActivity(intent);
                }
            }

            ////ALMACENAR LAS CREDENCIALES DEL USUARIO
            string contra = shared.GetString("contrase", "");
            string usua = shared.GetString("usua", "");
            edtPass.Text = contra;
            edtUser.Text = usua;
        }

        private void TxtRegister_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(RegisterActivity));            
            StartActivity(i);
        }

        private void BtnLogin_Click(object sender, System.EventArgs e)
        {        
            //ALMACENA EL OBJETO DEL SERVICIO
            var usuario = Service.Login(edtUser.Text.Trim(), edtPass.Text.Trim());

            //SI NO ESTA LOGEADO
            if (!usuario.IsLogged)
            {
                //ENVIA UN MENSAJE CON SU RESPECTIVO ERROR
                Toast.MakeText(this, usuario.Mensaje, ToastLength.Long).Show();
            }
            else
            {
                editor.PutString("contrase", edtPass.Text);
                editor.PutString("usua", edtUser.Text);
                editor.Commit();

                //SI ESTA LOGEADO Y SU ROL ES USUARIO
                if (usuario.Rol == "User")
                {
                    if (dataCustomer(edtUser.Text.Trim()))
                    {
                        //ACTIVITY DE USUARIO
                        Intent i = new Intent(this, typeof(ActivityOperationsCustomer));
                        StartActivity(i);

                        Toast.MakeText(this, usuario.Mensaje + " Tu rol es " + usuario.Rol, ToastLength.Long).Show();
                    }
                }
                else
                {
                    //ACTIVITY DE ADMIN
                    Intent i = new Intent(this, typeof(ActivityOperationsAdmin));
                    StartActivity(i);

                    Toast.MakeText(this, usuario.Mensaje + " Tu rol es " + usuario.Rol, ToastLength.Long).Show();
                }
            }
        }

        /// <summary>
        /// OBTIENE LA INFORMACION DEL CLIENTE
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool dataCustomer(string email)
        {
            //BUSCAR DATOS DEL CLIENTE
            var data = Service.CustomerData(email);
            if (data != null)
            {
                //ALMACENAR SUS DATOS
                Service.CustomerId = data.Id;
                Service.Fullname = data.Nombres + " " + data.Apellidos;
                Service.Identification = data.Codigo;
                Service.Email = data.Email;
                return true;
            }
            else
                return false;
        }
    }
}