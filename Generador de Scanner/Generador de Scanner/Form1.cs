using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Generador_de_Scanner
{
    public partial class Principal : Form
    {
        // Variable que guarda la direccion del archivo que se cargará
        string PathArchivo = "";
        string error = "";
        List<string> txt = new List<string>();
        Procesos procesos = new Procesos();

        public Principal()
        {
            InitializeComponent();

            // Se configura el Open File Dialog
            openFileDialog1.Title = "Elija el archivo a ordenar";
            openFileDialog1.Filter = "Archivos de texto|*.txt";
            openFileDialog1.FileName = "";
        }

        private void btnAbrirArchivo_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            PathArchivo = openFileDialog1.FileName;
            //MessageBox.Show("El Archivo seleccionado está en " + PathArchivo);

            StreamReader Lector = new StreamReader(PathArchivo);
            String Linea = Lector.ReadLine();

            while (Linea != null)
            {
                txt.Add(Linea);
                Linea = Lector.ReadLine();
            }

            int linea = 0;

            List<Set> Sets = new List<Set>();
            List<Token> Tokens = new List<Token>();

            Lector.Close();

            if (procesos.AnalizarArchivo(txt, ref error, ref linea, ref Sets, ref Tokens) == false)
                MessageBox.Show("ERROR en Linea " + (linea + 1) + "\n" + error, "ERROR");

            List<string> mensajes = new List<string>();
            OperarArchivo(ref mensajes, Sets, Tokens);

            foreach (var item in mensajes)
            {
                listBox1.Items.Add(item);
            }
        }

        private void OperarArchivo(ref List<string> mensajes, List<Set> Sets, List<Token> Tokens)
        {
            
            int leaf = 1;
            int cont = 1;
            Stack<Node> Posfijo = new Stack<Node>();
            List<Node> Leafs = new List<Node>();

            // Se opera cada token (|)
            foreach (var item in Tokens)
            {
                cont = 1;
                string ER = item.getElementos();

                procesos.ObtenerPosfijo(ref mensajes, ref Posfijo, ref Leafs, Sets, ER, ref cont, ref leaf);

                if (Posfijo.Count == 2)
                {
                    Node Operador = new Node();
                    Operador.setContenido(Convert.ToString('|'));

                    Node C2 = Posfijo.Pop();
                    Node C1 = Posfijo.Pop();

                    // Nulable
                    if (C1.getNulable() || C2.getNulable())
                        Operador.setNulable(true);
                    else
                        Operador.setNulable(false);

                    Operador.setFirst(C1.getFirst() + "," + C2.getFirst());
                    Operador.setLast(C1.getLast() + "," + C2.getLast());

                    Operador.setC1(C1);
                    Operador.setC2(C2);

                    Posfijo.Push(Operador);
                }

            }

            cont = 1;
            procesos.ObtenerPosfijo(ref mensajes, ref Posfijo, ref Leafs, Sets, "(#)", ref cont, ref leaf);

            // Concatenacion del resultante con #
            Node FOperador = new Node();
            FOperador.setContenido(Convert.ToString('.'));

            Node FC2 = Posfijo.Pop();
            Node FC1 = Posfijo.Pop();

            // Nulable
            if (FC1.getNulable() || FC2.getNulable())
                FOperador.setNulable(true);
            else
                FOperador.setNulable(false);

            // First Last
            if (FC1.getNulable())
                FOperador.setFirst(FC1.getFirst() + "," + FC2.getFirst());
            else
                FOperador.setFirst(FC1.getFirst());

            if (FC2.getNulable())
                FOperador.setLast(FC1.getLast() + "," + FC2.getLast());
            else
                FOperador.setLast(FC2.getLast());


            FOperador.setC1(FC1);
            FOperador.setC2(FC2);

            // Operador Final
            Posfijo.Push(FOperador);

            // -----------------------------------

            Dictionary<int, List<int>> Follows = new Dictionary<int, List<int>>();

            for (int i = 1; i < leaf; i++)
            {
                List<int> Follow = new List<int>();
                Follows.Add(i, Follow);
            }

            // Busqueda de Follows
            InOrden(Posfijo.Peek(), ref Follows);

            // Se transforma el first en una lista
            List<string> FirstPadre = new List<string>();
            string[] FPadre = FOperador.getFirst().Split(',');

            foreach (var item in FPadre)
            {
                FirstPadre.Add(item);
            }


            Transicion[,] TablaDeTransiciones = TablaTransiciones(FirstPadre, Leafs, Follows);

            MessageBox.Show("Todo OKKKK");

        }

        /// <summary>
        /// Metodo que recorre el Arbol en InOrden y Analiza los Follows
        /// </summary>
        /// <param name="nodoAuxiliar">Nodo Auxiliar</param>
        /// <param name="Follows"> Diccionario con los follows </param>
        private void InOrden(Node nodoAuxiliar, ref Dictionary<int, List<int>> Follows)
        {
            if (nodoAuxiliar != null)
            {
                InOrden(nodoAuxiliar.getC1(), ref Follows);

                string contNodo = nodoAuxiliar.getContenido();
                if (contNodo == "." && (nodoAuxiliar.getC1() != null && nodoAuxiliar.getC2() != null))
                {
                    string[] lasts = nodoAuxiliar.getC1().getLast().Split(',');
                    string[] first = nodoAuxiliar.getC2().getFirst().Split(',');

                    List<int> Lasts = new List<int>();
                    List<int> First = new List<int>();

                    foreach (var item in lasts)
                        Lasts.Add(Convert.ToInt16(item));

                    foreach (var item in first)
                        First.Add(Convert.ToInt16(item));

                    foreach (var itemLast in Lasts)
                    {
                        foreach (var itemFirst in First)
                        {
                            if (!Follows[itemLast].Contains(itemFirst))
                            {
                                Follows[itemLast].Add(itemFirst);
                            }
                        }
                    }

                }
                else if ((nodoAuxiliar.getContenido() == "*" || nodoAuxiliar.getContenido() == "+") && (nodoAuxiliar.getC1() != null))
                {
                    string[] lasts = nodoAuxiliar.getC1().getLast().Split(',');
                    string[] first = nodoAuxiliar.getC1().getFirst().Split(',');

                    List<int> Lasts = new List<int>();
                    List<int> First = new List<int>();

                    foreach (var item in lasts)
                        Lasts.Add(Convert.ToInt16(item));

                    foreach (var item in first)
                        First.Add(Convert.ToInt16(item));

                    foreach (var itemLast in Lasts)
                    {
                        foreach (var itemFirst in First)
                        {
                            if (!Follows[itemLast].Contains(itemFirst))
                            {
                                Follows[itemLast].Add(itemFirst);
                            }
                        }
                    }
                    

                }

                InOrden(nodoAuxiliar.getC2(), ref Follows);
            }
        }


        // DESARROLLO TABLA DE TRANSCICIONES

        /// <summary>
        /// Metodo que construye la matriz de la tabla de trancisiones
        /// </summary>
        /// <param name="FirstPadre">Rl First con el que se comienza a calcular la tabla</param>
        /// <param name="Encabezado">Columnas de la tabla</param>
        /// <param name="Follows">Follows para hacer los calculos respectivos</param>
        /// <returns>La matriz de la tabla de transiciones</returns>
        private Transicion[,] TablaTransiciones(List<string> FirstPadre, List<Node> Leafs, Dictionary<int, List<int>> Follows)
        {
            Columna[] Encabezado = ConstruirColumnas(Leafs);
            int filas = CantFilas(FirstPadre, Encabezado, Follows);

            Transicion[,] TablaDeTransiciones = new Transicion[filas, Encabezado.Length + 1];

            int caracter = 65;
            char letra = Convert.ToChar(caracter);

            // Registra todas los estados en la tabla de tranciciones
            List<Transicion> Transiciones = new List<Transicion>();
            // Registra los estados pendientes por analizar en la tabla de trancisiones
            Queue<Transicion> Pendientes = new Queue<Transicion>();

            // Inicializacion de la matriz
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < (Encabezado.Length + 1); j++)
                {
                    TablaDeTransiciones[i, j] = new Transicion();
                }
            }

            TablaDeTransiciones[0, 0].setElementos(FirstPadre);
            TablaDeTransiciones[0, 0].setIdentificador(Convert.ToString(letra));

            // Punto inicial de la tabla de transiciones
            Transicion temp = new Transicion(Convert.ToString(letra), FirstPadre);
            Transiciones.Add(temp);
            letra++;

            int fila = 0;

            do
            {
                // Por cada Columna (Hoja)
                foreach (var Columna in Encabezado)
                {
                    // Se obteienen los elementos del first acutal
                    foreach (var Elemento in temp.getElementos())
                    {
                        if (Columna.getHojas().Contains(Convert.ToInt16(Elemento)))
                        {
                            List<int> ElementosFollow = Follows[Convert.ToInt16(Elemento)];

                            // Convierte los elementos del follow de int a string
                            List<string> Elementos = new List<string>();
                            foreach (var item in ElementosFollow)
                                Elementos.Add(Convert.ToString(item));

                            int posicion = Columna.getNumColumna();
                            TablaDeTransiciones[fila, posicion].setElementos(Elementos);

                            bool añadir = true;
                            foreach (var item in Transiciones)
                            {
                                if (TablaDeTransiciones[fila, Columna.getNumColumna()].getElementos() == item.getElementos())
                                    añadir = false;
                            }

                            if (añadir)
                            {
                                bool letraExistente = false;
                                string letraAnterior = "";

                                foreach (var item in Transiciones)
                                {
                                    // Se convierten en strings para comarar si son el mismo emento en la tabla
                                    string cadena1 = "";
                                    string cadena2 = "";

                                    foreach (var elemento in item.getElementos())
                                        cadena1 += elemento;

                                    foreach (var elemento in Elementos)
                                        cadena2 += elemento;

                                    if (cadena1.Equals(cadena2))
                                    {
                                        letraExistente = true;
                                        letraAnterior = item.getIdentificador();
                                        break;
                                    }
                                }

                                if (letraExistente)
                                {
                                    TablaDeTransiciones[fila, Columna.getNumColumna()].setIdentificador(letraAnterior);
                                }
                                else
                                {
                                    TablaDeTransiciones[fila, Columna.getNumColumna()].setIdentificador(Convert.ToString(letra));
                                    letra++;
                                    Transiciones.Add(TablaDeTransiciones[fila, Columna.getNumColumna()]);
                                    Pendientes.Enqueue(TablaDeTransiciones[fila, Columna.getNumColumna()]);
                                }
                            }
                        }
                    }
                }

                fila++;
                temp = Pendientes.Dequeue();
                FirstPadre = temp.getElementos();

                TablaDeTransiciones[fila, 0].setIdentificador(temp.getIdentificador());
                TablaDeTransiciones[fila, 0].setElementos(temp.getElementos());

                string identificador = temp.getIdentificador();

            } while (Pendientes.Count != 0);

            // Cual es el simbolo terminal
            int simboloTerminal = Leafs.Count;

            // Impresion en el DataGridView
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < (Encabezado.Length + 1); j++)
                {
                    if (TablaDeTransiciones[i, j].getElementos() == null)
                    {
                        dgv_TablaTrancisiones.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                    }

                    if (i == 0 && j == 0)
                    {
                        dgv_TablaTrancisiones.Rows[i].Cells[j].Style.BackColor = Color.LightSalmon;
                    }

                    if (TablaDeTransiciones[i, 0].getElementos().Contains(Convert.ToString(simboloTerminal)))
                    {
                        dgv_TablaTrancisiones.Rows[i].Cells[0].Style.BackColor = Color.LightSteelBlue;
                    }

                    // Ingresa en el Data Grid View los valores de la matriz
                    if (TablaDeTransiciones[i, j].getElementos() != null)
                    {
                        dgv_TablaTrancisiones.Rows[i].Cells[j].Value = TablaDeTransiciones[i, j].getElementosCadena() + " "
                           + TablaDeTransiciones[i, j].getIdentificador();
                        dgv_TablaTrancisiones.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    
                }
            }

            return TablaDeTransiciones;
        }

        /// <summary>
        /// Metodo que obtiene el encabezado de mi tabla de transiciones
        /// </summary>
        /// <param name="Leafs">Lista de nodos en donde estan todas las hojas</param>
        /// <returns>Cadena de encabezados</returns>
        private Columna[] ConstruirColumnas(List<Node> Leafs)
        {
            List<string> nombres = new List<string>();
            foreach (var item in Leafs)
            {
                if (!nombres.Contains(item.getContenido()) && item.getContenido() != "#")
                    nombres.Add(item.getContenido());
            }

            Columna[] Encabezado = new Columna[nombres.Count];

            for (int i = 0; i < Encabezado.Length; i++)
            {
                Encabezado[i] = new Columna();
                Encabezado[i].setNombre(nombres[i]);
                Encabezado[i].setNumColumna(i + 1);
                List<int> hojas = new List<int>();

                foreach (var item in Leafs)
                {
                    if (item.getContenido() == Encabezado[i].getNombre())
                        hojas.Add(Convert.ToInt16(item.getFirst()));
                }

                Encabezado[i].setHojas(hojas);
            }

            dgv_TablaTrancisiones.Columns.Add(0.ToString(), "");

            for (int i = 0; i < Encabezado.Length; i++)
            {
                dgv_TablaTrancisiones.Columns.Add(i.ToString(), Encabezado[i].getNombre());
                //dgv_TablaTrancisiones.Columns["CustomerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            
            return Encabezado;
        }

        /// <summary>
        /// Metodo que encuentra la cantidad de lineas de la matriz (coordenada i)
        /// </summary>
        /// <param name="FirstPadre">Rl First con el que se comienza a calcular la tabla</param>
        /// <param name="Encabezado">Columnas de la tabla</param>
        /// <param name="Follows">Follows para hacer los calculos respectivos</param>
        /// <returns>Cantidad de lineas de la matriz</returns>
        private int CantFilas(List<string> FirstPadre, Columna[] Encabezado, Dictionary<int, List<int>> Follows)
        {
            int cant = 1;
            int caracter = 65;
            char letra = Convert.ToChar(caracter);

            List<Transicion> Transiciones = new List<Transicion>();
            Queue<Transicion> Pendientes = new Queue<Transicion>();

            Transicion[] Linea = new Transicion[Encabezado.Length + 1];

            // Inicializacion de la lista
            for (int i = 0; i < Linea.Length; i++)
                Linea[i] = new Transicion();

            Linea[0].setElementos(FirstPadre);
            Linea[0].setIdentificador(Convert.ToString(letra));

            // Punto inicial de la tabla de transiciones
            Transicion temp = new Transicion(Convert.ToString(letra), FirstPadre);
            Transiciones.Add(temp);
            letra++;

            do
            {
                // Por cada Columna (Hoja)
                foreach (var Columna in Encabezado)
                {
                    // Se obteienen los elementos del first acutal
                    foreach (var Elemento in temp.getElementos())
                    {
                        if (Columna.getHojas().Contains(Convert.ToInt16(Elemento)))
                        {
                            List<int> ElementosFollow = Follows[Convert.ToInt16(Elemento)];

                            // Convierte los elementos del follow de int a string
                            List<string> Elementos = new List<string>();
                            foreach (var item in ElementosFollow)
                                Elementos.Add(Convert.ToString(item));

                            int posicion = Columna.getNumColumna();
                            Linea[posicion].setElementos(Elementos);

                            bool añadir = true;
                            foreach (var item in Transiciones)
                            {
                                if (Linea[Columna.getNumColumna()].getElementos() == item.getElementos())
                                    añadir = false;
                            }

                            if (añadir)
                            {
                                bool letraExistente = false;
                                string letraAnterior = "";

                                foreach (var item in Transiciones)
                                {
                                    // Se convierten en strings para comarar si son el mismo emento en la tabla
                                    string cadena1 = "";
                                    string cadena2 = "";

                                    foreach (var elemento in item.getElementos())
                                        cadena1 += elemento;

                                    foreach (var elemento in Elementos)
                                        cadena2 += elemento;

                                    if (cadena1.Equals(cadena2))
                                    {
                                        letraExistente = true;
                                        letraAnterior = item.getIdentificador();
                                        break;
                                    }
                                }

                                if (letraExistente)
                                {
                                    Linea[Columna.getNumColumna()].setIdentificador(letraAnterior);
                                }
                                else
                                {
                                    Linea[Columna.getNumColumna()].setIdentificador(Convert.ToString(letra));
                                    letra++;
                                    Transiciones.Add(Linea[Columna.getNumColumna()]);
                                    Pendientes.Enqueue(Linea[Columna.getNumColumna()]);
                                }
                            }
                        }
                    }
                }

                cant++;
                temp = Pendientes.Dequeue();
                FirstPadre = temp.getElementos();
                string identificador = temp.getIdentificador();

                Linea = new Transicion[Encabezado.Length + 1];

                // Inicializacion de la lista
                for (int i = 0; i < Linea.Length; i++)
                    Linea[i] = new Transicion();

                Linea[0].setElementos(FirstPadre);
                Linea[0].setIdentificador(identificador);

            } while (Pendientes.Count != 0);

            dgv_TablaTrancisiones.Rows.Add(cant - 1);

            return cant;
        }
    }
}
