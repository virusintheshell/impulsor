//AUTOR:DIEGO A. OLVERA
//FECHA DE CREACIÓN: 27-02-2022
//DESCRIPCIÓN: CLASE QUE GENERA UN ARCHIVO EN EXCEL DE LAS CALIFICIONES POR GRUPO

namespace PRASYDE.ControlEscolar.DataAcess.ClasesExcel
{
    using System;
    using System.IO;
    using System.Data;
    using ClosedXML.Excel;
    using PRASYDE.ControlEscolar.Entities;
    using PRASYDE.ControlEscolar.DataAcess.Framework;

    public class ExportarCalificaciones
    {
        public static ExcelSheetResponse ExportReport(System.Data.DataSet ds)
        {
            var response = new ExcelSheetResponse();
            var workbook = new XLWorkbook();

            DataTable dataTableNiveles = ds.Tables[0];
            DataTable dataTableAlumnos = ds.Tables[1];
            DataTable dataTableAsignaturas = ds.Tables[2];
            DataTable dataTableTotalEvaluaciones = ds.Tables[3];
            DataTable dataTableInfo = ds.Tables[4];


            foreach (DataRow item in dataTableNiveles.Rows)
            {
                //CREAMOS UNA HOJA POR CADA NIVEL QUE EXISTA Y LE ASIGNAMOS EL NOMBRE A LAS HOJAS
                var worksheet = workbook.Worksheets.Add("Nivel " + item["nivel"].ToString());
                int respuestaCabecera = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

                int contador = 2;

                DataRow[] asignaturas = dataTableAsignaturas.Select("nivel = '" + item["nivel"] + "'");


                foreach (DataRow itemAsignatura in asignaturas)
                {

                    ImprimirTitulos(worksheet, itemAsignatura["clave"].ToString(), itemAsignatura["asignatura"].ToString(), ref contador);

                    contador++;

                    ImprimirCabeceras(worksheet, itemAsignatura["clave"].ToString(), dataTableTotalEvaluaciones, itemAsignatura["asignatura"].ToString(), ref contador);

                    ImprimirCalificaciones(worksheet, itemAsignatura["clave"].ToString(), Convert.ToInt16(item["nivel"]), dataTableAlumnos, dataTableInfo, ref contador);
                }

                worksheet.Columns().AdjustToContents();
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                response.FileArray = memoryStream.ToArray();
                memoryStream.Close();
            }

            return response;
        }


        private static int ImprimirTitulos(IXLWorksheet worksheet, string clave, string asignatura, ref int contador)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);

            try
            {
                worksheet.Cell(contador, 2).Value = "CLAVE: " + clave;
                worksheet.Cell(contador, 2).Style.Font.FontSize = 11;
                worksheet.Cell(contador, 2).Style.Font.Bold = true;

                contador = contador + 1;
                worksheet.Cell(contador, 2).Value = "ASIGNATURA: " + asignatura;
                worksheet.Cell(contador, 2).Style.Font.FontSize = 11;
                worksheet.Cell(contador, 2).Style.Font.Bold = true;
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ExportarCalificaciones-ImprimirTitulos. ", e.Message.ToString());
            }

            return respuesta;
        }

        private static int ImprimirCabeceras(IXLWorksheet worksheet, string clave, DataTable dataTableTotalEvaluaciones,
                                             string asignatura, ref int contador)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {
                contador = contador + 1;

                worksheet.Cell(contador, 2).Value = "NOMBRE";
                worksheet.Cell(contador, 2).Style.Font.FontSize = 10;
                worksheet.Cell(contador, 2).Style.Font.Bold = true;

                DataRow[] parciales = dataTableTotalEvaluaciones.Select("tipoEvaluacion = 'Parcial' AND asignatura ='" + asignatura.ToString() + "'");
               
                int totalColumnas = 0;
                foreach (DataRow item in parciales)
                {
                    totalColumnas = Convert.ToInt16(item["total"]);
                }
                //totalColumnas = Convert.ToInt16(parciales[0][3]);
              
                int totalEvaluaciones = 1;

                int columna = 3;

                for (int i = 1; i <= totalColumnas; i++)
                {
                    worksheet.Cell(contador, columna).Value = "PARCIAL " + totalEvaluaciones.ToString();
                    worksheet.Cell(contador, columna).Style.Font.FontSize = 10;
                    worksheet.Cell(contador, columna).Style.Font.Bold = true;

                    columna++;
                    totalEvaluaciones++;
                }

                worksheet.Cell(contador, columna).Value = "FINAL";
                worksheet.Cell(contador, columna).Style.Font.FontSize = 10;
                worksheet.Cell(contador, columna).Style.Font.Bold = true;

                contador++;

            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExportarCalificaciones-ImprimirCabeceras. ", e.Message.ToString());
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            }
            return respuesta;
        }



        private static int ImprimirCalificaciones(IXLWorksheet worksheet, string clave, int nivel, DataTable dataTableAlumnos, DataTable dataTableInfo, ref int contador)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {

                foreach (DataRow item in dataTableAlumnos.Rows)
                {

                    int idAlumno = Convert.ToInt32(item["idAlumno"]);
                    DataRow[] calficaciones = dataTableInfo.Select("idAlumno = '" + idAlumno + "' AND clave = '" + clave + "' AND nivel ='" + nivel.ToString() + "'");

                    int columna = 2;

                    worksheet.Cell(contador, columna).Value = item["nombreAlumno"];
                    worksheet.Cell(contador, columna).Style.Font.FontSize = 10;


                    foreach (DataRow itemA in calficaciones)
                    {
                        columna++;
                        worksheet.Cell(contador, columna).Value = itemA["calificacion"];
                        worksheet.Cell(contador, columna).Style.Font.FontSize = 10;

                    }
                    contador++;
                }



                contador++;
            }
            catch (Exception e)
            {
                EscrituraLog.guardar("ExportarCalificaciones-ImprimirCalificaciones. ", e.Message.ToString());
                respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.InternalServerError);
            }
            return respuesta;
        }

        private static int Titulossss(IXLWorksheet worksheet, DataTable dataTableInfo, DataTable dataTableUsuarios, DataTable dataTablePreguntas, DataTable dataTableRespuestas)
        {
            int respuesta = Convert.ToInt16(Enumerados.Codigos_Respuesta.OK);
            try
            {

                int contador = 4;
                foreach (DataRow item in dataTableInfo.Rows)
                {
                    int idSeccion = Convert.ToInt32(item["idSeccionFormulario"]);

                    worksheet.Cell(contador, 1).Value = item["nombre"].ToString().ToUpper();
                    worksheet.Cell(contador, 1).Style.Font.FontSize = 9;
                    worksheet.Cell(contador, 1).Style.Font.Bold = true;

                    worksheet.Cell(contador + 1, 2).Value = "Pregunta";
                    worksheet.Cell(contador + 1, 2).Style.Font.FontSize = 9;
                    worksheet.Cell(contador + 1, 2).Style.Font.Bold = true;


                    int contadorUsuario = 3;
                    foreach (DataRow usuarios in dataTableUsuarios.Rows)
                    {
                        worksheet.Cell(contador + 1, contadorUsuario).Value = usuarios["nombre"];
                        worksheet.Cell(contador + 1, contadorUsuario).Style.Font.FontSize = 9;
                        worksheet.Cell(contador + 1, contadorUsuario).Style.Font.Bold = true;
                        contadorUsuario += 1;
                    }

                    DataRow[] preguntas = dataTablePreguntas.Select("idSeccionFormulario =" + idSeccion + "");

                    int contadorColunaPregunta = 2;

                    foreach (var pregunta in preguntas)
                    {
                        contador += 1;
                        worksheet.Cell(contador + 1, contadorColunaPregunta).Value = pregunta["pregunta"];
                        worksheet.Cell(contador + 1, contadorColunaPregunta).Style.Font.FontSize = 9;

                        string[] obj = { };

                        //ObtenerRespuestas(idSeccion, Convert.ToInt32(pregunta["numeroPregunta"]), dataTableRespuestas, ref obj);

                        int contadorColumnaRespuesta = 3;
                        int posicionFila = contador + 1;
                        foreach (var respuesta1 in obj)
                        {

                            worksheet.Cell(posicionFila, contadorColumnaRespuesta).Value = respuesta1;
                            worksheet.Cell(posicionFila, contadorColumnaRespuesta).Style.Font.FontSize = 9;
                            contadorColumnaRespuesta += 1;
                        }

                    }
                    contador += 2;
                }
            }
            catch (Exception e)
            {
                respuesta = Convert.ToInt32(Enumerados.Codigos_Respuesta.InternalServerError);
                EscrituraLog.guardar("ExportarDiagnostico-Titulos. ", e.Message.ToString());
            }

            return respuesta;
        }


    }
}





//OBTENEMOS LAS CALIFIACIONES POR CADA ALUMNO 

//DataRow[] respuestaCatalogo = dataTableInfo.Select("idFormulario =" + Convert.ToInt32(item["idFormulario"]) + "");
//DataTable dataTableInfoGeneral = respuestaCatalogo.CopyToDataTable();

//string[] columna = { "idSeccionFormulario", "nombre" };
//DataTable DataTableTitulo = General.ObtenerRegistrosDiferentes(dataTableInfoGeneral, columna);

//Titulos(worksheet, DataTableTitulo, dataTableUsuarios, dataTableInfoGeneral, dataTableRespuestas);


