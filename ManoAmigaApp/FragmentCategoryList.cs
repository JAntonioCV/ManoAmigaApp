using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    public class FragmentCategoryList : Fragment
    {
        ListView listacategoria;
        View rootView;
        List<CategoryWS> list;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            list = Service.CategoryList();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            rootView = inflater.Inflate(Resource.Layout.fragmentcategorylist, container, false);

            listacategoria = rootView.FindViewById<ListView>(Resource.Id.listView1);

            listacategoria.Adapter = new AdapterCategoryList((Activity)rootView.Context,list);


            listacategoria.ItemClick += Listacategoria_ItemClick;

            return rootView;

        }

        private void Listacategoria_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            int id=list[e.Position].Id;

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder((Activity)rootView.Context)
            .SetTitle("Opciones")
            .SetIcon(Android.Resource.Drawable.IcDialogInfo)
            .SetMessage("¿Desea ver los libros de esta categoría?")
            .SetNegativeButton("No", (IDialogInterfaceOnClickListener)null)
            .SetPositiveButton("Si", delegate
            {
                //Llamar al fragment de los libros por categoria y le paso el parametro de la categoria
                Bundle bundle = new Bundle();
                bundle.PutInt("IdCategory", id);

                Fragment fragment = new FragmentBooksByCategory();
                fragment.Arguments = bundle;
                FragmentManager.BeginTransaction().
                Replace(Resource.Id.content_frame, fragment).Commit();

            });
            alert.Create().Show();
        }
    }

 }