using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TransportesMVC.Models;

namespace TransportesMVC.services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "CamionesService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione CamionesService.svc o CamionesService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class CamionesService : ICamionesService
    {
        //creo un instancia de mi contexto que sea visible y alcanzable para toods los metodos
        private readonly TransportesEntities _context;

        //creo un constructor que inicialice el contexto para poder usarlo
        public CamionesService()
        {
            _context = new TransportesEntities();
        }
        public string create_Camion(string Matricula, string Tipo_Camion, string Marca, string Modelo, int Capacidad, double Kilometraje, string UrlFoto, bool Disponibilidad)
        {
            //preparo una respuesta
            string respuesta = "";
            try
            {
                //creo un objto del modelo original para asignarle los valores del exterior
                Camiones _camion = new Camiones();
                _camion.Matricula = Matricula;
                _camion.Tipo_Camion = Tipo_Camion;
                _camion.Marca = Marca;
                _camion.Modelo = Modelo;
                _camion.Capacidad = Capacidad;
                _camion.Kilometraje = Kilometraje;
                _camion.UrlFoto = UrlFoto;
                _camion.Disponibilidad = Disponibilidad;

                //añado e objeto al contexto
                _context.Camiones.Add(_camion);
                //impacto la bd
                _context.SaveChanges();
                //respuesta
                return respuesta = "Camion registrado con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }

        }

        public string delete_Camion(int id)
        {
            //preparo una espuesta
            string respuesta = "";
            try
            {
                //busco el camion (el modelo original)
                Camiones _camion = _context.Camiones.Find(id);
                //elimino el objeto del contexto
                _context.Camiones.Remove(_camion);
                //impacto a la bd
                _context.SaveChanges();
                //respondo
                return respuesta = $"Camion {id} eliminado con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }
        }

        public List<Camiones_DTO> list_camiones(int id)
        {
            //creo y lleno una lista de camiones_DTO utilizando LinQ
            List<Camiones_DTO> list = new List<Camiones_DTO>();
            try
            {
                list = (from c in _context.Camiones
                        where (id == 0 || c.ID_Camion == id)
                        select new Camiones_DTO()
                        {
                            ID_Camion = c.ID_Camion,
                            Matricula = c.Matricula,
                            Marca = c.Marca,
                            Modelo = c.Modelo,
                            Capacidad = c.Capacidad,
                            Kilometraje = c.Kilometraje,
                            UrlFoto = c.UrlFoto,
                            Disponibilidad = c.Disponibilidad,
                            Tipo_Camion = c.Tipo_Camion
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return list;
        }

        public string update_Camion(int id, string Matricula, string Tipo_Camion, string Marca, string Modelo, int Capacidad, double Kilometraje, string UrlFoto, bool Disponibilidad)
        {
            //preparo una respuesta
            //preparo una respuesta
            string respuesta = "";
            try
            {
                //creo un objto del modelo original para asignarle los valores del exterior
                Camiones _camion = new Camiones();
                _camion.Matricula = Matricula;
                _camion.Tipo_Camion = Tipo_Camion;
                _camion.Marca = Marca;
                _camion.Modelo = Modelo;
                _camion.Capacidad = Capacidad;
                _camion.Kilometraje = Kilometraje;
                _camion.UrlFoto = UrlFoto;
                _camion.Disponibilidad = Disponibilidad;

                //modifico el estado en el contexto
                _context.Entry(_camion).State =System.Data.Entity.EntityState.Modified;
                //impcto la bd
                _context.SaveChanges();
                //respondo
                return respuesta = "Camion actualizado con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }

        }
    }
}
