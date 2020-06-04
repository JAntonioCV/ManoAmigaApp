using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ManoAmigaApp
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText edtEmail, edtNames, edtLastName, edtId, edtPass, edtPassConf;
        Button btnRegister;
        ISharedPreferences shared;
        ISharedPreferencesEditor editor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registro);
            // Create your application here

            shared = PreferenceManager.GetDefaultSharedPreferences(this);
            editor = shared.Edit();

            edtEmail = FindViewById<EditText>(Resource.Id.editText2);
            edtId = FindViewById<EditText>(Resource.Id.editText1);
            edtNames = FindViewById<EditText>(Resource.Id.editText6);
            edtLastName = FindViewById<EditText>(Resource.Id.editText5);
            edtPass = FindViewById<EditText>(Resource.Id.editText4);
            edtPassConf = FindViewById<EditText>(Resource.Id.editText3);
            btnRegister = FindViewById<Button>(Resource.Id.button1);


            btnRegister.Click += BtnRegister_Click;




        }


        /// <summary>
        /// REGISTRO DEL CLIENTE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            //NO DEJAR PASAR DATOS IMPORTANTES VACIOS
            if (edtNames.Text != "" && edtLastName.Text != "" && edtId.Text != "" && edtEmail.Text != "" && edtPass.Text != "" && edtPassConf.Text != "")
            {
                //LA CONTRASEÑA DEBE SER MAYOR A 6
                if (edtPass.Text.Length > 6)
                {
                    //SIMULACION DE CONFIRMACION DE CONTRASEÑA
                    if (edtPass.Text == edtPassConf.Text)
                    {
                        //OBTENER LOS DATOS DEL SERVICIO DE REGISTRO
                        var register = Service.Register(edtId.Text.Trim(), edtNames.Text.Trim(), edtLastName.Text.Trim(), edtEmail.Text.Trim(), edtPassConf.Text.Trim(), "User");

                        //SI EL REGISTRO FUE EXITOSO
                        if (register.Register)
                        {
                            Toast.MakeText(this, "Cuenta creada exitosamente", ToastLength.Long).Show();

                            //OBTENER LOS DATOS DEL CLIENTE REGISTRADO
                            if (Service.CustomerData(edtEmail.Text.Trim()) != null)
                            {
                                //ALMACENAR CREDENCIALES
                                editor.PutString("contrase", edtPass.Text);
                                editor.PutString("usua", edtEmail.Text);
                                editor.Commit();

                                //GUARDAR DATOS DE MI CLIENTE
                                var datos = Service.CustomerData(edtEmail.Text.Trim());
                                //SE GUARDAN EN LA CLASE DE SERVICIO
                                Service.CustomerId = datos.Id;
                                Service.Identification = datos.Codigo;
                                Service.Fullname = datos.Nombres + " " + datos.Apellidos;
                                Service.Email = datos.Email;

                                //SE INICIA LA ACTIVIDAD PRINCIPAL
                                Intent i = new Intent(this, typeof(ActivityOperationsCustomer));
                                this.Finish();
                                StartActivity(i);                                
                                Toast.MakeText(this, "Registro exitoso", ToastLength.Long).Show();
                            }
                        }
                        else
                            Toast.MakeText(this, register.Mensaje, ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "Contraseñas no coinciden", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(this, "La contraseña debe ser mayor a 6 dígitos", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(this, "Llene los campos especificados", ToastLength.Long).Show();
        }
    }
}