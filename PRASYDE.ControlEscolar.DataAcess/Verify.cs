
namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using System.Net;

    public class Verify
    {
        public static void Response(ref int codigoRespuesta, ref string textoRespuesta, ref int Codigo_Respuesta)
        {
            switch (codigoRespuesta)
            {
                case 200: // TODO ES CORRECTO
                    {
                        Codigo_Respuesta = Convert.ToInt16(HttpStatusCode.OK);
                        textoRespuesta = "OK";
                    }
                    break;
                case 204: // TODO ES CORRECTO
                    {
                        Codigo_Respuesta = Convert.ToInt16(HttpStatusCode.OK);
                        textoRespuesta = "No Content";
                    }
                    break;
                case 207: // TODO ES CORRECTO PERO EL EL DATO ESTA REPETIDO EN BASE DE DATOS 
                    {
                        Codigo_Respuesta = Convert.ToInt16(HttpStatusCode.OK);
                        textoRespuesta = "Data repeted";

                    }
                    break;
                case 427: //NO TIENE PERMISOS
                    {
                        Codigo_Respuesta = Convert.ToInt16(HttpStatusCode.OK);
                        textoRespuesta = "No permission";

                    }
                    break;
                case 428: //EL CUPO ES MENOR AL TOTAL DE ALUMNOS INSCRITOS 
                    {
                        Codigo_Respuesta = Convert.ToInt16(HttpStatusCode.OK);
                        textoRespuesta = "Update no valid";

                    }
                    break;
                case 500: // OCURRIO UN ERROR 
                    {
                        Codigo_Respuesta = Convert.ToInt16(HttpStatusCode.InternalServerError);
                        textoRespuesta = "Internal Server Error";
                    }
                    break;
            }
        }
    }
}
