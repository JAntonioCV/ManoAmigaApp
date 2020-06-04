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
    public class FragmentBookList : Fragment
    {
        GridView gridView;
        SearchView searchView;
        View rootView;
        List<BookWS> list;
        List<BookWS> filtrada;
        TextView welcome;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            list = Service.BookList();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            rootView = inflater.Inflate(Resource.Layout.fragmentbooklist, container, false);

            gridView = rootView.FindViewById<GridView>(Resource.Id.gridView1);
            searchView = rootView.FindViewById<SearchView>(Resource.Id.searchView1);
            welcome = rootView.FindViewById<TextView>(Resource.Id.textView1);
            welcome.Text = "Bienvenido " + Service.Fullname;
            welcome.SetTypeface(Typeface.SansSerif, TypefaceStyle.Italic);

            gridView.Adapter = new AdapterBookList((Activity)rootView.Context,list);

            searchView.QueryTextChange += SearchView_QueryTextChange;

            gridView.ItemClick += GridView_ItemClick;

            return rootView;
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //ALMACENAR LOS DATOS DEL LIBRO SELECCIONADO
            var item = list[e.Position];
            //CONVERTIR LA IMAGEN TRAIDA DE LA BD
            Bitmap bitmap = BitmapFactory.DecodeByteArray(item.Portada, 0, item.Portada.Length);

            //SE INFLA LA VISTA CON EL LAYOUT DETALLE DE LIBRO
            View view = LayoutInflater.Inflate(Resource.Layout.bookdetails, null);

            //SE CREA EL POP UP DEL LIBRO
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder((Activity)rootView.Context)
               .SetTitle("Detalles del Libro")
               .SetIcon(Android.Resource.Drawable.IcDialogInfo)               
               .SetNegativeButton("Compartir Libro", delegate {
                   var i = new Intent(Intent.ActionSend);
                   i.PutExtra(Intent.ExtraText, "Me gusta el Libro " + item.Titulo +" del autor: " + item.Autor + "\nhttps://play.google.com/");
                   i.SetType("text/plain");
                   StartActivity(i);
               })
               .SetNeutralButton("Cerrar", (IDialogInterfaceOnClickListener)null)
               .SetPositiveButton("Ver valoraciones", delegate {


                   //Llamando a la actividad feeedback
                   Intent i = new Intent((Activity)rootView.Context,typeof(ActivityFeedBack));
                   i.PutExtra("IdLibro",item.Id);
                   StartActivity(i);


               }).Create();
            //ASIGNACION DE TEXTOS :v
            view.FindViewById<TextView>(Resource.Id.textView1).Text = item.Titulo;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = item.Autor;
            view.FindViewById<TextView>(Resource.Id.textView3).Text = item.Descripcion;
            view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageBitmap(bitmap);

            builder.SetView(view);
            builder.Show();
        }

        private void SearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {

            if (e.NewText.Length > 0)
            {
                filtrada = list.Where(x => x.Titulo.ToUpper().StartsWith(e.NewText.ToUpper())).ToList();
                gridView.Adapter = new AdapterBookList((Activity)rootView.Context, filtrada);
            }
            else
            {
                gridView.Adapter = new AdapterBookList((Activity)rootView.Context, list);
            }

        }


    }
}