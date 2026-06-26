using System;
using System.Collections.Generic;
using System.Text;
using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface ILoginService
    {
        Usuario? Login(string nombre, string clave, out string mensaje);
        bool CambiarClave(string nombre, string claveActual, string nuevaClave, out string mensaje);
    }
}
