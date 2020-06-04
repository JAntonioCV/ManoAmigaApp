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
    class AdapterCategoryList : BaseAdapter
    {

        List<BookWS> booklist = Service.BookList();

        Activity Context;
        List<CategoryWS> list;

        public AdapterCategoryList(Activity context, List<CategoryWS> list)
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
                view = Context.LayoutInflater.Inflate(Resource.Layout.listcategorylist, null);
            view.FindViewById<TextView>(Resource.Id.textView1).Text = list[position].Descripcion;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = "Cantidad: " + booklist.Where(x => x.MateriaId == list[position].Id).Count().ToString();

            return view;
        }
    }
}