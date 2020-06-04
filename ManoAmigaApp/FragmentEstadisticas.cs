using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using ManoAmigaApp.com.somee.manoamiga;
using MikePhil.Charting.Animation;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;

namespace ManoAmigaApp
{
    public class FragmentEstadisticas : Fragment
    {
        private float[] yData = { 25.3f, 10.6f, 66.76f };
        private String[] xData = { "Mitch", "Jessica", "Mohammad" };

        View rootView;
        Toolbar toolbar;
        LibroSolicitado[] libroSolicitados;
        ImageButton imageButton;
        TextView txtfecha, txtcantidad;

        FloatingActionButton fabn;
        PieChart piechart;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            libroSolicitados = Service.MostRental(DateTime.Now.Date);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(Resource.Layout.piechart, container, false);

            toolbar = rootView.FindViewById<Toolbar>(Resource.Id.toolbar);
            piechart = rootView.FindViewById<PieChart>(Resource.Id.chart);
            txtfecha = rootView.FindViewById<TextView>(Resource.Id.textView1);
            imageButton = rootView.FindViewById<ImageButton>(Resource.Id.button1);
            txtcantidad = rootView.FindViewById<TextView>(Resource.Id.textView2);
            fabn= rootView.FindViewById<FloatingActionButton>(Resource.Id.botonflotante);

            libroSolicitados = Service.MostRental(DateTime.Now);

            if (libroSolicitados != null)
            {
                txtcantidad.Text = "Cantidad: " + libroSolicitados.Sum(x => x.Cantidad).ToString();
            }
            else
                txtcantidad.Text = "Sin libros este mes";


            imageButton.Click += ImageButton_Click;
            fabn.Click += Fabn_Click;

            piechart.Description.Text = "Libros mas Solicitados";
            piechart.Description.TextSize = 200;
            piechart.RotationEnabled = true;
            piechart.HoleRadius = 25f;
            piechart.CenterText = "Libros";
            piechart.SetTransparentCircleAlpha(0);
            piechart.SetCenterTextSize(20);
            piechart.SetDrawEntryLabels(true);
            piechart.SetDrawCenterText(true);
            piechart.AnimateX(1000, Easing.EasingOption.EaseInOutCubic);

            AddDataSet(libroSolicitados);

            piechart.SetTouchEnabled(true);

            return rootView;
        }


        private void AddDataSet(LibroSolicitado[] libroSolicitados)
        {
            List<PieEntry> yEntry = new List<PieEntry>();

                for (int i = 0; i < yData.Length; i++)
                {
                    yEntry.Add(new PieEntry(libroSolicitados[i].Cantidad, libroSolicitados[i].Titulo));
                }

                PieDataSet pieDataSet = new PieDataSet(yEntry, " ");
                pieDataSet.SliceSpace = 4;
                pieDataSet.ValueTextSize = 12;

                int[] colors = { Color.Gray, Color.Blue, Color.Red };

                pieDataSet.SetColors(colors);

                Legend legend = piechart.Legend;
                legend.Form = Legend.LegendForm.Circle;
                #pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                legend.Position = Legend.LegendPosition.BelowChartCenter;
                #pragma warning restore CS0618 // El tipo o el miembro están obsoletos

                PieData pieData = new PieData(pieDataSet);

                piechart.Data = pieData;

                piechart.Invalidate();
        }

        private void Fabn_Click(object sender, EventArgs e)
        {
            if (piechart.SaveToGallery("chart", 90))
            {
                Toast.MakeText((Activity)rootView.Context, "Se guardo", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText((Activity)rootView.Context, "No se guardo", ToastLength.Short).Show();
            }
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                txtfecha.Text = time.ToShortDateString();

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}