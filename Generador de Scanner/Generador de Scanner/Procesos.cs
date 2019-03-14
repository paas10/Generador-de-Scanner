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
            char[] letra = linea.ToCharArray();
            string linea_sin_espacios = "";
            bool abierto = false;

            for (int i = 0; i < letra.Length; i++)
            {
                if (letra[i] == '\'' && abierto == false)
                    abierto = true;
                else if (letra[i] == '\'' && abierto == true)
                    abierto = false;

                try
                {
                    if (letra[i] == '\'' && letra[i + 1] == '\'' && letra[i + 2] == '\'')
                    {
                        linea_sin_espacios += Convert.ToString("'''");
                        i = i + 3;
                        abierto = false;
                    }
                }
                catch { }
                

                if (abierto == false)
                {
                    if (letra[i] != ' ' && letra[i] != '\t')
                        linea_sin_espacios += Convert.ToString(letra[i]);
                }
                else
                {
                    linea_sin_espacios += Convert.ToString(letra[i]);
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

        private bool ValidarOperador(char[] letras, ref int caracter, ref string error)
        {
            if (letras[caracter] == '*' || letras[caracter] == '+' || letras[caracter] == '?')
            {

                if (caracter + 1 == letras.Length)
                {
                    caracter++;
                    return true;
                }

                if (letras[caracter + 1] != ' ')
                {
                    error = "Operador ( " + letras[caracter] + " ) en lugar invalido";
                    return false;
                }
                
                try
                {
                    if (letras[caracter - 1] == '(' || letras[caracter - 1] == '|')
                    {
                        error = "Invalido: no se le puede asignar un operador a: " + letras[caracter - 1];
                        return false;
                    }
                }
                catch
                {
                    error = "Invalido: Operador al inicio de la expresión " + letras[caracter];
                    return false;
                }

                try
                {
                    if (letras[caracter + 1] == '*' || letras[caracter + 1] == '+' || letras[caracter + 1] == '?')
                    {
                        error = "Invalido: Dos operadores juntos ( " + letras[caracter] + " " + letras[caracter + 1] + " )";
                        return false;
                    }
                    else
                    {
                        caracter++;
                        return true;
                    }
                }
                catch
                {
                    caracter++;
                    return true;
                }
            }

            return true;
            //if ((letras[caracter] == ' ' || letras[caracter] == ')' || letras[caracter] == '\'') && (letras[caracter] == ))
        }

        /// <summary>
        /// Recibida la expresion regular la normaliza
        /// </summary>
        /// <param name="linea"> Expresion Regular </param>
        /// <returns> Expresion regular Normalizada </returns>
        private string OrdenarExpresionRegular(string linea)
        {
            string Filtrado = "";
            linea = linea.TrimEnd(' ');
            char[] caracteres = linea.ToCharArray();
            int cont = 0;
            string ER = "";
            
            // Elimina los espacios que vengan al inicio
            while (caracteres[cont] == ' ')
                cont++;

            // Agrega los puntos correspondientes
            while (cont != caracteres.Length)
            {
                if (caracteres[cont] != ' ')
                {
                    ER += Convert.ToString(caracteres[cont]);
                    cont++;
                }
                else
                {
                    if (caracteres[cont - 1] == '(' || caracteres[cont + 1] == ')' || caracteres[cont + 1] == '|' || caracteres[cont - 1] == '|'
                        || caracteres[cont + 1] == '*' || caracteres[cont + 1] == '+' || caracteres[cont + 1] == '?')
                    {
                        cont++;
                    }
                    else
                    {
                        if (!(caracteres[cont - 1] == ' '))
                            ER += ".";
                    
                        cont++;
                    }
                }
            }

            // Verifica si hay dos comillas simples juntas y agrega puntos entre ellas
            if (ER.Contains("''"))
            {
                char[] letras = ER.ToCharArray();
                string expresionRegular = Convert.ToString(letras[0]);


                try
                {
                    for (int i = 1; i < caracteres.Length - 1; i++)
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
                
                // Elimina todas las comillas simples
                cont = 0;
                char[] Expresion = expresionRegular.ToCharArray();


                while (cont != Expresion.Length)
                {
                    try
                    {
                        if (Expresion[cont] != '\'')
                            Filtrado += Convert.ToString(Expresion[cont]);
                        else if (Expresion[cont - 1] == '\'' && Expresion[cont] == '\'' && Expresion[cont + 1] == '\'')
                            Filtrado += Convert.ToString('\'');
                    }
                    catch
                    { }

                    cont++;
                }

                string ExpresionRegular = "";
                string[] SeparacionOr = Filtrado.Split('|');

                if (SeparacionOr.Length > 1)
                {
                    for (int i = 0; i < SeparacionOr.Length; i++)
                    {
                        if (i != SeparacionOr.Length - 1)
                            ExpresionRegular += "(" + SeparacionOr[i] + ")|";
                        else
                            ExpresionRegular += "(" + SeparacionOr[i] + ")";
                    }

                    return ExpresionRegular;
                }
                else
                {
                    return Filtrado;
                }
            }
            else
            {
                // Elimina todas las comillas simples
                cont = 0;
                char[] Expresion = ER.ToCharArray();

                while (cont != Expresion.Length)
                {
                    try
                    {
                        if (Expresion[cont] != '\'')
                            Filtrado += Convert.ToString(Expresion[cont]);
                        else if (Expresion[cont - 1] == '\'' && Expresion[cont] == '\'' && Expresion[cont + 1] == '\'')
                            Filtrado += Convert.ToString('\'');
                    }
                    catch
                    { }

                    cont++;
                }

                string ExpresionRegular = ""; 
                string[] SeparacionOr = Filtrado.Split('|');

                // Análisis de como se debe agrupar 
                if (SeparacionOr.Length > 1)
                {
                    for (int i = 0; i < SeparacionOr.Length; i++)
                    {
                        int abierto = 0;
                        int cerrado = 0;
                        bool concatenacion = false;

                        char[] fragmentos = SeparacionOr[i].ToCharArray();

                        for (int j = 1; j < fragmentos.Length; j++)
                        {
                            if (fragmentos[j] == '(')
                                abierto++;
                            else if (fragmentos[j] == ')')
                                cerrado++;
                            else if (fragmentos[j] == '.')
                                concatenacion = true;
                        }

                        if (i != SeparacionOr.Length - 1)
                        {
                            if (concatenacion)
                            {
                                if (abierto == cerrado && abierto != 0)
                                    ExpresionRegular += "(" + SeparacionOr[i] + ")|";
                                else
                                    ExpresionRegular += SeparacionOr[i] + "|";
                            }
                            else
                            {
                                ExpresionRegular += SeparacionOr[i] + "|";
                            }
                        }
                        else
                        {
                            if (concatenacion)
                            {
                                ExpresionRegular += "(" + SeparacionOr[i] + ")";
                            }
                            else
                            {
                                ExpresionRegular += SeparacionOr[i];
                            }
                        }
                            
                    }

                    return agrupacionOperadores(ExpresionRegular);
                }
                else
                {
                    return agrupacionOperadores(Filtrado);
                }
            }
        }

        /// <summary>
        /// Inserta parentesis a la expresion a la cual afecte un operador
        /// </summary>
        /// <param name="linea">Token ingresado</param>
        /// <returns>Token Normalizado</returns>
        public string agrupacionOperadores(string linea)
        {
            char[] caracteres = linea.ToCharArray();
            string ExpresionRegular = "";
            List<int> index = new List<int>();
            List<int> endex = new List<int>();
            bool parentesis = false;
            int abre = 0;
            int cierra = 0;

            if (linea.Contains('*') || linea.Contains('+') || linea.Contains('?'))
            {
                for (int i = 0; i < caracteres.Length; i++)
                {
                    if (caracteres[i] == '*' || caracteres[i] == '+' || caracteres[i] == '?')
                    {
                        index.Add(i);

                        // Se debe detener cuando encuentre el parentesis de apertura
                        try
                        {
                            if (caracteres[i - 1] == ')')
                                parentesis = true;

                            // Encuentra el indice donde hay que abrir parentesis
                            for (int j = index[index.Count - 1]; j >= 0; j--)
                            {
                                if (parentesis)
                                {
                                    if (caracteres[j] == ')')
                                        cierra++;
                                    else if (caracteres[j] == '(')
                                        abre++;

                                    if (caracteres[j] == '(' && cierra == abre)
                                        endex.Add(j);
                                }
                                else
                                {
                                    if (caracteres[j] == '.' || caracteres[j] == '|')
                                    {
                                        endex.Add(j);
                                        break;
                                    }
                                }

                            }
                        }
                        catch
                        { }

                    }
                }

                if (index.Count != 0 && endex.Count != 0)
                {
                    for (int i = 0; i < linea.Length; i++)
                    {
                        ExpresionRegular += caracteres[i];

                        if (endex.Contains(i))
                            ExpresionRegular += "(";

                        if (index.Contains(i))
                            ExpresionRegular += ")";
                    }

                    return ExpresionRegular;
                }
                else
                {
                    return linea;
                }
                
            }
            else
            {
                return linea;
            }

        }

        /// <summary>
        /// Método que opera Posfijo cada Token enviado
        /// </summary>
        /// <param name="Posfijo">Stack de elementos</param>
        /// <param name="Leafs">Lista donde se insertan las hojas</param>
        /// <param name="Sets">Lista con los Sets Existentes</param>
        /// <param name="ExpresionRegular">Token enviado</param>
        /// <param name="cont">Caracter que se está leyendo</param>
        public void ObtenerPosfijo(ref Stack<Node> Posfijo, ref List<Node> Leafs, List<Set> Sets, string ExpresionRegular, ref int cont, ref int leaf)
        {
            string error = "";
            char[] letras = ExpresionRegular.ToCharArray();

            ObtenerPalabraPosfijo(ref Posfijo, ref Leafs, letras, ref cont, ref leaf, Sets);

            if (letras[cont] == ')')
                cont++;

            while (letras.Length != cont)
            {
                Node Operador = new Node();

                if (letras[cont] == '*' || letras[cont] == '+' || letras[cont] == '?')
                {
                    Node C1 = Posfijo.Pop();

                    Operador.setContenido(Convert.ToString(letras[cont]));

                    if (letras[cont] == '*' || letras[cont] == '?')
                        Operador.setNulable(true);
                    else if (letras[cont] == '+')
                        Operador.setNulable(C1.getNulable());

                    Operador.setFirst(C1.getFirst());
                    Operador.setLast(C1.getLast());

                    Operador.setC1(C1);
                    Operador.setC2(null);

                    Posfijo.Push(Operador);
                    cont++;
                }
                else if (letras[cont] == '.' || letras[cont] == '|')
                {
                    Operador.setContenido(Convert.ToString(letras[cont]));
                    cont++;

                    if (letras[cont] == '(')
                    {
                        string segmento = "";

                        for (int i = cont; i < letras.Length; i++)
                            segmento += Convert.ToString(letras[i]);

                        int cont2 = 1;

                        ObtenerPosfijo(ref Posfijo, ref Leafs, Sets, segmento, ref cont2, ref leaf);

                        cont += cont2 - 1;
                    }
                    else 
                    {
                        ObtenerPalabraPosfijo(ref Posfijo, ref Leafs, letras, ref cont, ref leaf, Sets);
                    }

                    Node C2 = Posfijo.Pop();
                    Node C1 = Posfijo.Pop();

                    // First Last -- PUNTO
                    if (Operador.getContenido() == ".")
                    {
                        // Nulable
                        if (C1.getNulable() && C2.getNulable())
                            Operador.setNulable(true);
                        else
                            Operador.setNulable(false);


                        if (C1.getNulable())
                            Operador.setFirst(C1.getFirst() + "," + C2.getFirst());
                        else
                            Operador.setFirst(C1.getFirst());

                        if (C2.getNulable())
                            Operador.setLast(C1.getLast() + "," + C2.getLast());
                        else
                            Operador.setLast(C2.getLast());
                    }
                    // First Last -- OR
                    else if (Operador.getContenido() == "|")
                    {
                        // Nulable
                        if (C1.getNulable() || C2.getNulable())
                            Operador.setNulable(true);
                        else
                            Operador.setNulable(false);

                        Operador.setFirst(C1.getFirst() + "," + C2.getFirst());
                        Operador.setLast(C1.getLast() + "," + C2.getLast());
                    }

                    Operador.setC1(C1);
                    Operador.setC2(C2);

                    Posfijo.Push(Operador);
                }

                try
                {
                    while (letras[cont] == ')')
                        cont++;
                }
                catch { }
                

            }
        }



        /// <summary>
        /// Método que encuentra una palabra en la ER y la mete al Stack de Posfijo
        /// </summary>
        /// <param name="Posfijo"> Variable por Referencia </param>
        /// <param name="Leafs"> Lista en donde se encuentran todas las hojas </param>
        /// <param name="letras"> Cadena con todos los caracteres de la ER </param>
        /// <param name="cont"> Por que posision me voy desplazando </param>
        /// <param name="leaf"> Cantidad de Hojas ingresadas </param>
        /// <param name="Sets"> Sets </param>
        private void ObtenerPalabraPosfijo(ref Stack<Node> Posfijo, ref List<Node> Leafs, char[] letras, ref int cont, ref int leaf, List<Set> Sets)
        {
            String palabra = ""; 
            string error = "";
            List<string> palabras = new List<string>();
            bool analizar = true;
            bool abierto = false;

            //if (letras[cont -1] == '(')
            //    abierto = true;

            if (letras[cont] == '(')
                cont++;

            while (analizar)
            {
                if ((letras[cont] == '*' || letras[cont] == '+' || letras[cont] == '?') && abierto == true)
                {
                    palabra += letras[cont];
                    cont++;
                }

                if (letras[cont] != '.' && letras[cont] != '*' && letras[cont] != '+' && letras[cont] != '?' && letras[cont] != '|' && letras[cont] != ')')
                {
                    palabra += letras[cont];
                    cont++;

                    if (letras[cont] == ')')
                        abierto = false;
                }
                else
                {
                    palabras.Add(palabra);

                    // Si encontró la palabra siendo un lenguaje o un token detiene el ciclo
                    if (EncontrarLenguajes(Sets, palabras, ref error) || EncontrarPalabras(Sets, palabras, ref error))
                    {
                        analizar = false;
                        Node temp = new Node(palabra, false, Convert.ToString(leaf), Convert.ToString(leaf));

                        Posfijo.Push(temp);
                        Leafs.Add(temp);
                        leaf++;
                        palabras.RemoveAt(0);
                    }
                }
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
            while (FiltrarEspacio(txt[linea]) == "")
                linea++;

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

                    if (FiltrarEspacio(txt[linea]).ToUpper() == "ACTIONS")
                    {
                        linea++;
                        if (AnalizarActions(txt, ref error, ref linea) == false)
                            return false;
                    }
                    else
                    {
                        error = "No se ha encontrado 'ACTIONS' en el archivo";
                        return false;
                    }
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

        private bool AnalizarSets(List<string> txt, ref string error, ref int linea, ref List<Set> Sets)
        {
            string set = "";

            // mientras no esté analizando la linea inicial de tokens
            while (FiltrarEspacio(txt[linea]).ToUpper() != "TOKENS")
            {
                while (FiltrarEspacio(txt[linea]) == "")
                    linea++;

                if (FiltrarEspacio(txt[linea]).ToUpper() == "TOKENS")
                    break;

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

                foreach (Set sets in Sets)
                {
                    if (sets.getNombre() == SetTemp.getNombre())
                    {
                        error = "Set invalido, identificador repetido ( " + SetTemp.getNombre() + " )";
                        return false;
                    }
                }

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

        private bool AnalizarTokens(List<string> txt, ref string error, ref int linea, ref List<Token> Tokens, List<Set> Sets)
        {
            string token = "";

            // mientras no esté analizando la linea inicial de ations
            while (FiltrarEspacio(txt[linea]).ToUpper() != "ACTIONS")
            {
                while (FiltrarEspacio(txt[linea]) == "")
                    linea++;

                if (FiltrarEspacio(txt[linea]).ToUpper() == "ACTIONS")
                    break; 

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

                bool AgragarParentesis = false;
                char[] caracteres = OrdenarExpresionRegular(lineaToken).ToCharArray();

                if (caracteres[0] == '(' && caracteres[caracteres.Length - 1] == ')')
                {
                    for (int i = 1; i < caracteres.Length - 1; i++)
                    {
                        if (caracteres[i] == ')')
                        {
                            AgragarParentesis = true;
                            break;
                        }
                    }
                }
                else
                {
                    AgragarParentesis = true;
                }
                

                if (AgragarParentesis)
                    TokenTemp.setElementos("(" + OrdenarExpresionRegular(lineaToken) + ")");
                else
                    TokenTemp.setElementos(OrdenarExpresionRegular(lineaToken));

                foreach (var tokens in Tokens)
                {
                    if (tokens.getNumeroToken() == TokenTemp.getNumeroToken())
                    {
                        error = "Token invalido, identificador repetido ( " + TokenTemp.getNumeroToken() + " )";
                        return false;
                    }
                }

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

        private bool AnalizarActions(List<string> txt, ref string error, ref int linea)
        {
            bool leer = true;
            string lineaAnalizada = FiltrarEspacio(txt[linea]);
            char[] caracteres = lineaAnalizada.ToCharArray();

            if (caracteres[0] == 'E' && caracteres[1] == 'R' && caracteres[2] == 'R' && caracteres[3] == 'O' && caracteres[4] == 'R')
            {
                leer = false;
                error = "No se han declarado ACTIONS";
                return false;
            }

            // mientras no esté analizando la linea inicial de ations
            while (leer)
            {
                while (FiltrarEspacio(txt[linea]) == "")
                    linea++;

                lineaAnalizada = FiltrarEspacio(txt[linea]);
                caracteres = lineaAnalizada.ToCharArray();

                if (caracteres[0] == 'E' && caracteres[1] == 'R' && caracteres[2] == 'R' && caracteres[3] == 'O' && caracteres[4] == 'R')
                    break;

                int cont = 0;
                string action = "";

                // validaciones Nombre del Action
                try
                {
                    while (caracteres[cont] != '(')
                    {
                        action += Convert.ToString(caracteres[cont]);
                        cont++;
                    }
                }
                catch
                {
                    // Si se sale del limite del arreglo es porque no tiene parentesis abierto
                    error = "Caracter Faltante '('";
                    return false;
                }

                // Si no está el parentesis cerrando
                try
                {
                    if (caracteres[cont] == '(' && caracteres[cont + 1] != ')')
                    {
                        error = "Caracter inválido ( " + caracteres[cont + 1] + " )";
                        return false;
                    }
                }
                catch
                {
                    error = "Caracter faltante ')'";
                    return false;
                }

                linea++;

                if (FiltrarEspacio(txt[linea]) != "{")
                {
                    error = "Caracter Inválido ( " + FiltrarEspacio(txt[linea]) + " )\n" +
                        "Se esperaba Abrir Llave ( { )";
                    return false;
                }
                else
                {
                    linea++;
                }

                try
                {
                    while (!FiltrarEspacio(txt[linea]).Equals("}"))
                    {
                        string linAction = FiltrarEspacio(txt[linea]);
                        string[] fragmentos = linAction.Split('=');

                        if (fragmentos.Length == 1)
                        {
                            error = "Ausencia de '='";
                            return false;
                        }

                        int idAction = 0;
                        if (!int.TryParse(fragmentos[0], out idAction))
                        {
                            error = "Identificador no válido";
                            return false;
                        }

                        string unionFragmentos = "";

                        if (fragmentos.Length > 2)
                        {
                            for (int i = 1; i < fragmentos.Length; i++)
                            {
                                unionFragmentos += fragmentos[i];

                                if (i != fragmentos.Length - 1)
                                    unionFragmentos += "=";
                            }
                        }
                        else
                            unionFragmentos = fragmentos[1];

                        char[] contenido = unionFragmentos.ToCharArray();

                        if (contenido[0] != '\'' || contenido[contenido.Length - 1] != '\'')
                        {
                            error = "Sintaxis incorrecta. Ausencia de Comilla Simple (')";
                            return false;
                        }

                        linea++;
                    }
                }
                catch
                {
                    // Si nunca sale del ciclo no hay }
                    error = "Ausencia de Cerrar Llave ( } )";
                    return false;
                }

                if (FiltrarEspacio(txt[linea]).Equals("}"))
                    linea++;

                while (FiltrarEspacio(txt[linea]) == "")
                    linea++;

                lineaAnalizada = FiltrarEspacio(txt[linea]);
                caracteres = lineaAnalizada.ToCharArray();

                if (caracteres[0] == 'E' && caracteres[1] == 'R' && caracteres[2] == 'R' && caracteres[3] == 'O' && caracteres[4] == 'R')
                    leer = false;

            }

            return true;
        }

        private bool AnalizarError(List<string> txt, ref string error, ref int linea)
        {
            // Error con MAYUSCULAS para el error del archivo de texto
            string[] Error = FiltrarEspacio(txt[linea]).Split('=');

            if (Error.Length > 2)
            {
                error = "Signo no reconocido ( = )";
                return false;
            }

            if (Error[0] != "ERROR")
            {
                error = "Palabra no reconocida ( " + Error[0] + " )";
                return false;
            }

            int numError;

            if (!int.TryParse(Error[1], out numError))
            {
                error = "Numero del Error no reconocido ( " + Error[1] + " )";
                return false;
            }

            return true;
        }


    // ANALISIS BAJO NIVEL

        private bool obtenerElementosSets(string linea, int caracter, ref string error, ref List<string> elementos)
        {
            linea = linea.TrimEnd(' ');
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

            // Validacion de Comilla Simple
            if (letras[caracter] == '\'')
            {
                caracter++;

                try
                {
                    if (letras[caracter] == '\'')
                    {
                        try
                        {
                            if (letras[caracter + 1] != '\'')
                            {
                                error = "Elemento vacío";
                                return false;
                            }
                            else
                            {
                                elementos.Add(Convert.ToString('\''));

                                caracter += 2;

                                if (caracter == letras.Length)
                                    return true;
                                else
                                    caracter++;
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
                        caracter--;
                    }
                }
                catch
                {
                    caracter--;
                }
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
            linea = linea.TrimEnd(' ');
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

                if (letras[cont] == '*' || letras[cont] == '+' || letras[cont] == '?')
                {
                    error = "Operador ( " + letras[cont] + " ) en lugar invalido";
                    return false;
                }

                if (letras[cont] == '(' || letras[cont] == '|' || letras[cont] == ')')
                {
                    cont++;

                    if (cont == letras.Length)
                        break;
                }

                while (letras[cont] == ' ')
                {
                    cont++;

                    if (cont == letras.Length)
                        break;
                }

                if (ValidarOperador(letras, ref cont, ref error) == false)
                    return false;

                if (cont == letras.Length)
                    break;

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

                                    if (cont == letras.Length)
                                        break;

                                    if (ValidarOperador(letras, ref cont, ref error) == false)
                                        return false;
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

                                if (cont == letras.Length)
                                    break;

                                if (ValidarOperador(letras, ref cont, ref error) == false)
                                    return false;
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

                    if (cont == letras.Length)
                        break;

                    if (ValidarOperador(letras, ref cont, ref error) == false)
                        return false;
                }

                if (cont == letras.Length)
                    break;

                // Valida el caso en que no venga espacio entre las comillas
                if (!(letras[cont - 2] != '\'' && letras[cont - 1] == '\'' && letras[cont] == '\'' && letras[cont + 1] != '\''))
                {
                    if (letras[cont] == ' ' || letras[cont] == '|' || letras[cont] == '(' || letras[cont] == ')')
                    {
                        cont++;

                        if (cont == letras.Length)
                            break;

                        if (ValidarOperador(letras, ref cont, ref error) == false)
                            return false; 

                    }
                    else
                    {
                        error = "Caracter no reconocido ( " + letras[cont] + " )";
                        return false;
                    }
                }

                if (cont == letras.Length)
                    break;

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
