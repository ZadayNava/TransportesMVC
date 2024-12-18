using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

        //GET:Editar_Camion/{id}
        public ActionResult Editar_Camion(int id)
        {
            if(id > 0)//validar que realmente llegue un ID valido
            {
                Camiones_DTO camion = new Camiones_DTO();//creo una instancia del tipo DTO para pasar informcion desde el contexto a la vista con ayuda en EF y linQ
                using (TransportesEntities context = new TransportesEntities())//cro una instancia de un solo uso de mi contexto
                {
                    //busco a aqul elemento que coincida con el ID
                    //bajo metodo
                    //no puedo colocar directamente un tipo de datos (modelo original) en un DTO, por lo que, primero me calgo de recuperarlo y posteriormente asigno sus valores (mapeo)
                    var camion_aux = context.Camiones.Where(x => x.ID_Camion == id).FirstOrDefault();
                    //otra alternativa
                    var camion_aux2 = context.Camiones.FirstOrDefault(x => x.ID_Camion == id);

                    camion.Matricula = camion_aux.Matricula;//mapeo
                    camion.Marca = camion_aux.Marca;
                    camion.Modelo = camion_aux.Modelo;
                    camion.Capacidad = camion_aux.Capacidad;
                    camion.Kilometraje = camion_aux.Kilometraje;
                    camion.Tipo_Camion = camion_aux.Tipo_Camion;
                    camion.Disponibilidad = camion_aux.Disponibilidad;
                    camion.UrlFoto = camion_aux.UrlFoto;
                    camion.ID_Camion = camion_aux.ID_Camion;

                    //bajo una consulta (usando LinQ)
                    //cuando hago una consulta directa, tengo la oportunidad de asignar valores a tipos de datos mas complejos o diferentes, incluso, pudiendo crear nuevos datos a artir de datos existentes (instancias de clases)
                    camion = (from c in context.Camiones where c.ID_Camion == id select new Camiones_DTO()
                    {
                        ID_Camion = c.ID_Camion,
                        Matricula = c.Matricula,
                        Marca = c.Marca,
                        Modelo = c.Modelo,
                        Capacidad = c.Capacidad,
                        Kilometraje = c.Kilometraje,
                        Tipo_Camion = c.Tipo_Camion,
                        Disponibilidad = c.Disponibilidad,
                        UrlFoto = c.UrlFoto
                    }).FirstOrDefault();
                }//cierre el "using(context)"
                if (camion == null) //valido si realmente recuperé los datos de la bd
                {
                    //sweet alert
                    return RedirectToAction("Index");
                }
                //si todo sale bien, envio a la vista con datos a editar
                ViewBag.Titulo = $"Editar camion {camion.ID_Camion}";
                cargarDDL();
                return View(camion);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        //POST: Editar_Camion
        [HttpPost]
        public ActionResult Editar_Camion(Camiones_DTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Camiones();

                        camion.ID_Camion = model.ID_Camion;
                        camion.Matricula = model.Matricula;
                        camion.Marca = model.Marca;
                        camion.Modelo = model.Modelo;
                        camion.Capacidad = model.Capacidad;
                        camion.Tipo_Camion = model.Tipo_Camion;
                        camion.Disponibilidad = model.Disponibilidad;
                        camion.Kilometraje = model.Kilometraje;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Assets/Imagenes/Camiones/");
                            if (model.UrlFoto.Length == 0)
                            {
                                //la imagen en la BD es null y hay que darle la imagen
                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }

                                imagen.SaveAs(pathdir + filename);
                                camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                            }
                            else
                            {
                                //validar si es la misma o es nueva
                                if (model.UrlFoto.Contains(filename))
                                {
                                    //es la misma
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                                else
                                {
                                    //es diferente
                                    if (!Directory.Exists(pathdir))
                                    {
                                        Directory.CreateDirectory(pathdir);
                                    }

                                    //Borro la imagen anterios
                                    //valido si existe

                                    try
                                    {
                                        string pathdir_old = Server.MapPath("~" + model.UrlFoto); //busco la imagen que catualmente tiene el camión
                                        if (System.IO.File.Exists(pathdir_old)) //valido si existe dicho archivo
                                        {
                                            //procedo a eliminarlo
                                            System.IO.File.Delete(pathdir_old);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Sweet Alert
                                    }

                                    imagen.SaveAs(pathdir + filename);
                                    camion.UrlFoto = "/Assets/Imagenes/Camiones/" + filename;
                                }
                            }
                        }
                        else //si no hya una nueva imagen, paso la misma
                        {
                            camion.UrlFoto = model.UrlFoto;
                        }

                        //Guardar cambios, validar excepciones, redirigir
                        //actualizar el estado de nuestro elemento
                        //.Entry() registrar la entrada de nueva información al contexto y notificar un cambio de estado usando System.Data.Entity.EntityState.Modified
                        context.Entry(camion).State = System.Data.Entity.EntityState.Modified;
                        //impactamos la BD
                        try
                        {
                            context.SaveChanges();
                        }
                        //agregar using desde 'using System.Data.Entity.Validation;'
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
                            //recorro todos los posibles errores de la Entidad Referencial
                            foreach (var error in ex.EntityValidationErrors)
                            {
                                //recorro los detalles de cada error
                                foreach (var validationError in error.ValidationErrors)
                                {
                                    resp += "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                                    resp += validationError.PropertyName;
                                    resp += validationError.ErrorMessage;
                                }
                            }
                            //Sweet Alert
                        }
                        //Sweet Alert
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //Sweet Alert
                    cargarDDL();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //Sweet Alert
                cargarDDL();
                return View(model);
            }
        }
        //GET: Eliminar_CAmion/{ID}
        public ActionResult Eliminar_Camion(int id)
        {
            try
            {
                using (TransportesEntities context = new TransportesEntities()) 
                {
                    //voy a aleminar el camion que deseo eliminar
                    var camion = context.Camiones.FirstOrDefault(x => x.ID_Camion == id);
                    //vlido si realmente existe dicho camion
                    if(camion == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi el camion con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Camiones.Remove(camion);
                    context.SaveChanges();
                    //sweetalert
                    SweetAlert("Eliminado", $"Ha ocurrido un error: ", NotificationType.success);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //sweetalert
                SweetAlert("Opsss...", $"Ha ocurrido un error: {ex.Message}", NotificationType.error);
                return RedirectToAction("Index");
            }
        }
        //GET: Confirmar ELiminar
        public ActionResult Confirmar_Eliminar(int id)
        {
            SweetAlert_Eliminar(id);
            return RedirectToAction("Index");
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

        #region Sweet Alert
        //declaraciones de un htmlhelper personalizado: Digase que alquel metodo auxiliar que me permite construir codigo html/s en tiempo real basado en las acciones del razor/controller
        private void SweetAlert(string title, string msg, NotificationType type)
        {
            var script = "<script languaje='javascript'> " +
                         "Swal.fire({" +
                         "title: '" + title + "'," +
                         "text: '" + msg + "'," +
                         "icon: '" + type + "'" +
                         "});" +
                         "</script>";
            //TempData funciona como ViewBag, pasando informacion del controlador a cualquier parte de mi proyecto, sinedo este, mas observable y con un tiempo de vida mayoe
            TempData["sweetalert"] = script;
        }
        private void SweetAlert_Eliminar(int id)
        {
            var script = "<script languaje='javascript'>" +
                "Swal.fire({" +
                "title: '¿Estás Seguro?'," +
                "text: 'Estás apunto de Eliminar el Camión: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Camiones/Eliminar_Camion/" + id + "';" +
                "} else if (result.isDenied) {  " +
                "Swal.fire('Se ha cancelado la operación','','info');" +
                "}" +
                "});" +
                "</script>";

            TempData["sweetalert"] = script;
        }

        public enum NotificationType //el enum es el que limita los datos, en eset caso limita los tipos de notificaiones para el sweet alert
        {
            error,
            success,
            info,
            question
        }
        #endregion
    }
}