
//AUTOR: DIEGO OLVERA
//FECHA: 06-06-2019
//DESCRIPCIÓN: CLASE QUE GESTIONA LOS OBJETOS DE LAS INSCRIPCIONES

namespace PRASYDE.ControlEscolar.DataAcess
{
    using System;
    using Framework;
    using System.Net;
    using System.Data;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using PRASYDE.ControlEscolar.Entities;

    public class InscripcionDataAccess : BaseDataAccess
    {
        [JsonIgnore]
        public int codigoRespuesta { get; set; }
        [JsonIgnore]
        public string textoRespuesta { get; set; }

        public object Obtener(Guid token, string idPrograma, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();

            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_LISTADO_GRUPOS_PLANTEL", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;
                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@ID_PROGRAMA_EDUCATIVO", SqlDbType.VarChar).Value = idPrograma;

                connection.Open();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(ObjCommand);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int respuesta = 0;
                object objetoFinal = new object();
                objetoFinal = ArmarObjetoGeneral(ds, ref respuesta);

                Codigo_Respuesta = HttpStatusCode.OK;
                codigoRespuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.OK);
                textoRespuesta = "OK";
                miObjetoRespuesta = objetoFinal;

                
            }
            catch (Exception e) {
                codigoRespuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("InscripcionDataAccess-Obtener. ", e.Message.ToString());
            }
            finally { connection.Close(); connection.Dispose(); }

            return miObjetoRespuesta;
        }

        private List<GruposPlantel> ArmarObjetoGeneral(DataSet ds, ref int respuesta)
        {
            List<GruposPlantel> objPrograama = new List<GruposPlantel>();
            try
            {
                DataTable datosGenerales = ds.Tables[0];

                string[] columna = { "idPlantel", "nombrePlantel", "direccion"};
                DataTable dataTable = RegistrosDiferentes.Obtener(datosGenerales, columna);

                foreach (DataRow item in dataTable.Rows)
                {
                    objPrograama.Add(new GruposPlantel
                    {
                        idPlantel = Convert.ToInt16(item["idPlantel"]),
                        nombrePlantel = Convert.ToString(item["nombrePlantel"]),
                        direccion = Convert.ToString(item["direccion"]),
                        grupos = ObtenerGrupos(Convert.ToInt16(item["idPlantel"]), datosGenerales)

                    });
                }
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("InscripcionDataAccess-Obtener. ", e.Message.ToString());
            }
            return objPrograama;
        }

        private List<DetalleGrupoPlantel> ObtenerGrupos(int idPlantel, DataTable datosGenerales)
        {
            List<DetalleGrupoPlantel> objLista = new List<DetalleGrupoPlantel>();
            try
            {
                DataRow[] resultado = datosGenerales.Select("idPlantel ='" + idPlantel + "'");

                foreach (DataRow item in resultado)
                {
                    objLista.Add(new DetalleGrupoPlantel
                    {
                        idGrupo = Convert.ToString(item["idGrupo"]),
                        claveGrupo = Convert.ToString(item["claveGrupo"]),
                        cupo = Convert.ToInt16(item["cupo"]),
                        inscritos = Convert.ToInt16(item["inscritos"]),
                        modalidad = Convert.ToString(item["modalidad"]),
                        detalleModalidad = Convert.ToString(item["detalleModalidad"]),
                        estatusGrupo = Convert.ToInt16(item["estatusGrupo"])
                    });
                }
            }
            catch (Exception e) {
                EscrituraLog.guardar("InscripcionDataAccess-ObtenerGrupos. ", e.Message.ToString());
            }
            return objLista;
        }

        public object Guardar(Inscripcion propiedades, Guid token, string pagina, ref HttpStatusCode Codigo_Respuesta)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            object miObjetoRespuesta = new object();
            try
            {
                SqlCommand ObjCommand = new SqlCommand("USP_GUARDAR_INSCRIPCION", connection);
                ObjCommand.CommandType = CommandType.StoredProcedure;

                ObjCommand.Parameters.Add("@TOKEN", SqlDbType.UniqueIdentifier).Value = token;
                ObjCommand.Parameters.Add("@PAGINA", SqlDbType.VarChar).Value = pagina;

                ObjCommand.Parameters.Add("@ID_ALUMNO", SqlDbType.VarChar).Value = propiedades.idAlumno.Trim();
                ObjCommand.Parameters.Add("@ID_PROGRAMAEDUCATIVO", SqlDbType.VarChar).Value = propiedades.idProgramaEducativo;
                ObjCommand.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar).Value = propiedades.idgrupo.Trim();
                ObjCommand.Parameters.Add("@MONTO_INSCRIPCION", SqlDbType.Decimal).Value = propiedades.pago.montoInscripcion;
                ObjCommand.Parameters.Add("@COLEGIATURA", SqlDbType.Decimal).Value = propiedades.pago.colegiatura;
                ObjCommand.Parameters.Add("@PRIODICIDAD", SqlDbType.Int).Value = propiedades.pago.periodicidad;
                ObjCommand.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar).Value = propiedades.pago.fechaInicio.Trim();

                ObjCommand.Parameters.Add("@RESPUESTA", SqlDbType.Int).Direction = ParameterDirection.Output;

                connection.Open();
                ObjCommand.ExecuteNonQuery();

                int respuesta = Convert.ToInt16(ObjCommand.Parameters["@RESPUESTA"].Value);
                if (respuesta == 200) {

                    ObjetoCorreoElectronico objeto = new ObjetoCorreoElectronico();
                    objeto.tipoCorreo = Convert.ToInt16(Enumerados.TipoEnvioCorreo.MensajeBienvenida);

                    objeto.nombre = propiedades.nombreAlumno;
                    objeto.gradoEducativo = propiedades.gradoEducativo;
                    objeto.programaEducativo = propiedades.programaEducativo;
                    objeto.nombreGrupo = propiedades.nombreGrupo;
                    objeto.modalidad = propiedades.modalidad;
                    objeto.dias = propiedades.dias;
                    objeto.plantel = propiedades.plantel;
                    objeto.direccionPlantel = propiedades.direccionPlantel;
                    objeto.correo = propiedades.correoAlumno;
                    objeto.asunto = "Mensaje de bienvenida";

                    EnvioCorreoElectronico.Enviar(objeto);
                }

                string textoRespuestaServicio = string.Empty;
                int Codigo_Respuesta_servicio = 0;
                Verify.Response(ref respuesta, ref textoRespuestaServicio, ref Codigo_Respuesta_servicio);

                Codigo_Respuesta = (HttpStatusCode)Codigo_Respuesta_servicio;
                codigoRespuesta = respuesta;
                textoRespuesta = textoRespuestaServicio;
                miObjetoRespuesta = new object();
            }
            catch (Exception e) {
                EscrituraLog.guardar("InscripcionDataAccess-Guardar. ", e.Message.ToString());
            }
            finally { connection.Close();  connection.Dispose(); }

            return miObjetoRespuesta;
        }

    }
}
