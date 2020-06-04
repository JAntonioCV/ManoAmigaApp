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
    public class FragmentNewBook : Fragment
    {
        GridView gridView;
        TextView txtcategoria;
        View rootView;
        List<BookWS> listbook;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //Inflamos la vista de los libros
            rootView = inflater.Inflate(Resource.Layout.fragmentbooklist, container, false);

            //Cargamos las lista de los nuevos libros del mes de este mes
            listbook = Service.NewsBook(DateTime.Today);

            //Enlazar el texview
            txtcategoria = rootView.FindViewById<TextView>(Resource.Id.textView1);

            //Al texview le mandamos la descripcion de la cartegoria seleccionada
            txtcategoria.Text = "Nuevas Adquisiciones";

            //Enlazar el grid
            gridView = rootView.FindViewById<GridView>(Resource.Id.gridView1);

            //Adaptamos la lista de libros filtradas
            gridView.Adapter = new AdapterBookList((Activity)rootView.Context, listbook);

            //Evento click del item de libro
            gridView.ItemClick += GridView_ItemClick;

            return rootView;

        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //ALMACENAR LOS DATOS DEL LIBRO SELECCIONADO
            var item = listbook[e.Position];
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
                   i.PutExtra(Intent.ExtraText, "Mirá este nuevo libro " + item.Titulo + " del autor: " + item.Autor + "\nhttps://play.google.com/");
                   i.SetType("text/plain");
                   StartActivity(i);
               })
               .SetNeutralButton("Cerrar", (IDialogInterfaceOnClickListener)null)
               .SetPositiveButton("Ver valoraciones", delegate {


                   Intent i = new Intent((Activity)rootView.Context, typeof(ActivityFeedBack));
                   i.PutExtra("IdLibro", item.Id);
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
    }
}