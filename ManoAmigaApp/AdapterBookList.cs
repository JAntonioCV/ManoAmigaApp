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
    class AdapterBookList : BaseAdapter
    {
        Activity Context;
        List<BookWS> list;

        public AdapterBookList(Activity context, List<BookWS> list)
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
            var item = list[position];
            Bitmap bitmap = BitmapFactory.DecodeByteArray(item.Portada, 0, item.Portada.Length);
            View view = convertView;
            if (view == null)
                //view = Context.LayoutInflater.Inflate(Resource.Layout.listbooklist, null);
                  view = Context.LayoutInflater.Inflate(Resource.Layout.feebacklist, null);
            view.FindViewById<TextView>(Resource.Id.textView1).Text = list[position].Titulo;
            view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageBitmap(bitmap);


            return view;
        }

    }
}