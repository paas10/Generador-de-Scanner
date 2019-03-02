using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_de_Scanner
{
    class Procesos
    {

    // METODOS DE MANIPULACION DE TEXTO

        /// <summary>
        /// Eliminta todos los espacios y tabs de la linea enviada
        /// </summary>
        /// <param name="linea"> Linea a analizar </param>
        /// <returns> Linea sin espacios </returns>
        public string FiltrarEspacio(string linea)
        {
            char[] letras = linea.ToCharArray();
            string linea_sin_espacios = "";

            foreach (char letra in linea)
            {
                if (letra != ' ' && letra != '\t')
                    linea_sin_espacios += Convert.ToString(letra);
            }
                
            return linea_sin_espacios;
        }

        /// <summary>
        /// Elimina los espacios cuando no estén dentro de comillas simples
        /// </summary>
        /// <param name="linea"> Linea a analizar </param>
        /// <returns> Retorna la linea sin espacios </returns>
        public string FiltrarEspacioInteligente(string linea)
        {
            char[] letras = linea.ToCharArray();
            string linea_sin_espacios = "";
            bool abierto = false;

            foreach (char letra in linea)
            {
                if (letra == '\'' && abierto == false)
                    abierto = true;
                else if (letra == '\'' && abierto == true)
                    abierto = false;

                if (abierto == false)
                {
                    if (letra != ' ' && letra != '\t')
                        linea_sin_espacios += Convert.ToString(letra);
                }
                else
                {
                    linea_sin_espacios += Convert.ToString(letra);
                }
                
            }

            return linea_sin_espacios;
        }

        /// <summary>
        /// Método que obtiene el numero del char ingresado
        /// </summary>
        /// <param name="linea"> Texto enviado </param>
        /// <param name="caracter"> Caracter donde se encuentra el numero </param>
        /// <returns> Numero de caracter en la tabla ASCII </returns>
        private string ObtenerCharset(string linea, ref int caracter)
        {
            char[] letras = linea.ToCharArray();
            string chr = "";

            while (letras[caracter] != ')')
            {
                chr += Convert.ToString(letras[caracter]);
                caracter++;
            }

            caracter++;

            return chr;
        }



    // METODOS DE PROCESO PRINCIPAL

        /// <summary>
        /// Metodo que analiza el archivo de texto.
        /// </summary>
        /// <param name="Lista">Lista con el archivo de texto</param>
        /// <param name="linea">Numero de linea analizada. Si el archivo falla la variable contenerá la linea incorrecta</param>
        /// <returns>Si el archivo es correcto retorna true</returns>
        public bool AnalizarArchivo(List<string> txt, ref string error, ref int linea, ref List<Set> Sets)
        {
            // Analiza si la primera linea es de los SETS
            if (FiltrarEspacio(txt[linea]).ToUpper() == "SETS")
            {
                linea++;
                if (AnalizarSets(txt, ref error, ref linea, ref Sets) == false)
                    return false;

                // Se analiza que el siguiente bloque de texto sean los TOKENS
                if (FiltrarEspacio(txt[linea]).ToUpper() == "TOKENS")
                {
                    linea++;
                    // ANALIZAR ACTIONS
                }
                else
                {
                    error = "No se ha encontrado 'TOKENS' en el archivo";
                    return false;
                }
            }
            else
            {
                error = "No se ha encontrado 'SETS' al inicio del archivo";
                return false;
            }

            return true;
        }



    // ANALISIS SEGMENTADO

        public bool AnalizarSets(List<string> txt, ref string error, ref int linea, ref List<Set> Sets)
        {
            string set = "";

            // mientras no esté analizando la linea inicial de tokens
            while (FiltrarEspacio(txt[linea]).ToUpper() != "TOKENS")
            {
                set = FiltrarEspacioInteligente(txt[linea]);
                string[] fragmentos = set.Split('=');

                string lineaSets = "";

                // Por si no hay ningun "="
                if (fragmentos.Length == 1)
                {
                    error = "Ausencia de signo =";
                    return false;
                }

                // Por si habia mas de un "="; Se concatenan todos los demás fragmentos excepto el titulo;  
                for (int i = 1; i < fragmentos.Length; i++)
                {
                    lineaSets += fragmentos[i];

                    if (fragmentos.Length > 2 && i != (fragmentos.Length - 1))
                        lineaSets += "=";
                }

                // Creacion de un Set temporal para luego meterlo a la lista de SETS
                Set SetTemp = new Set();
                SetTemp.setNombre(fragmentos[0]);

                List<string> elementos = new List<string>();

                if (obtenerElementosSets(lineaSets, 0, ref error, ref elementos) == false)
                    return false;

                SetTemp.setElementos(elementos);

                Sets.Add(SetTemp);

                linea++;
            }

            if (Sets.Count == 0)
            {
                error = "No hay ningun SET para ingresar";
                return false;
            }
                
            return true;
        }

        


        public bool AnalizarTokens(List<string> txt, ref int linea)
        {
            return false;
        }

        public bool AnalizarActions(List<string> txt, ref int linea)
        {
            return false;
        }


    // ANALISIS BAJO NIVEL

        private bool obtenerElementosSets(string linea, int caracter, ref string error, ref List<string> elementos)
        {
            char[] letras = linea.ToCharArray();
            string actual = "";
            string finRango = "";

            bool analizar = true;

            // Validacion
            if (letras.Length == 0)
            {
                error = "Linea Vacía";
                return false;
            }

            // Si hay un (+) continua
            if (letras[caracter] == '+')
            {
                caracter++;

                // verifica si hay otro caracter después del +
                try
                {
                    if (letras[caracter + 1] == ' ') { }
                }
                catch
                {
                    error = "Signo + sin elementos consecutivos";
                    return false;
                }

            }

            // Validacion
            if (letras[caracter] != '\'' && (letras[caracter] != 'C' && letras[caracter + 1] != 'H' && letras[caracter + 2] != 'R'))
            {
                error = "Caracter inválido ( " + letras[caracter] + " )";
                return false;
            }


            // Si encuentra CHR en el texto analiza que caracter es
            if (letras[caracter] == 'C' && letras[caracter + 1] == 'H' && letras[caracter + 2] == 'R')
            {
                caracter = caracter + 4;
                actual = ObtenerCharset(linea, ref caracter);

                // por si la cadena no es tan larga
                try
                {
                    // Si encuentra un rango analiza el limite
                    if (letras[caracter] == '.' && letras[caracter + 1] == '.' && letras[caracter + 2] == 'C' && letras[caracter + 3] == 'H' && letras[caracter + 4] == 'R')
                    {
                        caracter = caracter + 6;
                        finRango = ObtenerCharset(linea, ref caracter);

                        for (int i = Convert.ToInt16(actual); i <= Convert.ToInt16(finRango); i++)
                        {
                            elementos.Add(Convert.ToString((char)i));
                        }
                    }
                    else if (letras[caracter] == '.' && letras[caracter + 1] != '.')
                    {
                        error = "Caracter no reconocido ( " + letras[caracter] + " )";
                        return false;
                    }
                    else
                    {
                        // Si no hay un rango agrega el CHR encontrado 
                        elementos.Add(Convert.ToString(Convert.ToChar(Convert.ToInt16(actual))));
                    }
                }
                catch { }

                if (letras.Length == caracter)
                    return true;

                if (letras[caracter] != '+')
                {
                    error = "Caracter no reconocido ( " + letras[caracter] + " )";
                    return false;
                }
            }

            // Busca y guarda 
            while (analizar)
            {
                // Si no hay comilla guarda lo encontrado
                if (letras[caracter] != '\'')
                    actual += Convert.ToString(letras[caracter]);

                // Avanza una posicion
                caracter++;

                try
                {
                    // Si encuentra comilla deja de analizar
                    if (letras[caracter] == '\'')
                    {
                        analizar = false;
                        caracter++;
                    }
                }
                catch
                {
                    error = "Falta commila simple ( ' )";
                    return false;
                }
                
            }

            try
            {
                if (letras[caracter] == '+')
                {
                    elementos.Add(actual);

                    try
                    {
                        if (obtenerElementosSets(linea, caracter, ref error, ref elementos) == false)
                            return false;
                    }
                    catch
                    {
                        error = "Signo + sin elementos consecutivos";
                        return false;
                    }

                }
                else if (letras[caracter] == '.')
                {
                    try
                    {
                        // Se verifica que haya otro espacio mas en la cadena
                        if (letras[caracter] == '.' && letras[caracter + 1] == '.')
                        {
                            caracter = caracter + 2;
                            analizar = true;

                            // Se analiza el tope del rango
                            while (analizar)
                            {
                                if (letras[caracter] != '\'')
                                    finRango += Convert.ToString(letras[caracter]);

                                caracter++;

                                try
                                {
                                    // Si encuentra comilla deja de analizar
                                    if (letras[caracter] == '\'')
                                    {
                                        analizar = false;
                                        caracter++;
                                    }
                                }
                                catch
                                {
                                    error = "Falta commila simple ( ' )";
                                    return false;
                                }
                            }

                            for (int i = (int)Convert.ToChar(actual); i <= (int)Convert.ToChar(finRango); i++)
                            {
                                elementos.Add(Convert.ToString((char)i));
                            }

                            // Solo se comprueba que aun ensté dentro del rango
                            try
                            {
                                if (letras[caracter] != '+')
                                {
                                    error = "Caracter no reconocido ( " + letras[caracter] + " )";
                                    return false;
                                }
                                else
                                {
                                    if (obtenerElementosSets(linea, caracter, ref error, ref elementos) == false)
                                        return false;
                                }

                            }
                            catch
                            { }
                        }
                        else if (letras[caracter] == '.' && letras[caracter + 1] != '.')
                        {
                            error = "Caracter no reconocido ( " + letras[caracter] + " )";
                            return false;
                        }
                    }
                    catch
                    {
                        error = "Caracter no reconocido ( " + letras[caracter] + " )";
                        return false;
                    }

                }
                else
                {
                    error = "Caracter invalido ( " + letras[caracter] + " )";
                    return false;
                }
                
            }
            catch
            {
                // SI EL INDICE YA ESTÁ FUERA DE LA CADENA SE ALMACENA EL ULTIMO ELEMENTO GUARDADO
                elementos.Add(actual);
            }

            return true;
        }
    }
}
