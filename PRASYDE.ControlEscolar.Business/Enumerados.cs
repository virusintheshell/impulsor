
namespace PRASYDE.ControlEscolar.Business
{
    public class Enumerados
    {
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

        public enum Plataforma
        {
            Web = 1,
            Movil = 2
        }

        public enum TipoNotificacion
        {
            Chat = 1,
            Avisos = 2,
            AsignacionTarea = 3,
            TareaEntregada = 4,
            ComentarioTarea = 5,
            Sistesis = 6,
            Pago = 7

        }

        public enum Textos_Respuesta
        {
            OK,
            DataRepeted,
            NoContent,
            InternalServerError,
            NoHeaders,
            Unauthorized,
            NoPermission,
            NoAccess,
            ErrorData

        }

        public enum NombreCatalago
        {
            Jerarquias,
            Permisos,
            Roles,
            Moviles,
            Validaciones,
            Secciones,
            DetalleSecciones,
            CatalogoRespuestas,
            DetalleCatalogoRespuestas,
            DetalleRespuestasFormulario,// SE UTILIZA PARA ARMAR EL FLUJO INTELIGENTE
            FuncionalidadCompania,
            Usuarios,
            Formularios,
            Estados,
            Municipios,
            PuntosPorEstado,
            PuntosEnvio,
            RolesSinRuta,
            UsuariosSinRutaAsignada,
            Proveedores,
            Clientes,
            Productos,
            UsuariosComunicacion,
            ObtenerMensajes,
            Zonas,
            ObtenerZonas,
            Categorias,
            SubCategorias,
            DetalleEntidades
        }




        public enum Contenedor
        {

            usuarios,
            preguntasformularios,
            preguntassecciones,
            respuestasformularios,
            respuestascatalogo,
            respuestasvideos,
            puntos,
            clientes,
            proveedores,
            codigopuntos
        }

        public enum TipoCatalogo
        {
            Texto = 1,
            Imagen = 2
        }

        public enum TipoEnvio
        {

            Roles = 1,
            Usuarios = 2
        }

        public enum ModoEnvio
        {
            Usuarios = 1,
            Puntos = 2
        }

        public enum TipoVigencia
        {

            TieneVigencia = 1,
            NoTieneVigencia = 0
        }
        public enum VecesAplicar
        {
            UnaVez = 1,
            NVeces = 2
        }

        public enum TipoConsulta
        {

            Formularios = 1,
            Referencias = 2

        }

        public enum EstatusRegistro
        {
            Activo = 1,
            Inactivo = 0
        }

        public enum ValidarEjecucion
        {
            CargarFormulario = 1,
            GuardarEjecucion = 2
        }

        //public enum TipoNotificacion
        //{
        //    Usuario_Actualizado = 101,
        //    Formulario_Actualizado = 201,
        //    Formulario_Eliminado = 202,
        //    Envios = 301,
        //    RutaAsignada = 401,
        //    RutaEditada = 402,
        //    Catalogos = 501,
        //    Mensajes = 601
        //}

        public enum TipoNotificacionHeader
        {
            campana = 1,
            sobre = 2,
        }
    }
}
