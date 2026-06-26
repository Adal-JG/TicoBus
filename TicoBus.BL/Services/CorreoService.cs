using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace TicoBus.BL.Services
{
    public class CorreoService
    {
        private readonly IConfiguration _configuration;

        public CorreoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnviarCorreo(string destino, string asunto, string cuerpo)
        {
            try
            {
                var correo = _configuration["EmailSettings:Correo"];
                var clave = _configuration["EmailSettings:Clave"];
                var servidor = _configuration["EmailSettings:Servidor"];
                var puertoTexto = _configuration["EmailSettings:Puerto"];

                if (string.IsNullOrWhiteSpace(correo) ||
                    string.IsNullOrWhiteSpace(clave) ||
                    string.IsNullOrWhiteSpace(servidor) ||
                    string.IsNullOrWhiteSpace(puertoTexto))
                {
                    return;
                }

                var puerto = int.Parse(puertoTexto);

                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress("TicoBus", correo));
                mensaje.To.Add(MailboxAddress.Parse(destino));
                mensaje.Subject = asunto;
                mensaje.Body = new TextPart("plain")
                {
                    Text = cuerpo
                };

                using var smtp = new SmtpClient();
                smtp.Connect(servidor, puerto, SecureSocketOptions.StartTls);
                smtp.Authenticate(correo, clave);
                smtp.Send(mensaje);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                // En pruebas locales ignoramos errores de correo.
                // Luego configuramos Gmail con clave de aplicación.
            }
        }
    }
}