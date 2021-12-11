
using System;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace PRASYDE.ControlEscolar.DataAcess.Framework
{
   public class EscrituraLog
    {
        public static void guardar(string obj, string ex)
        {
            string mensaje = string.Empty;
            string fecha = System.DateTime.Now.ToString("yyyyMMdd");
            string hora = System.DateTime.Now.ToString("HH:mm:ss");

            string archivo = fecha + ".txt";
            string path = ConfigurationManager.AppSettings["logs"] + archivo;

            try
            {
                StreamWriter sw = new StreamWriter(path, true);
                StackTrace stacktrace = new StackTrace();
                sw.WriteLine(obj + " " + hora);
                sw.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + ex);
                sw.WriteLine("");

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                mensaje = e.ToString();
            }
        }
    }
}
