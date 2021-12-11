
namespace PRASYDE.ControlEscolar.Business.Framework
{
    using System;
    using System.Text.RegularExpressions;

    public static class Seguridad
    {

        //REGRESA TRUE SI EL FORMATO DE CORREO ES VALIDO
        public static bool ValidaCorreo(string CorreoElectronico)
        {
            //return Regex.IsMatch(CorreoElectronico, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            //return Regex.IsMatch(CorreoElectronico, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return Regex.IsMatch(CorreoElectronico, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
        }

        //VALIDA QUE EL VALOR QUE SE ESTA PASANDO SEA NUMERICO
        public static bool IsNumeric(string num)
        {
            try
            {
                int x = Convert.ToInt16(num);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA VALIDAR LA LONGITUD DEL CAMPO 
        public static bool ValidaLongitud(string campo, int longitudCampo)
        {

            if (campo.Length <= longitudCampo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidaLongitud(string campo, int min, int max)
        {

            if (campo.Length >= min && campo.Length <= max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CURP(string inputValue)
        {

            bool isValid = false;
            string patternExpression = string.Empty;

            if (string.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
            }
            else
            {
                //El patron de curp puede contener el caracter Ñ-ñ
                patternExpression = @"^[a-zA-ZñÑ]{4}\d{6}[a-zA-ZñÑ]{6}\d{2}$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        /// <summary>
        /// Indica si la entrada de datos contiene un formato de número teléfonico válido (8 a 10  caracteres númericos, sin separadores de dígitos).
        /// </summary>
        /// <param name="inputValue">Número teléfonico</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool NumeroTelefonicoObligatorio(string inputValue)
        {

            bool isValid = false;
            string patternExpression = string.Empty;

            if (string.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
            }
            else
            {
                //De 8 a 10  caracteres númericos, sin separadores de dígitos.
                patternExpression = @"^\d{8,10}$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        public static bool NumeroTelefonicoNoObligatorio(string inputValue)
        {

            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = true;
            }
            else
            {
                //De 8 a 10  caracteres númericos, sin separadores de dígitos.
                patternExpression = @"^\d{8,10}$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }

            return isValid;
        }

        /// <summary>
        /// Indica si la entrada de datos contiene un formato de código postal válido.
        /// </summary>
        /// <param name="inputValue">Código Postal</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool ZipCode(string inputValue)
        {

            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
            }
            else
            {
                patternExpression = "^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        /// <summary>
        /// Indica si la entrada de datos consta de uno o más caracteres númericos en relación a una longitud especificada. [0-9]{LongitudMin,LongitudMax}
        /// </summary>
        /// <param name="inputValue">Cadena</param>
        /// <param name="LongitudMin">Longitud Minima de la cadena</param>
        /// <param name="LongitudMax">Longitud Máxima de la cadena</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool NumerosObligatorios(string inputValue, int LongitudMin, int LongitudMax)
        {
            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = false;
            }
            else
            {
                patternExpression = "^[-0-9]{LongitudMin,LongitudMax}$";
                patternExpression = patternExpression.Replace("LongitudMin", LongitudMin.ToString());
                patternExpression = patternExpression.Replace("LongitudMax", LongitudMax.ToString());
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }

            return isValid;
        }

        public static bool NumerosNoObligatorios(string inputValue, int LongitudMin, int LongitudMax)
        {
            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = true;
            }
            else
            {
                patternExpression = "^[-0-9]{LongitudMin,LongitudMax}$";
                patternExpression = patternExpression.Replace("LongitudMin", LongitudMin.ToString());
                patternExpression = patternExpression.Replace("LongitudMax", LongitudMax.ToString());
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }

            return isValid;
        }

        /// <summary>
        /// Indica si la entrada de datos consta solo de letras (c/s acento diacrítico), números, espacios y los siguientes caracteres especiales [ ,;.:¿?¡!=()'"-\s ] en relación a una longitud especificada [a-z[Spaces]-A-Z-0-9-áéíóúñÁÉÍÓÚ[Spaces]].
        /// </summary>
        /// <param name="inputValue">Cadena</param>
        /// <param name="LongitudMin">Longitud Minima de la cadena</param>
        /// <param name="LongitudMax">Longitud Máxima de la cadena</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool TextoSimpleObligatorio(string inputValue, int LongitudMin, int LongitudMax, bool validalongitudMax = true)
        {
            bool isValid = false;
            string patternExpression = string.Empty;

            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = false;
            }
            else
            {
                //Nota: Comentario de acontinuación representa todos los caracteres del teclado| ^[a-zA-Z_áéíóúñÑÁÉÍÓÚ0-9.\-,:;"'¡!*¿?\s]{1,255}$
                //String.Format("^([a-zA-Z_áéíóúñÑÁÉÍÓÚ0-9{0}ñÑ]{LongitudMin,LongitudMax}$)", sExpresionCaracteres)
                //string sExpresionCaracteres = string.Format(@"<>,;.:_^`+*~´¨¿¡'?=)(/&%$#{0}!|°¬\-\{1}\{2}\[\]\\\/", Convert.ToChar(34), Convert.ToChar(123), Convert.ToChar(125));
                string sExpresionCaracteres = string.Format(@",;.:¿?¡!=()#$|%@""_/{0}\-\s", Convert.ToChar(34));
                string auxPattern = "^([ a-zA-ZáéíóúñÑÁÉÍÓÚ0-9" + sExpresionCaracteres + "]{LongitudMin,LongitudMax}$)";
                patternExpression = auxPattern;
                patternExpression = patternExpression.Replace("LongitudMin", LongitudMin.ToString());
                if (validalongitudMax == true) { }
                patternExpression = patternExpression.Replace("LongitudMax", LongitudMax.ToString());
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        public static bool TextoSimpleNoObligatorio(string inputValue, int LongitudMin, int LongitudMax)
        {
            bool isValid = false;
            string patternExpression = string.Empty;

            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = true;
            }
            else
            {
                //Nota: Comentario de acontinuación representa todos los caracteres del teclado| ^[a-zA-Z_áéíóúñÑÁÉÍÓÚ0-9.\-,:;"'¡!*¿?\s]{1,255}$
                //String.Format("^([a-zA-Z_áéíóúñÑÁÉÍÓÚ0-9{0}ñÑ]{LongitudMin,LongitudMax}$)", sExpresionCaracteres)
                //string sExpresionCaracteres = string.Format(@"<>,;.:_^`+*~´¨¿¡'?=)(/&%$#{0}!|°¬\-\{1}\{2}\[\]\\\/", Convert.ToChar(34), Convert.ToChar(123), Convert.ToChar(125));
                string sExpresionCaracteres = string.Format(@",;.:¿?¡!=()#$|%@""_/{0}\-\s", Convert.ToChar(34));
                string auxPattern = "^([ a-zA-ZáéíóúñÑÁÉÍÓÚ0-9" + sExpresionCaracteres + "]{LongitudMin,LongitudMax}$)";
                patternExpression = auxPattern;
                patternExpression = patternExpression.Replace("LongitudMin", LongitudMin.ToString());
                patternExpression = patternExpression.Replace("LongitudMax", LongitudMax.ToString());
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }
        /// <summary>
        /// Indica si la entrada de datos consta solo de caracteres Alfa (c/s acento diacrítico) [A-ZÁÉÍÓÚÑ-a-záéíóúñ].
        /// </summary>
        /// <param name="inputValue">Cadena</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool Letters(string inputValue)
        {
            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
            }
            else
            {
                patternExpression = "^[a-zA-ZÁÉÍÓÚáéíóúñÑ]*$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        /// <summary>
        /// Indica si la entrada de datos consta solo de caracteres Alfa (c/s acento diacrítico) en relación a una longitud especificada. [A-ZÁÉÍÓÚÑ-a-záéíóúñ]{LongitudMin,LongitudMax}
        /// </summary>
        /// <param name="inputValue">Cadena</param>
        /// <param name="LongitudMin">Longitud Minima</param>
        /// <param name="LongitudMax">Longitud Máxima</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool letrasObligatorias(string inputValue, int LongitudMin, int LongitudMax)
        {

            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = false;
            }
            else
            {
                patternExpression = "^[a-zA-ZÁÉÍÓÚáéíóúñÑ ]{LongitudMin,LongitudMax}$";
                patternExpression = patternExpression.Replace("LongitudMin", LongitudMin.ToString());
                patternExpression = patternExpression.Replace("LongitudMax", LongitudMax.ToString());
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        public static bool letrasNoObligatorias(string inputValue, int LongitudMin, int LongitudMax)
        {

            bool isValid = false;
            string patternExpression = string.Empty;


            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = true;
            }
            else
            {
                patternExpression = "^[a-zA-ZÁÉÍÓÚáéíóúñÑ ]{LongitudMin,LongitudMax}$";
                patternExpression = patternExpression.Replace("LongitudMin", LongitudMin.ToString());
                patternExpression = patternExpression.Replace("LongitudMax", LongitudMax.ToString());
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        /// <summary>
        /// Indica si la entrada de datos contiene un formato Fecha válido (Considera años biciestos). [dd/MM/aaaa]
        /// </summary>
        /// <param name="inputValue">Fecha</param>
        /// <returns>Booleano True indicando si la entrada es una expresión es válida, de lo contrario False.</returns>
        public static bool FechaObligatorio(string inputValue)
        {
            bool isValid = false;
            string patternExpression = string.Empty;

            if (string.IsNullOrWhiteSpace(inputValue))
            {
                isValid = false;
            }
            else
            {
                //patternExpression = @"^(?:(?:0?[1-9]|1\d|2[0-8])(\/)(?:0?[1-9]|1[0-2]))(\/)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:31(\/)(?:0?[13578]|1[02]))|(?:(?:29|30)(\/)(?:0?[1,3-9]|1[0-2])))(\/)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(29(\/)0?2)(\/)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$";
                patternExpression = @"^([0-2][0-9]|3[0-1])(\/|-)(0[1-9]|1[0-2])\2(\d{4})(\s)([0-1][0-9]|2[0-3])(:)([0-5][0-9])(:)([0-5][0-9])$";
                //patternExpression = @"^(?:(?:0?[1-9]|1\d|2[0-8])(\/)(?:0?[1-9]|1[0-2]))(\/)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:31(\/)(?:0?[13578]|1[02]))|(?:(?:29|30)(\/)(?:0?[1,3-9]|1[0-2])))(\/)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(29(\/)0?2)(\/)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        public static bool FechaNoObligatorio(string inputValue)
        {
            bool isValid = false;
            string patternExpression = string.Empty;

            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = true;
            }
            else
            {
                patternExpression = @"^(?:(?:0?[1-9]|1\d|2[0-8])(\/)(?:0?[1-9]|1[0-2]))(\/)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:31(\/)(?:0?[13578]|1[02]))|(?:(?:29|30)(\/)(?:0?[1,3-9]|1[0-2])))(\/)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(29(\/)0?2)(\/)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }

        public static string ReemplazarCaracteres(string valor)
        {
            string cadena = valor.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#x27;").Replace("/", "&#x2F;");
            return cadena;
        }

        public static bool CadenaImagen(string valor)
        {
            bool isValid = false;
            if (valor.StartsWith("https", System.StringComparison.OrdinalIgnoreCase) || valor.StartsWith("data:image", System.StringComparison.OrdinalIgnoreCase))
            {
                isValid = true;
            }
            return isValid;
        }

        public static bool CadenaVideo(string valor)
        {
            bool isValid = false;
            if (valor.StartsWith("data:video", System.StringComparison.OrdinalIgnoreCase))
            {
                isValid = true;
            }
            return isValid;
        }

        public static bool ValidarFechas(string fechaInicial, string fechaFinal)
        {
            bool isValid = true;
            try
            {
                int diaIni = Convert.ToInt16(fechaInicial.Substring(0, 2));
                int mesIni = Convert.ToInt16(fechaInicial.Substring(3, 2));
                int anioIni = Convert.ToInt16(fechaInicial.Substring(6, 4));

                int diaFin = Convert.ToInt16(fechaFinal.Substring(0, 2));
                int mesFin = Convert.ToInt16(fechaFinal.Substring(3, 2));
                int anioFin = Convert.ToInt16(fechaFinal.Substring(6, 4));

                DateTime Inicial = new DateTime(anioIni, mesIni, diaIni, 0, 0, 0);
                DateTime Final = new DateTime(anioFin, mesFin, diaFin, 0, 0, 0);

                int resultado = DateTime.Compare(Inicial, Final);

                if (resultado < 0)  //LA FECHA INICIAL ES MENOR A LA FECHA FINAL 
                    isValid = true;
                else if (resultado == 0)  //LA FECHA INICIAL Y FINAL SON IGUALES
                    isValid = true;
                else   //LA FECHA INICIAL ES MAYOR A LA FECHA FINAL
                    isValid = false;
            }
            catch (Exception e)
            {
                isValid = false;
            }
            return isValid;
        }

        public static bool NumerosDecimales(string inputValue)
        {
            bool positiveOrNegativeValue = true;

            bool isValid = false;
            string patternExpression = string.Empty;
            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = false;
            }
            else
            {
                if (inputValue.Contains("-")) { positiveOrNegativeValue = false; }

                patternExpression = (positiveOrNegativeValue == true) ? @"^[0-9]+([,\.][0-9]*)?$" : @"^-[0-9]+([,\.][0-9]*)?$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }


        public static bool NumerosDecimalesNoObligatorio(string inputValue)
        {
            bool positiveOrNegativeValue = true;

            bool isValid = false;
            string patternExpression = string.Empty;
            if (string.IsNullOrEmpty(inputValue))
            {
                isValid = true;
            }
            else
            {
                patternExpression = (positiveOrNegativeValue == true) ? @"^[0-9]+([,\.][0-9]*)?$" : @"^-[0-9]+([,\.][0-9]*)?$";
                isValid = Regex.IsMatch(inputValue, patternExpression);
            }
            return isValid;
        }
    }
}
