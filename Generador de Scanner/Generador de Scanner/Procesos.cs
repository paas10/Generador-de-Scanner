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

        private string FiltrarTabs(string linea)
        {
            char[] letras = linea.ToCharArray();
            string linea_sin_tabs = "";

            foreach (char letra in linea)
            {
                if (letra != '\t')
                    linea_sin_tabs += Convert.ToString(letra);
            }

            return linea_sin_tabs;
        }

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

        /// <summary>
        /// Metodo que verifica que la cabecera del token tenga el formato correcto
        /// </summary>
        /// <param name="linea">Porcion del texto con el token</param>
        /// <param name="numero">Numero de token asignado</param>
        /// <param name="error">Por que es incorrecto en dado falle</param>
        /// <returns></returns>
        private bool ObtenerNoToken(string linea, ref int numero, ref string error)
        {
            char[] letra = FiltrarTabs(linea.ToUpper()).ToCharArray();
            string num = "";

            if (letra[0] == 'T' && letra[1] == 'O' && letra[2] == 'K' && letra[3] == 'E' && letra[4] == 'N' && letra[5] == ' ')
            {
                for (int i = 6; i < letra.Length; i++)
                    num += letra[i];

                if (int.TryParse(num, out numero) == true)
                {
                    return true;
                }
                else
                {
                    error = "Identificador del token Inválido";
                    return false;
                }
            }
            else
            {
                error = "Titulo Inválido del token";
                return false;
            }
        }

        private string OrdenarExpresionRegular(string linea)
        {
            char[] caracteres = linea.ToCharArray();
            int cont = 0;
            string ER = "";

            while (caracteres[cont] == ' ')
                cont++;

            while (cont != caracteres.Length)
            {
                if (caracteres[cont] != ' ')
                {
                    ER += Convert.ToString(caracteres[cont]);
                }
                else
                {
                    if (caracteres[cont + 1] != '|' && caracteres[cont - 1] != '|')
                        ER += ".";
                }

                cont++;
            }

            if (ER.Contains("''"))
            {
                char[] letras = ER.ToCharArray();
                string expresionRegular = Convert.ToString(letras[0]);

                try
                {
                    for (int i = 1; i < caracteres.Length - 2; i++)
                    {
                        if (letras[i - 1] != '\'' && letras[i] == '\'' && letras[i + 1] == '\'' && letras[i + 2] != '\'')
                        {
                            expresionRegular += Convert.ToString(letras[i]) + ".";
                        }
                        else
                        {
                            expresionRegular += Convert.ToString(letras[i]);
                        }
                    }
                }
                catch { }


                expresionRegular += Convert.ToString(letras[letras.Length - 1]);

                return expresionRegular;
            }
            else
            {
                return ER;
            }
        }



    // METODOS DE PROCESO PRINCIPAL

        /// <summary>
        /// Metodo que analiza el archivo de texto.
        /// </summary>
        /// <param name="Lista">Lista con el archivo de texto</param>
        /// <param name="linea">Numero de linea analizada. Si el archivo falla la variable contenerá la linea incorrecta</param>
        /// <returns>Si el archivo es correcto retorna true</returns>
        public bool AnalizarArchivo(List<string> txt, ref string error, ref int linea, ref List<Set> Sets, ref List<Token> Tokens)
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
                    if (AnalizarTokens(txt, ref error, ref linea, ref Tokens, Sets) == false)
                        return false;
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

        public bool AnalizarTokens(List<string> txt, ref string error, ref int linea, ref List<Token> Tokens, List<Set> Sets)
        {
            string token = "";

            // mientras no esté analizando la linea inicial de ations
            while (FiltrarEspacio(txt[linea]).ToUpper() != "ACTIONS")
            {
                token = FiltrarTabs(txt[linea]);
                string[] fragmentos = token.Split('=');

                string lineaToken = "";

                // Por si no hay ningun "="
                if (fragmentos.Length == 1)
                {
                    error = "Ausencia de signo =";
                    return false;
                }

                // Por si habia mas de un "="; Se concatenan todos los demás fragmentos excepto el titulo;  
                for (int i = 1; i < fragmentos.Length; i++)
                {
                    lineaToken += fragmentos[i];

                    if (fragmentos.Length > 2 && i != (fragmentos.Length - 1))
                        lineaToken += "=";
                }

                // Creacion de un Token temporal para luego meterlo a la lista de TOKENS
                Token TokenTemp = new Token();

                int numeroToken = 0;

                if (ObtenerNoToken(fragmentos[0], ref numeroToken, ref error) == false)
                    return false;

                TokenTemp.setNumeroToken(numeroToken);

                List<string> lenguajes = new List<string>();
                List<string> palabras = new List<string>();

                if (obtenerElementosER(lineaToken, ref error, ref lenguajes, ref palabras) == false)
                    return false;

                if (lenguajes.Count == 0 && palabras.Count == 0)
                {
                    error = "No se han encontrado elementos del token";
                    return false;
                }

                if (lenguajes.Count != 0)
                {
                    if (EncontrarLenguajes(Sets, lenguajes, ref error) == false)
                        return false;
                }

                if (palabras.Count != 0)
                {
                    if (EncontrarPalabras(Sets, palabras, ref error) == false)
                        return false;
                }

                TokenTemp.setElementos(OrdenarExpresionRegular(lineaToken));

                Tokens.Add(TokenTemp);

                linea++;
            }

            if (Sets.Count == 0)
            {
                error = "No hay ningun TOKEN para ingresar";
                return false;
            }

            return true;
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



        private bool obtenerElementosER(string linea, ref string error, ref List<string> lenguajes, ref List<string> palabras)
        {
            int cont = 0;
            char[] letras = linea.ToCharArray();
            string palabra = "";
            string lenguaje = "";

            while (cont < letras.Length)
            {
                while (letras[cont] == ' ')
                {
                    cont++;

                    if (cont == letras.Length)
                        break;
                }

                if (letras[cont] == '(' || letras[cont] == '|')
                {
                    cont++;

                    if (cont == letras.Length)
                        break;
                }


                if (letras[cont] == '\'')
                {
                    cont++;

                    try
                    {
                        if (letras[cont] == '\'')
                        {
                            try
                            {
                                if (letras[cont + 1] != '\'')
                                {
                                    error = "Elemento vacío";
                                    return false;
                                }
                                else
                                {
                                    palabra = Convert.ToString('\'');
                                    palabras.Add(palabra);

                                    palabra = "";
                                    cont += 2;
                                }
                            }
                            catch
                            {
                                error = "Elemento vacío";
                                return false;
                            }
                        }
                        else
                        {
                            try
                            {
                                // Agrega un elemento hasta que encuentre la comilla simple que cierra
                                while (letras[cont] != '\'')
                                {
                                    palabra += Convert.ToString(letras[cont]);
                                    cont++;
                                }
                                cont++;

                                palabras.Add(palabra);
                                palabra = "";

                            }
                            catch
                            {
                                error = "Comilla Simple Faltante ( ' )";
                                return false;
                            }
                        }
                    }
                    catch
                    {
                        error = "Comilla simple fuera de contexto";
                        return false;
                    }
                    // hasta acá se guardó un elemento entre comillas

                }
                else
                {
                    // si lo primero que se encuentra no está dentro de comillas se guarda el lenguaje

                    while (letras[cont] != ' ' && letras[cont] != '*' && letras[cont] != '+' && letras[cont] != '?' && letras[cont] != '(' && letras[cont] != '|' && letras[cont] != ')')
                    {
                        lenguaje += Convert.ToString(letras[cont]);
                        cont++;

                        if (cont == letras.Length)
                            break;
                    }

                    lenguajes.Add(lenguaje);
                    lenguaje = ""; 
                }

                if (cont == letras.Length)
                    break;

                // Valida el caso en que no venga espacio entre las comillas
                if (!(letras[cont - 2] != '\'' && letras[cont - 1] == '\'' && letras[cont] == '\'' && letras[cont + 1] != '\''))
                {
                    if (letras[cont] == ' ' || letras[cont] == '|' || letras[cont] == '(' || letras[cont] == ')' || letras[cont] == '*' || letras[cont] == '+' || letras[cont] == '?')
                    {
                        cont++;

                        if (cont == letras.Length)
                            break;

                        // Valida si viene algun signo adicional
                        if (letras[cont] == '*' || letras[cont] == '+' || letras[cont] == '?')
                        {
                            cont++;

                            if (cont == letras.Length)
                                break;
                        }
                    }
                    else
                    {
                        error = "Caracter no reconocido ( " + letras[cont] + " )";
                        return false;
                    }
                }
                
            }

            return true;
        }


        private bool EncontrarLenguajes(List<Set> Sets, List<string> lenguajes, ref string error)
        {
            foreach (string lenguaje in lenguajes)
            {
                bool encontrada = false;

                foreach (Set set in Sets)
                {
                    if (set.getNombre() == lenguaje)
                    {
                        encontrada = true;
                        break;
                    }
                }

                if (encontrada == false)
                {
                    error = "No se ha encontrado \"" + lenguaje + "\" definido en los SETS";
                    return false;
                }
                    
            }

            return true;
        }

        private bool EncontrarPalabras(List<Set> Sets, List<string> palabras, ref string error)
        {
            foreach (string palabra in palabras)
            {
                bool encontrada = false;

                foreach (Set set in Sets)
                {
                    encontrada = false;
                    List<string> elementos = set.getElementos();

                    if (elementos.Contains(palabra))
                    {
                        encontrada = true;
                        break;
                    }
                }

                if (encontrada == false)
                {
                    error = "No se ha encontrado \"" + palabra + "\" definido en los SETS";
                    return false;
                }
            }

            return true;
        }
    }
}
