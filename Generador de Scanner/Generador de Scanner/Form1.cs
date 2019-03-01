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
            MessageBox.Show("El Archivo seleccionado está en " + PathArchivo);

            StreamReader Lector = new StreamReader(PathArchivo);
            String Linea = Lector.ReadLine();

            while (Linea != null)
            {
                txt.Add(Linea);
                Linea = Lector.ReadLine();
            }

            int linea = 0;

            List<Sets> Sets = new List<Sets>();
            procesos.AnalizarArchivo(txt, ref linea, ref Sets);

            MessageBox.Show("Si llegamos hasta acá todo va bien con los sets");
        }
        
    }
}
