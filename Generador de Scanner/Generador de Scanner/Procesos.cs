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

        private bool obtenerElementosSets(string linea, int caracter, ref List<string> elementos)
        {
            char[] letras = linea.ToCharArray();
            string actual = "";
            string finRango = "";

            bool analizar = true;

            if (letras[caracter] == '+')
                caracter++;

            while (analizar)
            {
                if (letras[caracter] != '\'')
                    actual += Convert.ToString(letras[caracter]);

                caracter++;

                if (letras[caracter] == '\'')
                {
                    analizar = false;
                    caracter++;
                }
            }

            try
            {
                if (letras[caracter] == '+') { }
                    // SOLO SE VALIDA SI ESTÁ FUERA DE LA CADENA 
            }
            catch
            {
                elementos.Add(actual);
            }

            try
            {
                if (letras[caracter] == '+')
                {
                    elementos.Add(actual);
                    obtenerElementosSets(linea, caracter, ref elementos);
                }
                else if (letras[caracter] == '.' && letras[caracter + 1] == '.')
                {
                    caracter = caracter + 2;
                    analizar = true;

                    // Se analiza el tope del rango
                    while (analizar)
                    {
                        if (letras[caracter] != '\'')
                            finRango += Convert.ToString(letras[caracter]);

                        caracter++;

                        if (letras[caracter] == '\'')
                        {
                            analizar = false;
                            caracter++;
                        }
                    }

                    for (int i = (int)Convert.ToChar(actual); i <= (int)Convert.ToChar(finRango); i++)
                    {
                        elementos.Add(Convert.ToString((char)i));
                    }

                    obtenerElementosSets(linea, caracter, ref elementos);
                }
                else
                {
                    return false;
                }

            }
            catch { }


            return true;
        }


        // METODOS DE PROCESO

        /// <summary>
        /// Metodo que analiza el archivo de texto.
        /// </summary>
        /// <param name="Lista">Lista con el archivo de texto</param>
        /// <param name="linea">Numero de linea analizada. Si el archivo falla la variable contenerá la linea incorrecta</param>
        /// <returns>Si el archivo es correcto retorna true</returns>
        public bool AnalizarArchivo(List<string> txt, ref int linea, ref List<Sets> Sets)
        {
            if (FiltrarEspacio(txt[linea]).ToUpper() == "SETS")
            {
                linea++;
                AnalizarSets(txt, ref linea, ref Sets);


                if (FiltrarEspacio(txt[linea]).ToUpper() == "TOKENS")
                {
                    linea++;
                    // ANALIZAR ACTIONS
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }


        public bool AnalizarSets(List<string> txt, ref int linea, ref List<Sets> Sets)
        {
            int numLinea = linea;
            string set = "";

            while (FiltrarEspacio(txt[linea]).ToUpper() != "TOKENS")
            {
                
                set = FiltrarEspacioInteligente(txt[numLinea]);
                string[] fragmentos = set.Split('=');

                Sets SetTemp = new Sets();
                SetTemp.setNombre(fragmentos[0]);

                List<string> elementos = new List<string>();

                if (obtenerElementosSets(fragmentos[1], 0, ref elementos) == false)
                    return false;

                SetTemp.setElementos(elementos);

                Sets.Add(SetTemp);

                linea++;
            }

            if (Sets.Count == 0)
                return false;

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
    }
}
