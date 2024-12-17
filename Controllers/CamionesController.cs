using DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportesMVC.Models;

namespace TransportesMVC.Controllers
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            //crear una lista del modelo original
            List<Camiones> list_camionoes = new List<Camiones>();

            //lleno la lista con elementos existentes dentor del contexto (BD) utilizando EF y LinQ
            using (TransportesEntities context = new TransportesEntities())//es desechable, solo se ejecuta una vez y desaparece
            {
                //lleno mi lista directamente usando linq
                list_camionoes = (from camion in context.Camiones select camion).ToList();
                //otra forma de usar linq es 
                //list_camionoes = context.Camiones.ToList();
                //otra forma de hacerlo
                /* foreach(Camiones cam in context.Camiones){
                 list_camionoes.Add(cam);
                 }*/
            }
            //ViewBag  (forma parte de razor) se caracteriza por hacer uso de una propiedad arbitraria que sirve para pasar informcion desde el controlador a la vista
            ViewBag.Titulo = "Lista de camiones";
            ViewBag.Subtitulo = "Utilizando ASP.NET MVC";

            //ViewData se caracteriza por hacer uso de un atributo arbitrario y tiene el mismo funcionamiento que el ViewBag
            ViewData["Titulo2"] = "Segundo titulo";

            //TempData se caracterizza por premitir crear variables temporales que existen durante la ejecucion del runtime de asp
            // ademas, los temdata me permite compartir informacion nos solo del controlador a la vista, si no tambien entre otras vistas y otros controladores
            //tempData.Add("Clave","Valor");

            //retorno la vista con los datos del modelo
            return View(list_camionoes);
        }

        //GET:Nuevo_CAmion
        public ActionResult Nuevo_Camion()
        {
            ViewBag.Titulo = "Nuevo Camion";
            //cargo los DDL con las opciones del tipo camion
            cargarDDL();
            return View();
        }
        //POST: Nuevo_Camion
        [HttpPost]
        public ActionResult Nuevo_Camion(Camiones_DTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)//verifica si los campos y los tipos de datos corresponden al modelo (DTO)
                {
                    using(TransportesEntities context = new TransportesEntities())//crea una instancia de un unico uso del contexto
                    {
                        var camion = new Camiones();//creo una instancia de un objeto del modelo original (<Proyecto>.Models)
                        //asigno todos los valores del modelo de entrada (DTO) al objeto que sera enviado a la BD
                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Capacidad = model.Capacidad;
                        camion.Kilometraje = model.Kilometraje;
                        camion.Disponibilidad = model.Disponibilidad;

                        //vaido si existe una imagen en la peticion
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);//recupero el nombre de la imagen que viene de la peticion
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");//mapeo la ruta donde guardare mi imagen en el servidor
                            if (!Directory.Exists(pathdir))//si no existe el directorio , lo creo
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);//guardo la imagen en el servidor
                            camion.UrlFoto = "/Assets/Imagenes/Camiones/"+filename; //guardo la ruta y el nombre del archivo para enviarlo a la bd
                            //impacto sobre la bd usando EF
                            context.Camiones.Add(camion);//agrego un nuevo camion al contexto
                            context.SaveChanges(); //impacto la base de datos enviando las modificaciones sufridas en el cotexto
                            //sweet alert
                            return RedirectToAction("Index");  //finalmente, regreso al listado de este mismo controlador (camiones) si es que todo salio bien
                        }
                        else
                        {
                            //sweet alert
                            cargarDDL();
                            return View(model);
                        }
                    }
                }
                else
                {
                    //sweet alert
                    cargarDDL();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //en caso de que ocurra una excepcion, voy a mostrar un mensae cn el error (sweet alert), voy a devolverle a la vista el modelo que causo el conflico(return View(Model)) y vuelvo a cargar el DDL para que esten disponibles esas opciones(cargarDDL())
                //Sweet Alert
                cargarDDL();
                return View(model);
            }
        }
        #region Auxiliares
        private class Opcines
        {
            public string Numero { get; set; }
            public string Descripcion { get; set; }
        }
        public void cargarDDL()
        {
            List<Opcines> Lista_Opciones = new List<Opcines>()
            {
                new Opcines() {Numero = "0", Descripcion = "Seleccione una opcin"},
                new Opcines() {Numero = "1", Descripcion = "Volteo"},
                new Opcines() {Numero = "2", Descripcion = "Redilas"},
                new Opcines() {Numero = "3", Descripcion = "Transporte"}
            };
            ViewBag.ListaTipos = Lista_Opciones;
        }
        #endregion
    }
}