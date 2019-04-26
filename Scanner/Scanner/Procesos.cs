using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
    class Procesos
    {


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

                    Operador.setExpresionAcumulada(C1.getExpresionAcumulada() + Operador.getContenido());

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

                    Operador.setExpresionAcumulada(C1.getExpresionAcumulada() + Operador.getContenido() + C2.getExpresionAcumulada());

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

            if (letras[cont - 1] == '(')
                abierto = true;

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

                    abierto = false;
                }
                else
                {
                    palabras.Add(palabra);

                    if (palabra == "#")
                    {
                        analizar = false;
                        Node temp = new Node(palabra, palabra, false, Convert.ToString(leaf), Convert.ToString(leaf));

                        Posfijo.Push(temp);
                        Leafs.Add(temp);
                        leaf++;
                        palabras.RemoveAt(0);
                    }
                    else

                    // Si encontró la palabra siendo un lenguaje o un token detiene el ciclo
                    if (EncontrarLenguajes(Sets, palabras, ref error) || EncontrarPalabras(Sets, palabras, ref error))
                    {
                        analizar = false;
                        Node temp = new Node(palabra, palabra, false, Convert.ToString(leaf), Convert.ToString(leaf));

                        Posfijo.Push(temp);
                        Leafs.Add(temp);
                        leaf++;
                        palabras.RemoveAt(0);
                    }
                }
            }
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

        public List<string> VerificarTokenDirecto(string Token)
        {
            List<string> TokenDirecto = new List<string>();
            bool directo = true;

            string TokenSinEspacios = FiltrarEspacio(Token);
            char[] temp = TokenSinEspacios.ToCharArray();

            for (int i = 1; i < temp.Length - 1; i++)
            {
                if ((temp[i] == '*' || temp[i] == '+' || temp[i] == '?') && (temp[i + 1] == '.' || temp[i + 1] == ')' || temp[i - 1] == ')'))
                    directo = false;
            }

            if ((temp[temp.Length - 1] == '*' || temp[temp.Length - 1] == '+' || temp[temp.Length - 1] == '?') && temp[temp.Length - 2] == ')')
                directo = false;

            if (directo)
            {
                bool abierto = false;

                for (int i = 0; i < temp.Length; i++)
                {
                    if ((temp[i] == '\'' || temp[i] == '(') && abierto == false)
                        abierto = true;

                    if ((temp[i] == '\'' || temp[i] == ')') && abierto == true)
                        abierto = false;
                    else if (abierto == true && (temp[i] != '\'' && temp[i] != '(' && temp[i] != '.'))
                        TokenDirecto.Add(Convert.ToString(temp[i]));
                }
            }
            else
            {
                TokenDirecto.Clear();
            }

            return TokenDirecto;
        }

        public Queue<string> ConvertToSets(List<Set> Sets, string entrada)
        {
            Queue<string> Salida = new Queue<string>();

            int cont = 0;
            char[] caracter = entrada.ToCharArray();

            
            while (cont < caracter.Length)
            {
                bool encontrado = false;
                string segmento = Convert.ToString(caracter[cont]);

                foreach (var item in Sets)
                {
                    if (item.getElementos().Contains(segmento))
                    {
                        encontrado = true;
                        Salida.Enqueue(item.getNombre());
                        cont++;
                        break;
                    }
                }

                // Si el segmento analizado no se encontró en ningun Set se retorna una cola vacia, que significa ERROR
                if (encontrado == false)
                {
                    Salida.Clear();
                    break;
                }
            }

            return Salida;
        }

        public bool AnalizarEntrada(Columna[] Encabezado, Transicion[,] TablaDeTransiciones, Queue<string> Expresion, int simboloTerminal)
        {
            int columna = -1;
            string FilaActual = "A";
            int numFilaActual = -1;

            //>>
            while (Expresion.Count() != 0)
            {
                string Elemento = Expresion.Dequeue();

                for (int i = 0; i < Encabezado.Length; i++)
                {
                    if (Elemento.Equals(Encabezado[i].getNombre()))
                    {
                        columna = i + 1;
                        break;
                    }
                }

                // Porque no encontró la columna
                if (columna == -1)
                    return false;

                //  
                int cont = 0;
                while (TablaDeTransiciones[cont, 0] != null)
                {
                    if (TablaDeTransiciones[cont, 0].getIdentificador().Equals(FilaActual))
                    {
                        numFilaActual = cont;
                        break;
                    }
                    else
                    {
                        cont++;
                    }
                }

                if (TablaDeTransiciones[numFilaActual, columna].getElementos() != null)
                {
                    FilaActual = TablaDeTransiciones[numFilaActual, columna].getIdentificador();

                    if (Expresion.Count() == 0)
                    {
                        if (TablaDeTransiciones[numFilaActual, columna].getElementos().Contains(Convert.ToString(simboloTerminal)))
                            return true;
                        else
                            return false;
                    }
                }
                else
                {
                    return false;
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
                    if (lenguaje == "α" || lenguaje == "β" || lenguaje == "ɣ" || lenguaje == "δ" || lenguaje == "ε" || lenguaje == "ϑ")
                    {
                        return true;
                    }
                    else
                    {
                        error = "No se ha encontrado \"" + lenguaje + "\" definido en los SETS";
                        return false;
                    }
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
                    if (palabra == "α" || palabra == "β" || palabra == "ɣ" || palabra == "δ" || palabra == "ε" || palabra == "ϑ")
                    {
                        return true;
                    }
                    else
                    {
                        error = "No se ha encontrado \"" + palabra + "\" definido en los SETS";
                        return false;
                    }

                }
            }

            return true;
        }

    }
}
