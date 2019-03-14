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
using System.Timers;

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

            OperarArchivo(Sets, Tokens);
        }

        private void OperarArchivo(List<Set> Sets, List<Token> Tokens)
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

                procesos.ObtenerPosfijo(ref Posfijo, ref Leafs, Sets, ER, ref cont, ref leaf);

                if (Posfijo.Count == 2)
                {
                    Node Operador = new Node();
                    Operador.setContenido(Convert.ToString('.'));

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
            procesos.ObtenerPosfijo(ref Posfijo, ref Leafs, Sets, "(#)", ref cont, ref leaf);

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

            MessageBox.Show("Todo OKKKK");

        }

        private void InOrden(Node nodoAuxiliar, ref Dictionary<int, List<int>> Follows)
        {
            if (nodoAuxiliar != null)
            {
                InOrden(nodoAuxiliar.getC1(), ref Follows);

                if (nodoAuxiliar.getContenido() == "." && (nodoAuxiliar.getC1() != null && nodoAuxiliar.getC2() != null))
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
                else if ((nodoAuxiliar.getContenido() == "*" || nodoAuxiliar.getContenido() == "+") && (nodoAuxiliar.getC1() != null && nodoAuxiliar.getC2() != null))
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
    }
}
