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

            int leaf = 1;
            int cont = 1;
            Stack<Node> Posfijo = new Stack<Node>();
            List<Node> Leafs = new List<Node>();

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


            MessageBox.Show("Todo OKKKK");

        }
    }
}
