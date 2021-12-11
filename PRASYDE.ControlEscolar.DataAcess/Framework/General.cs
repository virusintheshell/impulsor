using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PRASYDE.ControlEscolar.DataAcess.Framework
{
   public  class General
    {

        public static MemoryStream getStreamFromFile(HttpPostedFile file)
        {
            var stream = new MemoryStream();
            file.InputStream.CopyTo(stream);
            return stream;
        }

        public static bool saveAttachment(MemoryStream stream, string filePath)
        {
            bool result = false;
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllBytes(filePath, stream.GetBuffer());
            result = true;
            return result;
        }

        public static void GuardarImagen(string filepathImage, string cadenaImagen, ref int respuestaImagen)
        {
            try
            {
                string x = string.Empty;

                int index = cadenaImagen.IndexOf(';');
                string cabeceraImagen = cadenaImagen.Substring(0, index);
                string[] tipoImagenSplit = cabeceraImagen.Split('/');
                string tipoImagen = tipoImagenSplit[1];

                if (tipoImagen == "jpeg") { x = cadenaImagen.Replace("data:image/jpeg;base64,", ""); }
                if (tipoImagen == "png") { x = cadenaImagen.Replace("data:image/png;base64,", ""); }

                byte[] imageBytes = Convert.FromBase64String(x);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                if (!Directory.Exists(Path.GetDirectoryName(filepathImage)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filepathImage));
                }
                if (File.Exists(filepathImage)) { File.Delete(filepathImage); }

                image.Save(filepathImage, System.Drawing.Imaging.ImageFormat.Jpeg);

                respuestaImagen = Convert.ToInt16(200);
            }
            catch (Exception e)
            {
                respuestaImagen = Convert.ToInt16(500);
            }
        }

        public static void EliminarImagen(string filepathImage, ref int respuestaImagen)
        {
            try
            {
                if (File.Exists(filepathImage))
                {
                    File.Delete(filepathImage);
                }
                respuestaImagen = Convert.ToInt16(200);
            }
            catch (Exception e)
            {
                respuestaImagen = Convert.ToInt16(500);
            }
        }

        public static DataTable ObtenerRegistrosDiferentes(DataTable dt, string[] columnas)
        {
            DataTable dtRegistrosUnicos = new DataTable();
            dtRegistrosUnicos = dt.DefaultView.ToTable(true, columnas);
            return dtRegistrosUnicos;
        }
    }
}
