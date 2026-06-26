using System;
using System.Collections.Generic;
using System.Text;
using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface ILoginService
    {
        Usuario? Login(string nombreUsuario, string clave, out string mensaje);
        bool CambiarClave(string nombreUsuario, string claveActual, string nuevaClave, out string mensaje);
    }
}