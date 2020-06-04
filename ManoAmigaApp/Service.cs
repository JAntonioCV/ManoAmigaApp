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
using ManoAmigaApp.com.somee.manoamiga;

namespace ManoAmigaApp
{
    public class Service
    {
        public static LibrarySW service = new LibrarySW();
        //ALMACENAR LOS DATOS DEL CLIENTE LUEGO DE INICIAR SESION O REGISTRARSE
        public static string Email, Fullname, Identification;
        public static int CustomerId;

        /***********************************************************************************************************
         *                                        SERVICIOS DEL CLIENTE
         ***********************************************************************************************************/
        /// <summary>
        /// REALIZA EL REGISTRO DE UN CLIENTE, SOLAMENTE CLIENTE
        /// </summary>
        /// <param name="codigo">SE REFIERE A LA CEDULA</param>
        /// <param name="nombres"></param>
        /// <param name="apellidos"></param>
        /// <param name="email"></param>
        /// <param name="Foto">PUEDE SER NULO</param>
        /// <param name="pass"></param>
        /// <param name="permiso">POR DEFECTO EL PERMISO ES DE USUARIO</param>
        /// <returns></returns>
        public static WebRegisterResult Register(string codigo, string nombres, string apellidos, string email, string pass, string permiso)
        {
            return service.AccountSecurity(email, pass, permiso, codigo, nombres, apellidos);
        }

        /// <summary>
        /// INICIO DE SESION
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static WebLoginResult Login(string email, string pass)
        {
            return service.Login(email, pass);
        }

        /// <summary>
        /// OBTIENE LOS DATOS DEL CLIENTE AL INICIAR SESIÓN
        /// </summary>
        /// <param name="cedula">LA CEDULA CON UNA LONGITUD DE 16 DIGITOS</param>
        /// <returns></returns>
        public static CustomerWS CustomerData(string email)
        {
            return service.CustomerData(email);
        }

        /// <summary>
        /// LISTA DE TODOS LOS LIBROS QUE POSEE LA BIBLIOTECA
        /// </summary>
        /// <returns></returns>
        public static List<BookWS> BookList()
        {
            return service.BookList().ToList();
        }

        /// <summary>
        /// LISTA DE TODAS LAS CATEGORIAS
        /// </summary>
        /// <returns></returns>
        public static List<CategoryWS> CategoryList()
        {
            return service.BookCategory().ToList();
        }

        /// <summary>
        /// LIBROS FILTRADOS POR CATEGORIA
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public static List<BookWS> BooksByCategory(int CategoryId)
        {
            return service.BookListByCategory(CategoryId).ToList();
        }

        /// <summary>
        /// LOS LIBROS MAS NUEVOS OBTENIDOS EN LA BIBLIOTECA
        /// </summary>
        /// <param name="hoy">LA FECHA SE TOMA DESDE LA MAQUINA</param>
        /// <returns></returns>
        public static List<BookWS> NewsBook(DateTime hoy)
        {
            return service.NewBooks(hoy).ToList();
        }

        /// <summary>
        /// MOSTRAR TODOS LOS ALQUILERES REALIZADOS POR EL CLIENTE EN CUESTION
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public static List<RentalWS> RentalCustomer(int CustomerId)
        {
            return service.RentalByCustomer(CustomerId).ToList();
        }

        /// <summary>
        /// MOSTRAR TODOS LOS LIBROS DE LA RENTA EN CUESTION
        /// </summary>
        /// <param name="RentalId">SE MANDA EL ID DEL ALQUILER</param>
        /// <returns></returns>
        public static List<RentalDetailsWS> RentalDetailsCustomer(int RentalId)
        {
            return service.RentalDetails(RentalId).ToList();
        }

        /// <summary>
        /// MUESTA LOS COMENTARIOS Y VALORES DADO UN ID DE LIBRO
        /// </summary>
        /// <param name="Bookid"></param>
        /// <returns></returns>
        public static List<FeedBackWS> FeedBackByBook(int Bookid)
        {
            return service.FeedbackBook(Bookid).ToList();
        }

        /// <summary>
        /// REGISTRA UN COMENTARIO DEL CLIENTE ACERCA DE UN LIBRO
        /// </summary>
        /// <param name="Score">PUNTAJE</param>
        /// <param name="Comment">COMENTARIO</param>
        /// <param name="Tip">SUGERENCIA</param>
        /// <param name="customerId">CLIENTE ID</param>
        /// <param name="BookId">LIBRO ID</param>
        /// <returns></returns>
        public static bool CustomerFeedback(float Score, string Comment, string Tip, int customerId, int BookId, DateTime date)
        {
            return service.FeedbackCustomer(Score, Comment, Tip, customerId, BookId,date);
        }

        /// <summary>
        /// LISTA DE PRESTAMOS PENDIENTES POR CLIENTE
        /// </summary>
        /// <param name="idcientes"></param>
        /// <returns></returns>
        public static List<Stattics> PendingByCustomer(int idcientes)
        {
            return service.PendientesByCustomer(idcientes).ToList();
        }

        /// <summary>
        /// LISTA DE COPIAS DE LIBROS
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns></returns>
        public static List<CopysBookWS> CopiesByBook(string ISBN)
        {
            return service.CopiesBook(ISBN).ToList();
        }

        /// <summary>
        /// BUSCA UN LIBRO POR ISBN
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns></returns>
        public static BookWS SearchBook(string ISBN)
        {
            return service.SearchBook(ISBN);
        }

        /// <summary>
        /// CALCULA EL CODIGO MAXIMO DE UNA RENTA
        /// </summary>
        /// <returns></returns>
        public static string RentalCode()
        {
            return service.RentalCode();
        }

        /// <summary>
        /// CALCULA EL CODIGO MAXIMO DE UN LIBRO
        /// </summary>
        /// <returns></returns>
        public static string BookCode()
        {
            return service.BookCode();
        }

        /// <summary>
        /// Busca Los libros mas solicitados por mes
        /// </summary>
        /// <returns></returns>
        public static LibroSolicitado[] MostRental(DateTime fecha)
        {
            return service.LibrosSolicitados(fecha).ToArray();
        }


        /***********************************************************************************************************
         *                                        SERVICIOS DEL ADMIN
         ***********************************************************************************************************/
        /// <summary>
        /// LISTA TODOS LOS PRESTAMOS PENDIENTES DE TODOS LOS CLIENTES
        /// </summary>
        /// <returns></returns>
        public static List<Stattics> Pendientes()
        {
            return service.Pendientes().ToList();
        }

        /// <summary>
        /// AGREGA UNA RENTA PARA UN CLIENTE
        /// </summary>
        /// <param name="Code">CODIGO DE ALQUILER</param>
        /// <param name="DateNow">FECHA ALQUILER</param>
        /// <param name="ReturnDate">FECHA DEVOLUCION</param>
        /// <param name="RealReturnDate">FECHA REAL DEVOLUCION</param>
        /// <param name="BooksDetails">TODOS LOS LIBROS DE LA RENTA</param>
        /// <returns></returns>
        public static MessageWS AddRental(int ClienteId,string Code, DateTime DateNow, DateTime ReturnDate, DateTime RealReturnDate, List<RentalDetailsWS> BooksDetails)
        {
            return service.AddRental(ClienteId,Code,DateNow, ReturnDate, Convert.ToDateTime("12/12/1900"), BooksDetails.ToArray());
        }

        /// <summary>
        /// DEVOLUCION DE UN PRESTAMO
        /// </summary>
        /// <param name="RentalCode">CODIGO DE LA RENTA</param>
        /// <returns></returns>
        public static MessageWS RentalReturns(string RentalCode)
        {
            return service.RentalReturns(RentalCode);
        }

        /// <summary>
        /// ALMACENA UN NUEVO LIBRO
        /// </summary>
        /// <param name="Code">CODIGO DE LIBRO</param>
        /// <param name="Title"></param>
        /// <param name="ISBN"></param>
        /// <param name="Autor"></param>
        /// <param name="Portada"></param>
        /// <param name="Adquisicion"></param>
        /// <param name="Description">RESUMEN</param>
        /// <param name="Category">MATERIA ID</param>
        /// <returns></returns>
        public static MessageWS AddBook(string Code, string Title, string ISBN, string Autor, byte[] Portada, DateTime Adquisicion, string Description, int Category)
        {
            return service.AddBook(Code, Title, ISBN, Autor, Portada, Adquisicion, Description, Category);
        }

        /// <summary>
        /// ALMACENA UNA COPIA DE LIBRO
        /// </summary>
        /// <param name="NoCopia"></param>
        /// <param name="NoLibro"></param>
        /// <returns></returns>
        public static MessageWS AddCopy(int NoCopia, int NoLibro)
        {
            return service.AddCopy(NoCopia, NoLibro);
        }

        /// <summary>
        /// AGREGA UN NUEVO CLIENTE
        /// </summary>
        /// <param name="Name">NOMBRES</param>
        /// <param name="LastName">APELLIDOS</param>
        /// <param name="Id">CEDULA</param>
        /// <returns></returns>
        public static MessageWS AddCustomer(string Name, string LastName, string Id)
        {
            return service.AddCustomer(Name, LastName, Id);
        }

        /// <summary>
        /// LISTA DE TODOS LOS CLIENTES
        /// </summary>
        /// <returns></returns>
        public static List<CustomerWS> CustumerList()
        {
            return service.CustomerList().ToList();
        }

        /// <summary>
        /// BUSCA UN LIBRO POR SU ISBN
        /// </summary>
        /// <param name="ISBN">ISBN</param>
        /// <returns></returns>
        public static BookWS BookSearch(string ISBN)
        {
            return service.SearchBook(ISBN);
        }

        /// <summary>
        /// LISTA EL DETALLE DE UNA RENTA
        /// </summary>
        /// <param name="RentalId"></param>
        /// <returns></returns>
        public static List<RentalDetailsWS> RentalDetailList(int RentalId)
        {
            return service.RentalDetails(RentalId).ToList().ToList();
        }

        /// <summary>
        /// OBTIENE LA INFORMACION DE UN CLIENTE POR CEDULA
        /// </summary>
        /// <param name="code">CEDULA</param>
        /// <returns></returns>
        public static CustomerWS GetCustomerData(string code)
        {
            return service.CustomerInfo(code);
        }

        /***********************************************************************************************************
         *                                        SERVICIOS DE PERSONALIZACION                                     *
         ***********************************************************************************************************/
        /// <summary>
        /// CAMBIA LA IMAGEN DE PERFIL DEL CLIENTE
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="ImageProfile"></param>
        /// <returns></returns>
        public static MessageWS ImageChange(int customerId, byte[] ImageProfile)
        {
            return service.ImageProfileChange(customerId, ImageProfile);
        }

        /// <summary>
        /// CAMBIA LA CONTRASEÑA DE LA CUENTA DEL CLIENTE
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="OldPass"></param>
        /// <param name="NewPass"></param>
        /// <returns></returns>
        public static MessageWS PasswordChange(string Email, string OldPass, string NewPass)
        {
            return service.PasswordChange(Email, OldPass, NewPass);
        }
    }
}