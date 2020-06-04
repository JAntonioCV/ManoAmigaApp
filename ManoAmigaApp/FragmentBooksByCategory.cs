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
    public class FragmentBooksByCategory : Fragment
    {
        GridView gridView;
        TextView txtcategoria;
        SearchView searchView;
        View rootView;
        List<BookWS> listbook;
        List<BookWS> filtrada;
        List<CategoryWS> listcat;
        CategoryWS selectcateory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            listcat = Service.CategoryList();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);


            //Cargo el parametro categoria pasado en el fragmento anterior
            int Idcategoria = Arguments.GetInt("IdCategory", 0);

            //Cargamos la lista de libros filtrada para saber cual seleciono
            listbook = Service.BooksByCategory(Idcategoria);

            //Inflamos la vista de los libros
            rootView = inflater.Inflate(Resource.Layout.fragmentbooklist, container, false);
            //Encontramos la categoria seleccionada para poder acceder a sus valores mediante esa busqueda
            selectcateory = listcat.Where(x => x.Id == Idcategoria).FirstOrDefault();
            //Enlazar el texview
            txtcategoria = rootView.FindViewById<TextView>(Resource.Id.textView1);
            //Al texview le mandamos la descripcion de la cartegoria seleccionada
            txtcategoria.Text = "Categoria: "+selectcateory.Descripcion;

            //Enlazar el grid
            gridView = rootView.FindViewById<GridView>(Resource.Id.gridView1);
            //Enlazar el searchview
            searchView = rootView.FindViewById<SearchView>(Resource.Id.searchView1);

            //Adaptamos la lista de libros filtradas
            gridView.Adapter = new AdapterBookList((Activity)rootView.Context,listbook);

            searchView.QueryTextChange += SearchView_QueryTextChange;

            //Evento click del item de libro
            gridView.ItemClick += GridView_ItemClick;

            return rootView;


        }

        private void SearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (e.NewText.Length > 0)
            {
                filtrada = listbook.Where(x => x.Titulo.ToUpper().StartsWith(e.NewText.ToUpper())).ToList();
                gridView.Adapter = new AdapterBookList((Activity)rootView.Context, filtrada);
            }
            else
            {
                gridView.Adapter = new AdapterBookList((Activity)rootView.Context, listbook);
            }
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
                   i.PutExtra(Intent.ExtraText, "Me gusta el Libro " + item.Titulo + " del autor: " + item.Autor + "\nhttps://play.google.com/");
                   i.SetType("text/plain");
                   StartActivity(i);
               })
               .SetNeutralButton("Cerrar", (IDialogInterfaceOnClickListener)null)
               .SetPositiveButton("Ver valoraciones", delegate {


                   //Llamando a la actividad feeedback
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