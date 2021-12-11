
namespace PRASYDE.ControlEscolar.DataAcess.Framework
{
    public class Enumerados
    {

        public enum TipoEnvioCorreo
        {
            CambioContrasena = 1,
            MensajeBienvenida = 2
        }

        public enum Codigos_Respuesta
        {
            Error = 0,
            OK = 200,
            DataRepeted = 207,
            NoContent = 204,
            InternalServerError = 500,
            NoHeaders = 205,
            Unauthorized = 401,
            NoPermission = 427,
            NoAccess = 428,
            ErrorData = 429

        }
    }
}
