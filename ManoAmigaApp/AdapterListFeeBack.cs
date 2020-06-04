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
using Android.Graphics;
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    class AdapterListFeeBack : BaseAdapter
    {
        Activity Context;
        List<FeedBackWS> list;

        public AdapterListFeeBack(Activity context, List<FeedBackWS> list)
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
                view = Context.LayoutInflater.Inflate(Resource.Layout.listfeedbackbook, null);

            var item = list[position];

            Bitmap bitmap = null;

            if (item.CustomerPhoto != null)
            {
                bitmap = BitmapFactory.DecodeByteArray(item.CustomerPhoto, 0, item.CustomerPhoto.Length);
            }

            view.FindViewById<TextView>(Resource.Id.textView1).Text = list[position].NombresCliente;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = list[position].Comentario;
            view.FindViewById<RatingBar>(Resource.Id.ratingBar1).Rating = list[position].Puntaje;
            view.FindViewById<TextView>(Resource.Id.textView3).Text = list[position].FechaValoracion.ToShortDateString();

            if (item.CustomerPhoto == null)
                view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(Resource.Drawable.perfil);
            else
                view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageBitmap(bitmap);



            return view;
        }
    }
}