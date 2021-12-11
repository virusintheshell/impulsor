



namespace PRASYDE.ControlEscolar.DataAcess.Framework
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using Microsoft.Office.Interop.Excel;

    public class Excel
    {
        public int ExportarExcel() {

            int resultado = 0;


            // Unos valores de muestra para rellenar
            string[,] sArray;
            sArray = new string[5, 3];
            sArray[0, 0] = "Celda 0-0";
            sArray[0, 1] = "Celda 0-1";
            sArray[0, 2] = "Celda 0-2";
            sArray[1, 0] = "Celda 1-0";
            sArray[1, 1] = "Celda 1-1";
            sArray[1, 2] = "Celda 1-2";
            sArray[2, 0] = "Celda 2-0";
            sArray[2, 1] = "Celda 2-1";
            sArray[2, 2] = "Celda 2-2";
            sArray[3, 0] = "Celda 3-0";
            sArray[3, 1] = "Celda 3-1";
            sArray[3, 2] = "Celda 3-2";
            sArray[4, 0] = "Celda 4-0";
            sArray[4, 1] = "Celda 4-1";
            sArray[4, 2] = "Celda 4-2";
            // Crear los objetos necesarios para trabajar con Excel.
            Application obj_Excel;
            Workbook libroexcel;
            Worksheet hojaexcel;
            object misValue = System.Reflection.Missing.Value;
            obj_Excel = new Microsoft.Office.Interop.Excel.Application();
            libroexcel = obj_Excel.Workbooks.Add(misValue);
            hojaexcel = (Worksheet)libroexcel.Worksheets.get_Item(1);

            obj_Excel.Visible = false; // Permite ver o no la hoja en pantalla mientras el programa trabaja con ella.

            for (int f = 1; f < 6; f++)
            {
                for (int n = 1; n < 4; n++)
                {
                    hojaexcel.Cells[f, n] = sArray[f - 1, n - 1];
                }
            }

            libroexcel.SaveAs("Libroexcel.xls");
            libroexcel.Close();
            return resultado;


        }
    }

}
