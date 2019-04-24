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
