using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ManoAmigaApp.com.somee.manoamiga;


namespace ManoAmigaApp
{
    class AdapterPendingList : BaseAdapter
    {
        Activity Context;
        List<Stattics> list;

        public AdapterPendingList(Activity context, List<Stattics> list)
        {
            Context = context;
            this.list = list;
        }

        public override int Count => list.Count();

        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = Context.LayoutInflater.Inflate(Resource.Layout.listpending, null);


            view.FindViewById<TextView>(Resource.Id.textView1).Text = "Codigo: " + list[position].CodigoAlquiler;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = list[position].Cliente;
            view.FindViewById<TextView>(Resource.Id.textView3).Text = list[position].CantidadLibros.ToString();
            view.FindViewById<TextView>(Resource.Id.textView4).Text = "Prestamo: " + list[position].FechaAlq.ToShortDateString();
            view.FindViewById<TextView>(Resource.Id.textView5).Text = "Devolución: " + list[position].FechaDevo.ToShortDateString();


            if (list[position].FechaDevo < DateTime.Now.Date)
            {
                view.SetBackgroundColor(Color.IndianRed);
            }
           

            return view;

        }
    }
}