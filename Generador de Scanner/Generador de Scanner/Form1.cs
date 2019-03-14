﻿using System;
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
            Stack<Node> Posfijo = new Stack<Node>();
            List<Node> Leafs = new List<Node>();

            foreach (var item in Tokens)
            {
                int cont = 1;
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

                    // First Last
                    if (C1.getNulable())
                        Operador.setFirst(C1.getFirst() + "," + C2.getFirst());
                    else
                        Operador.setFirst(C1.getFirst());

                    if (C2.getNulable())
                        Operador.setLast(C1.getLast() + "," + C2.getLast());
                    else
                        Operador.setLast(C2.getLast());


                    Operador.setC1(C1);
                    Operador.setC2(C2);

                    Posfijo.Push(Operador);
                }
            }

            /*
             * CODIGO PARA CONCATENAR LA EXPRESION REGULAR
             * 
            string ExpresionRegular = "(";
            int tokens = 0;


            foreach (var item in Tokens)
            {
                if (tokens != Tokens.Count - 1)
                {
                    ExpresionRegular += item.getElementos() + ".";
                    tokens++;
                }
                else
                    ExpresionRegular += item.getElementos();
            }

            ExpresionRegular += ").#";
            */




            MessageBox.Show("Todo OKKKK");

        }
    }
}
