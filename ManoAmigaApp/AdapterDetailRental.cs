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
using ManoAmigaApp.com.somee.manoamiga;
using Java.Lang;

namespace ManoAmigaApp
{
    class AdapterDetailRental : BaseAdapter
    {
        Activity context;
        List<CopysBookWS> list;

        public AdapterDetailRental(Activity contexto, List<CopysBookWS> list)
        {
            this.context = contexto;
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
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = list[position].Titulo;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = list[position].NumeroCopia.ToString();



            return view;
        }
    }
}