using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    public class FragmentRental : Fragment
    {
        EditText edtCedula, edtNombres, edtCodAlq, edtFechaalq, edtFechaDevo, edtisbn, edttitulo, edtCode;
        Spinner spCopias;
        ImageButton btnScan, btnAgregar, btnFecha, btnlibros, btnguardar, btnAceptar;
        View rootView;
        MobileBarcodeScanner scanner;
        ListView listacopias;
        CopysBookWS lista;
        List<CopysBookWS> listasp;
        List<CopysBookWS> Detalleranta= new List<CopysBookWS>();
        int idCliente;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            //return base.OnCreateView(inflater, container, savedInstanceState);

            rootView = inflater.Inflate(Resource.Layout.fragmentrental, container, false);
            //INICIALIZAR LOS OBJETOS
            MobileBarcodeScanner.Initialize(Activity.Application);

            scanner = new MobileBarcodeScanner();

            edtCedula = rootView.FindViewById<EditText>(Resource.Id.editText1);
            edtNombres = rootView.FindViewById<EditText>(Resource.Id.editText2);
            edtCodAlq = rootView.FindViewById<EditText>(Resource.Id.editText3);
            edtFechaalq = rootView.FindViewById<EditText>(Resource.Id.editText4);
            edtFechaDevo = rootView.FindViewById<EditText>(Resource.Id.editText5);

            //DESACTIVAR LOS EDITEXT DE FECHA Y OTROS
            edtFechaalq.Enabled = false;
            edtFechaDevo.Enabled = false;
            edtCodAlq.Enabled = false;
            edtNombres.Enabled = false;

            int var = int.Parse(Service.RentalCode());
            edtCodAlq.Text = var.ToString();
            edtFechaalq.Text = DateTime.Now.ToShortDateString();

            //ASIGNACION DE OBJETOS Y ELIMINACION DE FONDOS A IMAGEBOTTON
            btnScan = rootView.FindViewById<ImageButton>(Resource.Id.button1);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnScan.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            btnAgregar = rootView.FindViewById<ImageButton>(Resource.Id.button2);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnAgregar.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            btnFecha = rootView.FindViewById<ImageButton>(Resource.Id.button3);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnFecha.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            btnlibros = rootView.FindViewById<ImageButton>(Resource.Id.button4);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnlibros.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            btnguardar = rootView.FindViewById<ImageButton>(Resource.Id.button5);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnguardar.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            btnAceptar = rootView.FindViewById<ImageButton>(Resource.Id.button6);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnAceptar.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            listacopias = rootView.FindViewById<ListView>(Resource.Id.listView1);

            btnlibros.Click += Btnlibros_Click;
            btnFecha.Click += BtnFecha_Click;
            btnAgregar.Click += BtnAgregar_Click;
            btnguardar.Click += Btnguardar_Click;
            btnAceptar.Click += BtnAceptar_Click;

            //ACTIVA EL ESCANEO
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

            return rootView;
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (edtCedula.Text != "")
            {
                //BUSCAR EL CLIENTE
                var cliente = Service.GetCustomerData(edtCedula.Text);
                if (cliente != null)
                {
                    idCliente = cliente.Id;
                    //LLENAR EL ENCABEZADO
                    edtCedula.Text = cliente.Codigo;
                    edtNombres.Text = cliente.Nombres + " " + cliente.Apellidos;
                }
                else
                    Toast.MakeText(rootView.Context, "No existe el cliente con ese número de cédula", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(rootView.Context, "Ingrese un número de cédula", ToastLength.Long).Show();
        }

        private void Btnguardar_Click(object sender, EventArgs e)
        {
            List<RentalDetailsWS> rentalDetailsWs = new List<RentalDetailsWS>();

            if (edtCodAlq.Text != "" && edtCedula.Text != "" && edtFechaalq.Text != "" && edtFechaDevo.Text != "" && listacopias.Count != 0)
            {
                foreach (var item in Detalleranta)
                {
                    var x = new RentalDetailsWS();
                    x.CopiaId = item.CopyId;
                    x.AlquilerId = 0;
                    x.Id = 0;
                    x.Portada = item.Portada;
                    x.NumeroCopia = 0;
                    x.TituloLibro = "";

                    rentalDetailsWs.Add(x);
                }

                var m = Service.AddRental(idCliente, edtCodAlq.Text, DateTime.Now.Date, Convert.ToDateTime(edtFechaDevo.Text), DateTime.Now.Date, rentalDetailsWs.ToList());
                if (m.band)
                    Toast.MakeText(rootView.Context, m.message, ToastLength.Long).Show();
                else
                    Toast.MakeText(rootView.Context, m.message, ToastLength.Long).Show();
            }
            else
                Toast.MakeText(rootView.Context, "Llene los campos importantes", ToastLength.Long).Show();
        }

        private void Btnlibros_Click(object sender, EventArgs e)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.copiasbybook, null);

            //SE CREA EL POP UP DEL LIBRO
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder((Activity)rootView.Context)
               .SetTitle("Agregar Libro")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo)
               .SetNegativeButton("Cerrar", (IDialogInterfaceOnClickListener)null)
               .SetPositiveButton("Agregar", delegate {

                   if (Detalleranta.Count() == 0)
                   {
                       Detalleranta.Add(lista);
                       Toast.MakeText(rootView.Context, "Se Agrego", ToastLength.Long).Show();
                   }
                   else
                   {
                       CopysBookWS Copia = Detalleranta.DefaultIfEmpty(null).FirstOrDefault(x => x.CopyId == lista.CopyId);

                       if (Copia == null)
                       {
                           Detalleranta.Add(lista);
                           Toast.MakeText(rootView.Context, "Se Agrego", ToastLength.Long).Show();
                       }
                       else
                       {
                           Toast.MakeText(rootView.Context, "No se puede agregar por que ya existe", ToastLength.Long).Show();
                       }
                   }

                   listacopias = rootView.FindViewById<ListView>(Resource.Id.listView1);
                   listacopias.Adapter = new AdapterDetailRental((Activity)rootView.Context, Detalleranta);
               }).Create();
            //ASIGNACION DE TEXTOS
            edtisbn= view.FindViewById<EditText>(Resource.Id.editText1);
            edttitulo= view.FindViewById<EditText>(Resource.Id.editText2);
            edttitulo.Enabled = false;
            spCopias = view.FindViewById<Spinner>(Resource.Id.spinner1);
            ImageButton btnISBN = view.FindViewById<ImageButton>(Resource.Id.button2);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnISBN.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            ImageButton btnScanner = view.FindViewById<ImageButton>(Resource.Id.button1);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnScanner.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            btnISBN.Click += delegate
            {
                if (edtisbn.Text != null)
                {
                    var libro = Service.SearchBook(edtisbn.Text);
                    if (libro != null)
                    {
                        //LLENO LOS EDITEXT
                        edtisbn.Text = libro.ISBN;
                        edttitulo.Text = libro.Titulo;

                        //BUSCO LAS COPIAS DISPONIBLES DEL LIBRO ENCONTRADO
                        listasp = Service.CopiesByBook(libro.ISBN);
                        //LLENAR EL ADAPTER CON LA LISTA DEL SERVICIO
                        spCopias.Adapter = new AdapterSpCopias((Activity)rootView.Context, listasp);
                        //spCopias.SetSelection(0,true);
                        spCopias.ItemSelected += SpCopias_ItemSelected;
                    }
                    else
                        Toast.MakeText(rootView.Context, "Dígite el código ISBN", ToastLength.Long).Show();
                }
            };
            btnScanner.Click += async delegate
            {
                //SCANEAR EL ISBN DEL LIBRO
                //Tell our scanner to use the default overlay
                scanner.UseCustomOverlay = false;

                //PERSONALIZAR LOS MENSAJES QUE SE MOSTRARAN EN LA CAMARA DEL SCANNER
                scanner.TopText = "Por favor, no mueva el dispositivo móvil\nMantengalo al menos 10cm de distancia";
                scanner.BottomText = "Espere mientras el scanner lee el código de barra";

                //COMIENZO DEL SCANEO Y ALMACENO SU VALOR
                var result = await scanner.Scan();

                HandleScanResult(result);
            };

            //view.FindViewById<ImageButton>(Resource.Id.button2).Click += FragmentRental_Click;

            listacopias = view.FindViewById<ListView>(Resource.Id.listView1);

            //spCopias = view.FindViewById<Spinner>(Resource.Id.spinner1);

            builder.SetView(view);
            builder.Show();
        }

        private void SpCopias_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            lista = new CopysBookWS();

            lista.ISBN = listasp[e.Position].ISBN;
            lista.LibroId = listasp[e.Position].LibroId;
            lista.Portada = listasp[e.Position].Portada;
            lista.CopyId = listasp[e.Position].CopyId;
            lista.NumeroCopia = listasp[e.Position].NumeroCopia;
            lista.Titulo = listasp[e.Position].Titulo;

            //if (Detalleranta.Count() == 0)
            //{
            //    Detalleranta.Add(lista);
            //    Toast.MakeText(rootView.Context, "Se Agrego", ToastLength.Long).Show();
            //}
            //else
            //{
            //    CopysBookWS Copia = Detalleranta.DefaultIfEmpty(null).FirstOrDefault(x => x.CopyId == lista.CopyId);

            //    if (Copia == null)
            //    {
            //        Detalleranta.Add(lista);
            //        Toast.MakeText(rootView.Context, "Se Agrego", ToastLength.Long).Show();
            //    }
            //    else
            //    {
            //        Toast.MakeText(rootView.Context, "No se puede agregar por que ya existe", ToastLength.Long).Show();
            //    }
            //}

        }

        private void FragmentRental_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ACCION QUE AGREGA UN CLIENTE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            //SE INFLA LA VISTA CON EL LAYOUT DETALLE DE LIBRO
            View view = LayoutInflater.Inflate(Resource.Layout.CustomerAdd, null);

            //SE CREA EL POP UP DEL LIBRO
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder((Activity)rootView.Context)
               .SetTitle("Agregar cliente")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo).Create();
            //ASIGNACION DE TEXTOS :v
            edtCode = view.FindViewById<EditText>(Resource.Id.editText1);
            EditText edtName = view.FindViewById<EditText>(Resource.Id.editText2);
            EditText edtLastName = view.FindViewById<EditText>(Resource.Id.editText3);
            ImageButton b = view.FindViewById<ImageButton>(Resource.Id.imageButton1);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            b.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            b.Click += async delegate {

                //Tell our scanner to use the default overlay
                scanner.UseCustomOverlay = false;

                //PERSONALIZAR LOS MENSAJES QUE SE MOSTRARAN EN LA CAMARA DEL SCANNER
                scanner.TopText = "Por favor, no mueva el dispositivo móvil\nMantengalo al menos 10cm de distancia";
                scanner.BottomText = "Espere mientras el scanner lee el código de barra";

                //COMIENZO DEL SCANEO
                var result = await scanner.Scan();

                HandleScanResult(result);
            };
            view.FindViewById<Button>(Resource.Id.button1).Click += delegate {
                if (edtCode.Text != "" && edtName.Text != "" && edtLastName.Text != "")
                {
                    var save = Service.AddCustomer(edtName.Text, edtLastName.Text, edtCode.Text);
                    if (save.band)
                    {
                        edtCedula.Text = edtCode.Text;
                        edtNombres.Text = edtName.Text + " " + edtLastName.Text;

                        //BUSCAR EL CLIENTE PARA SU ID
                        idCliente = Service.GetCustomerData(edtCode.Text).Id;
                        Toast.MakeText(rootView.Context, save.message, ToastLength.Long).Show();
                        //CERRAR EL DIALOG
                        builder.Dismiss();
                    }
                    else
                        Toast.MakeText(rootView.Context, save.message, ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(rootView.Context, "Llene los campos importantes", ToastLength.Long).Show();
            };

            builder.SetView(view);
            builder.Show();
        }

        /// <summary>
        /// EVENTO PARA MANDAR A LLAMAR EL DATEPICKER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFecha_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                edtFechaDevo.Text = time.ToLongDateString();
                //time = time.AddDays(7);
                edtFechaDevo.Text = time.ToShortDateString();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        /// <summary>
        /// EVENTO PARA SCANEAR EL CODIGO Y SUS ACCIONES
        /// </summary>
        /// <param name="result"></param>
        void HandleScanResult(ZXing.Result result)
        {
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                //LONGITUD DE CEDULA CLIENTE
                if (result.Text.Length == 14)
                {
                    //BUSCAR EL CLIENTE
                    var cliente = Service.GetCustomerData(result.Text);
                    if (cliente != null)
                    {
                        idCliente = cliente.Id;
                        //LLENAR EL ENCABEZADO
                        edtCedula.Text = cliente.Codigo;
                        edtNombres.Text = cliente.Nombres + " " + cliente.Apellidos;
                    }
                    else
                    if (cliente == null)
                        edtCode.Text = result.Text;
                }
                else//LONGITUD DE ISBN LIBRO
                if (result.Text.Length == 13)
                {
                    var libro = Service.SearchBook(result.Text);
                    if (libro != null)
                    {
                        //LLENO LOS EDITEXT
                        edtisbn.Text = libro.ISBN;
                        edttitulo.Text = libro.Titulo;

                        //BUSCO LAS COPIAS DISPONIBLES DEL LIBRO ENCONTRADO
                        listasp = Service.CopiesByBook(libro.ISBN);
                        //LLENAR EL ADAPTER CON LA LISTA DEL SERVICIO
                        spCopias.Adapter = new AdapterSpCopias((Activity)rootView.Context, listasp);
                        //spCopias.SetSelection(0,true);
                        spCopias.ItemSelected += SpCopias_ItemSelected;
                    }
                    else
                        Toast.MakeText(rootView.Context, "No se encontro el libro", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(rootView.Context, "Error en los datos", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(rootView.Context, "Vuelva a escanear", ToastLength.Long).Show();
        }
    }
}