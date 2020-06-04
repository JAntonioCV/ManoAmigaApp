using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.Design.Internal;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace ManoAmigaApp
{
    [Activity(Label = "Cliente", Theme = "@style/AppTheme")]
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    public class ActivityOperationsCustomer : Activity
    {
        BottomNavigationView bottomNavigation;
        Toolbar toolbar;
        ISharedPreferences shared;
        ISharedPreferencesEditor editor;

        private const int INTERVALO = 20; //2 segundos para salir
        private long tiempoPrimerClick;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.operationscustomer);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            //Activando los iconos y textos al mismo tiempo
            SetShiftMode(bottomNavigation,false,false);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            shared = PreferenceManager.GetDefaultSharedPreferences(this);
            editor = shared.Edit();

            Fragment fragment = new FragmentBookList();

            CargarFragment(fragment);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menutoolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent i;
            switch (item.ItemId)
            {
                case Resource.Id.cerrar:
                    //SOLO ELIMINAMOS LA CONTRASEÑA Y NO TODAS LAS VARIABLES ALMACENADAS  
                    editor.Remove("contrase").Apply();
                    i = new Intent(this, typeof(MainActivity));
                    StartActivity(i);
                    return true;
                case Resource.Id.cambiar:
                    i = new Intent(this, typeof(ActivityPersonalizacion));
                    StartActivity(i);
                    return true;
                case Resource.Id.contact:
                     i = new Intent(this, typeof(ActivityContact));
                    StartActivity(i);
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>
        /// Metodo cuando se seleciona una pestaña del bottomNavigationView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            Fragment fragment = null;
            switch (e.Item.ItemId)
            {
                case Resource.Id.libros:
                    fragment = new FragmentBookList(); 
                    break;
                case Resource.Id.categorias:
                    fragment = new FragmentCategoryList();
                    break;
                case Resource.Id.nuevos:
                    fragment = new FragmentNewBook();
                    break;
                case Resource.Id.pendientes:
                    fragment = new FragmentPendingCustomer();
                    break;
            }

            if (fragment == null)
                return;


            CargarFragment(fragment);

        }


        /// <summary>
        /// Carga los fragamentos que se le pasen por parametro
        /// </summary>
        /// <param name="fragment"></param>
        public void CargarFragment(Fragment fragment)
        {
            FragmentManager.BeginTransaction().
            Replace(Resource.Id.content_frame, fragment).Commit();
        }


        public void SetShiftMode(BottomNavigationView bottomNavigationView, bool enableShiftMode, bool enableItemShiftMode)
        {
            try
            {
                var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
                if (menuView == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to find BottomNavigationMenuView");
                    return;
                }


                var shiftMode = menuView.Class.GetDeclaredField("mShiftingMode");

                shiftMode.Accessible = true;
                shiftMode.SetBoolean(menuView, enableShiftMode);
                shiftMode.Accessible = false;
                shiftMode.Dispose();


                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                    if (item == null)
                        continue;

                    item.SetShiftingMode(enableItemShiftMode);
                    item.SetChecked(item.ItemData.IsChecked);

                }

                menuView.UpdateMenuView();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to set shift mode: {ex}");
            }
        }

        /// <summary>
        /// aqui estuvo josiel castillo
        /// </summary>
        public override void OnBackPressed()
        {
            IMenuItem categorias = bottomNavigation.Menu.GetItem(1);

            if (categorias.IsChecked)
            {
                Fragment fragment = new FragmentCategoryList();
                CargarFragment(fragment);
            }
            else
            {
                int valor = (int)Java.Lang.JavaSystem.CurrentTimeMillis() / 1000;
                long tiempo = tiempoPrimerClick + INTERVALO;


                if (tiempo > valor)
                {
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Vuelve a presionar para salir", ToastLength.Short).Show();
                }
                tiempoPrimerClick = (int)Java.Lang.JavaSystem.CurrentTimeMillis() / 1000;
            }
        }

    }
}