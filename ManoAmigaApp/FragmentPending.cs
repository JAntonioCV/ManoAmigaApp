using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ManoAmigaApp.com.somee.manoamiga;


namespace ManoAmigaApp
{
    public class FragmentPending : Fragment
    {
        List<Stattics>list;
        ListView listapendientes;
        View rootView;
        ListView detallepending;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            list = Service.Pendientes();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(Resource.Layout.fragmentpendinglist, container, false);

            listapendientes = rootView.FindViewById<ListView>(Resource.Id.listView1);

            listapendientes.Adapter = new AdapterPendingList((Activity)rootView.Context,list);

            listapendientes.ItemClick += Listapendientes_ItemClick;


            return rootView;

        }

        private void Listapendientes_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //ALMACENAR LOS DATOS DEL LIBRO SELECCIONADO
            var RentalId = list[e.Position].RentaId;

            List<RentalDetailsWS> Detailsrenta = Service.RentalDetailList(RentalId);

            //SE INFLA LA VISTA CON EL LAYOUT DETALLE DE LIBRO
            View view = LayoutInflater.Inflate(Resource.Layout.framentdetailspendinglist, null);

            //SE CREA EL POP UP DEL LIBRO
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder((Activity)rootView.Context)
               .SetTitle("Detalles del Prestamo")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo).Create();

            //ASIGNACION DE TEXTOS :v
            detallepending = view.FindViewById<ListView>(Resource.Id.listView1);

            detallepending.Adapter = new AdapterPendingDetails((Activity)rootView.Context, Detailsrenta);

            builder.SetView(view);
            builder.Show();
        }
    }
}