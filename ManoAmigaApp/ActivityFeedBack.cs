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
using Android.Support.Design.Widget;
using ManoAmigaApp.com.somee.manoamiga;
using Android.Graphics;

namespace ManoAmigaApp
{
    [Activity(Label = "ActivityFeedBack", Theme = "@style/AppTheme")]
    public class ActivityFeedBack : Activity
    {
        TextView titulo, resumen;
        ImageView foto;
        ListView listacomentarios;
        FloatingActionButton fabn;
        RatingBar ratingBarlibro;
        List<FeedBackWS> list;
        int IdLibro;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.feedbackbook);

            //obteniendo el id
            IdLibro = Intent.GetIntExtra("IdLibro", 0);

            //enlazando 
            titulo = FindViewById<TextView>(Resource.Id.textView1);
            resumen = FindViewById<TextView>(Resource.Id.textView2);
            foto = FindViewById<ImageView>(Resource.Id.imageView1);
            ratingBarlibro = FindViewById<RatingBar>(Resource.Id.ratingBar1);
            fabn = FindViewById<FloatingActionButton>(Resource.Id.botonflotante);
            listacomentarios = FindViewById<ListView>(Resource.Id.listView1);

            //cargando la lista
            list = Service.FeedBackByBook(IdLibro);

            float suma = calcularrating(list);


            var item = list[0];

            //Asignando valores
            Bitmap bitmap = BitmapFactory.DecodeByteArray(item.Portada, 0, item.Portada.Length);
            titulo.Text = item.Titulo;
            resumen.Text = item.Resumen;
            ratingBarlibro.Rating = suma;
            foto.SetImageBitmap(bitmap);

            if (item.Comentario != null && item.Puntaje != 0)
            {
                listacomentarios.Adapter = new AdapterListFeeBack(this, list);
            }
            else
            {
                Toast.MakeText(this, "Sin comentarios para este libro", ToastLength.Long).Show();
            }


            fabn.Click += Fabn_Click;
            listacomentarios.ItemClick += Listacomentarios_ItemClick;
        }

        private void Listacomentarios_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (list[e.Position].ClienteId == Service.CustomerId)
            {
                Toast.MakeText(this, "FELCIDADES SOS ING", ToastLength.Long).Show();

                var item = list[e.Position];

                View view = LayoutInflater.Inflate(Resource.Layout.layout1, null);

                Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this)
                    .SetTitle("Editar Valoración")
                    .SetIcon(Android.Resource.Drawable.IcDialogInfo).Create();
                EditText edtComment = view.FindViewById<EditText>(Resource.Id.editText1);
                edtComment.Text = item.Comentario;
                EditText edtSuggest = view.FindViewById<EditText>(Resource.Id.editText2);
                edtSuggest.Text = item.Sugerencia;
                RatingBar ratingBarComentario = view.FindViewById<RatingBar>(Resource.Id.ratingBar1);
                ratingBarComentario.Rating = item.Puntaje;

                view.FindViewById<Button>(Resource.Id.button1).Click += (send, arg) =>
                {
                    if (edtComment.Text != "" && ratingBarComentario.Rating != 0)
                    {
                        var servicio = Service.CustomerFeedback(float.Parse(ratingBarComentario.Rating.ToString()), edtComment.Text, edtSuggest.Text, Service.CustomerId, IdLibro, DateTime.Now);

                        if (servicio)
                        {
                            Toast.MakeText(this, "Comentario actualizado", ToastLength.Long).Show();
                            list = Service.FeedBackByBook(IdLibro);

                            /// <summary>
                            /// aqui estuvo josiel castillo
                            /// </summary>
                            float suma = calcularrating(list);
                            ratingBarlibro.Rating = suma;

                            listacomentarios.Adapter = new AdapterListFeeBack(this, list);
                            builder.Dismiss();
                        }
                        else
                            Toast.MakeText(this, "Error. Intentelo de nuevo", ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "Llene los campos necesarios", ToastLength.Long).Show();
                };

                builder.SetView(view);
                builder.Show();
            }
        }

        private void Fabn_Click(object sender, EventArgs e)
        {
            var cliente = list.DefaultIfEmpty(null).FirstOrDefault(x => x.ClienteId == Service.CustomerId);

            if (cliente == null)
            {
                View view = LayoutInflater.Inflate(Resource.Layout.layout1, null);

                Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this)
                    .SetTitle("Valoracion de libro")
                    .SetIcon(Android.Resource.Drawable.IcDialogInfo).Create();
                EditText edtComment = view.FindViewById<EditText>(Resource.Id.editText1);
                EditText edtSuggest = view.FindViewById<EditText>(Resource.Id.editText2);
                RatingBar ratingBarComentario = view.FindViewById<RatingBar>(Resource.Id.ratingBar1);

                view.FindViewById<Button>(Resource.Id.button1).Click += (send, arg) =>
                {
                    if (edtComment.Text != "" && ratingBarComentario.Rating != 0)
                    {
                        var servicio = Service.CustomerFeedback(float.Parse(ratingBarComentario.Rating.ToString()), edtComment.Text, edtSuggest.Text, Service.CustomerId, IdLibro, DateTime.Now);

                        if (servicio)
                        {
                            Toast.MakeText(this, "Comentario agregado", ToastLength.Long).Show();
                            list = Service.FeedBackByBook(IdLibro);

                            /// <summary>
                            /// aqui estuvo josiel castillo
                            /// </summary>
                            float suma = calcularrating(list);
                            ratingBarlibro.Rating = suma;

                            listacomentarios.Adapter = new AdapterListFeeBack(this, list);
                            builder.Dismiss();
                        }
                        else
                            Toast.MakeText(this, "Error. Intentelo de nuevo", ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "Llene los campos necesarios", ToastLength.Long).Show();

                };

                builder.SetView(view);
                builder.Show();
            }
            else
                Toast.MakeText(this, "Ya existe un comentario, que se puede editar", ToastLength.Long).Show();
        }

        /// <summary>
        /// aqui estuvo josiel castillo
        /// </summary>
        public static float calcularrating(List<FeedBackWS> feedBackWs)
        {
            var suma = feedBackWs.Sum(x => x.Puntaje);
            suma = suma / feedBackWs.Count();
            return suma;
        }
    }
}