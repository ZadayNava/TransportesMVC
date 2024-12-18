using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TransportesMVC.services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ICamionesService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ICamionesService
    {
        [OperationContract]
        string create_Camion(
            string Matricula,
            string Tipo_Camion,
            string Marca,
            string Modelo,
            int Capacidad,
            double Kilometraje,
            string UrlFoto,
            bool Disponibilidad);


        [OperationContract]
        List<Camiones_DTO> list_camiones(int id);


        [OperationContract]
        string update_Camion(
            int id,
            string Matricula,
            string Tipo_Camion,
            string Marca,
            string Modelo,
            int Capacidad,
            double Kilometraje,
            string UrlFoto,
            bool Disponibilidad);

        [OperationContract]
        string delete_Camion(int id);
    }
}
