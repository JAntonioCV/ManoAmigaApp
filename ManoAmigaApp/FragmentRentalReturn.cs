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
    class FragmentRentalReturn : Fragment
    {
        EditText edtCodigoC, edtNombresA;
        ImageButton btnEscanear, btnBuscar;
        MobileBarcodeScanner scanner;
        View rootView;
        ListView lista;
        List<Stattics> pendiente;
        List<Stattics> alquiler;

        ListView detalle;



        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            rootView = inflater.Inflate(Resource.Layout.fragmentRentalReturn, container, false);

            //INICIALIZAR LOS OBJETOS
            MobileBarcodeScanner.Initialize(Activity.Application);

            scanner = new MobileBarcodeScanner();

            edtCodigoC = rootView.FindViewById<EditText>(Resource.Id.editText1);
            edtNombresA = rootView.FindViewById<EditText>(Resource.Id.editText2);
            edtNombresA.Enabled = false;
            btnEscanear = rootView.FindViewById<ImageButton>(Resource.Id.button1);
            btnBuscar= rootView.FindViewById<ImageButton>(Resource.Id.button2);
            lista = rootView.FindViewById<ListView>(Resource.Id.listView1);

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnEscanear.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            btnBuscar.SetBackgroundDrawable(null);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            lista.ItemClick += Lista_ItemClick;


            btnBuscar.Click += BtnBuscar_Click;

            btnEscanear.Click += async delegate {

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

        private void Lista_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var RentalId = alquiler[e.Position].RentaId;

            List<RentalDetailsWS> Detailsrenta = Service.RentalDetailList(RentalId);

            //SE INFLA LA VISTA CON EL LAYOUT DETALLE DE LIBRO
            View view = LayoutInflater.Inflate(Resource.Layout.framentdetailspendinglist, null);

            //SE CREA EL POP UP DEL LIBRO
                AlertDialog builder = new AlertDialog.Builder((Activity)rootView.Context)
               .SetTitle("Detalles del Prestamo")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo)
               .SetNegativeButton("Cerrar",(IDialogInterfaceOnClickListener)null)
               .SetPositiveButton("Devolver",delegate 
               {                  
                   var cualquiernombre=Service.RentalReturns(alquiler[e.Position].CodigoAlquiler);

                   if (cualquiernombre.band)
                   {
                       Toast.MakeText(rootView.Context,cualquiernombre.message, ToastLength.Long).Show();
                   }
                   else
                       Toast.MakeText(rootView.Context, cualquiernombre.message, ToastLength.Long).Show();

               }).Create();

            //ASIGNACION DE TEXTOS :v
            detalle = view.FindViewById<ListView>(Resource.Id.listView1);

            detalle.Adapter = new AdapterPendingDetails((Activity)rootView.Context, Detailsrenta);

            builder.SetView(view);
            builder.Show();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
             var cliente = Service.GetCustomerData(edtCodigoC.Text);

            if (cliente!=null)
            {
                edtNombresA.Text = cliente.Nombres + " " + cliente.Apellidos;

                //BUSCA ALQUILERES DEL CLIENTE
                 alquiler = Service.PendingByCustomer(cliente.Id);


                if (alquiler != null)
                {
                    //LLENAR EL ENCABEZADO
                    edtCodigoC.Text = cliente.Codigo;
                    edtNombresA.Text = cliente.Nombres + " " + cliente.Apellidos;

                    //LLENAR EL ADAPTADOR
                    lista.Adapter = new AdapterPendingList((Activity)rootView.Context, alquiler);
                }
                else
                    Toast.MakeText(rootView.Context, "El cliente no tiene alquileres pendientes", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(rootView.Context, "Ingrese el número de cédula", ToastLength.Long).Show();
        }

        void HandleScanResult(ZXing.Result result)
        {
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                //BUSCAR EL CLIENTE
                var cliente = Service.GetCustomerData(result.Text);

                //SI SE ENCUENTRA EL CLIENTE
                if (cliente != null)
                {
                    //BUSCA ALQUILERES DEL CLIENTE
                    alquiler = Service.PendingByCustomer(cliente.Id);

                    if (alquiler != null)
                    {
                        //LLENAR EL ENCABEZADO
                        edtCodigoC.Text = cliente.Codigo;
                        edtNombresA.Text = cliente.Nombres + " " + cliente.Apellidos;

                        //LLENAR EL ADAPTADOR
                        lista.Adapter = new AdapterPendingList((Activity)rootView.Context,alquiler);
                    }
                    else
                        Toast.MakeText(rootView.Context, "El cliente no tiene alquileres pendientes", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(rootView.Context, "No existe el cliente con ese codigo", ToastLength.Long).Show();
            }
            else
                Toast.MakeText(rootView.Context, "Vuelva a escanear", ToastLength.Long).Show();
        }
    }
}