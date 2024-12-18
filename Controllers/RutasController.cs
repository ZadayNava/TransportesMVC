using Antlr.Runtime.Misc;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TransportesMVC.Models;

namespace TransportesMVC.Controllers
{
    public class RutasController : Controller
    {
        //creo una instancia global de mi contexto
        private TransportesEntities context = new TransportesEntities();

        // GET: Rutas
        public ActionResult Index()
        {
            //retorno el modelo exacto de ViewRutasDirectamente
            return View(context.View_Rutas.ToList());
        }

        //GET: View_LinQ
        public ActionResult View_LinQ()
        {
            // la idea de este controlador es poder crear obetos y datos a partir de datos ya existentes de la bd
            // crea una lista de objetos DTO
            List<View_Rutas_DTO> lista_view = new List<View_Rutas_DTO>();

            //lleno mi lista a partir de los datos del contexto usando LinQ
            lista_view = (from r in context.Rutas //el origen del dato, donde 'r' representa cada renglon de la tabla
                          join carg in context.Cargamentos on r.ID_Ruta equals carg.Ruta_ID //unir las tablas usando oin del linQ es similar a los join en SQL
                          //en este caso, 'carg' representa cada renglon de la tbala 'cargamentos', 'on' puedo traducirlos como 'donde' r.ID_Ruta sea igual (equals) a carg.Ruta_ID
                          join cam in context.Camiones on r.Camion_ID equals cam.ID_Camion
                          join cho in context.Choferes on r.Chofer_ID equals cho.ID_Chofer
                          join dir_o in context.Direcciones on r.Direccionorigen_ID equals dir_o.ID_Direccion
                          join dir_d in context.Direcciones on r.Direcciondestino_ID equals dir_d.ID_Direccion
                          //selecciono los datos que necesito para modelar la clase DTO
                          select new View_Rutas_DTO()
                          {
                              C_ = r.ID_Ruta,//en funcion del dato que deseo, selecciono conforma a la declaracion de los join de LinQ arriba
                              ID_Cargamento = carg.ID_Cargamento,
                              cargamento = carg.Descripcion,
                              ID_Direccion_Origen = dir_o.ID_Direccion,
                              Origen = "Calle: " + dir_o.Calle + " # " + dir_o.Numero + " Col. " + dir_o.Colonia + " CP. " + dir_o.CP,
                              Destino = "Calle: " + dir_d.Calle + " # " + dir_d.Numero + " Col. " + dir_d.Colonia + " CP. " + dir_d.CP,
                              Estado_Destino = dir_d.Estado,
                              ID_Chofer = cho.ID_Chofer,
                              Chofer = cho.Nombre + " " + cho.Apellido_Paterno + " " + cho.Apellido_Materno,
                              ID_Camion = cam.ID_Camion,
                              Camión = "Matricula: " + cam.Matricula + " Marca: " + cam.Marca + " Modelo: " + cam.Modelo,
                              Salida = (DateTime)r.Fecha_salida,
                              LLegada_Estimada = (DateTime)r.Fecha_llegadaestimada,
                          }
                ).ToList();
            ViewBag.Title = "Viista creada con LinQ";
            return View(lista_view);
        }

        //GET: EF_Nav
        public ActionResult EF_Nav()
        {
            //agregar importacion de 'using System.Data.Enttity;'
            //.Include() crea una posible navegacion en funcion de los campos virtuales de los modelos de EF
            var rutas = context.Rutas.Include(r => r.Camiones)
                .Include(r => r.Choferes)
                .Include(r => r.Direcciones)
                .Include(r => r.Direcciones1)
                .Include(r => r.Cargamentos);
            ViewBag.Titulo = "Vista de rutas con navegacion EF";
            return View(rutas.ToList());
        }
        // GET: Rutas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rutas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rutas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rutas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rutas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rutas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rutas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
