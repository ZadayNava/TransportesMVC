using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TransportesMVC.services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "CalculadoraService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione CalculadoraService.svc o CalculadoraService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class CalculadoraService : ICalculadoraService //ctrl + . en el ICalculadoraService y le das en implementar interfaz
    {
        public double dividir(double a, double b)
        {
            return a / b;
        }

        public double multiplicar(double a, double b)
        {
            return a * b;
        }

        public double restar(double a, double b)
        {
            return a - b;
        }

        public double sumar(double a, double b)
        {
            return a + b;
        }
    }
}
