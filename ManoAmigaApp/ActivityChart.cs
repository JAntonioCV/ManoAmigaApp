using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MikePhil.Charting.Animation;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Data;

namespace ManoAmigaApp
{
    [Activity(Label = "ActivityChart")]
    public class ActivityChart : Activity
    {
        private float[] yData = { 25.3f, 10.6f, 66.76f, 44.32f, 46.01f, 16.89f, 23.9f };
        private String[] xData = { "Mitch", "Jessica", "Mohammad"};

        PieChart piechart;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.piechart);

            piechart = FindViewById<PieChart>(Resource.Id.chart);

            piechart.Description.Text = "Libros mas vistos";
            piechart.RotationEnabled = true;
            piechart.HoleRadius = 25f;
            piechart.SetTransparentCircleAlpha(0);
            piechart.SetCenterTextSize(20);
            piechart.SetDrawEntryLabels(true);
            piechart.SetUsePercentValues(true);

            piechart.AnimateX(1000, Easing.EasingOption.EaseInOutCubic);

            AddDataSet();

            piechart.SetTouchEnabled(true);
            // Create your application here
        }


        private void AddDataSet()
        {
            List<PieEntry> yEntry = new List<PieEntry>();
            List<string> xEntry = new List<string>();

            for (int i = 0; i < yData.Length; i++)
            {
                yEntry.Add(new PieEntry(yData[i], xData[i]));
            }
            for (int i = 0; i < xData.Length; i++)
            {
                xEntry.Add(xData[i]);
            }

            PieDataSet pieDataSet = new PieDataSet(yEntry, "Employee Sales");
            pieDataSet.SliceSpace = 2;
            pieDataSet.ValueTextSize = 12;


            int[] colors = { Color.Blue.B*255, Color.Red.R*255, Color.Green.G*255 };

            pieDataSet.SetColors();



        }
        
    }
}