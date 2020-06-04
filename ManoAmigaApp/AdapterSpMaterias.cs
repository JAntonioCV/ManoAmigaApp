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
using Java.Lang;
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    class AdapterSpMaterias : BaseAdapter
    {
        Activity context;
        List<CategoryWS> list;

        public AdapterSpMaterias(Activity context, List<CategoryWS> list)
        {
            this.context = context;
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
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = list[position].Descripcion.ToString();

            return view;
        }
    }
}