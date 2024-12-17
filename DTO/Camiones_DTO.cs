using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Camiones_DTO
    {
        /*DTO => Data transfer object
         * Decoradores de codigo
         * sirven para dar caracteristicas y definiciones especificas a cada campos y/o elemento de una clase
         */


        //data anotation control punto a key
        [Key]
        public int ID_Camion { get; set; }
        [Required]
        [Display (Name = "Matrícula")]//data helper
        public string Matricula { get; set; }
        [Required]
        [Display(Name = "Tipo de camion")]//data helper
        public string Tipo_Camion { get; set; }
        [Required]
        [Display(Name = "Marca")]//data helper
        public string Marca { get; set; }
        [Required]
        [Display(Name = "Modelo")]//data helper
        public string Modelo { get; set; }
        [Required]
        [Display(Name = "Capacidad")]//data helper
        public int Capacidad { get; set; }
        [Required]
        [Display(Name = "Kilometraje")]//data helper
        public double Kilometraje { get; set; }

        [DataType(DataType.ImageUrl)]//data helper
        public string UrlFoto { get; set; }
        [Required]
        [Display(Name = "Disponibilidad")]//data helper
        public bool Disponibilidad { get; set; }
    }
}
