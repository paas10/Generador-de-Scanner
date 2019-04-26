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
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Generador_de_Scanner
{
    public partial class Principal : Form
    {
        // Variable que guarda la direccion del archivo que se cargará
        string PathArchivo = "";
        string error = "";
        List<string> txt = new List<string>();
        Procesos procesos = new Procesos();

        int iError = 0;
        List<Set> Sets;
        List<Token> Tokens;
        Dictionary<string, int> Actions;

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
            //PathArchivo = "";
            //txt = new List<string>();

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

            Sets = new List<Set>();
            Tokens = new List<Token>();
            Actions = new Dictionary<string, int>();

            Lector.Close();

            if (procesos.AnalizarArchivo(txt, ref error, ref linea, ref Sets, ref Tokens, ref Actions, ref iError) == false)
            {
                MessageBox.Show("ERROR en Linea " + (linea + 1) + "\n" + error, "ERROR");
                return;
            }

            // Lineas con Nullable, Firsts y Lasts
            List<string> lineasNFL = new List<string>();
            List<string> follows = new List<string>();

            // Lineas Nullable Firsts Lasts
            OperarArchivo(Sets, Tokens, ref follows, ref lineasNFL);

            // Impresion de Firsts y Lasts

            listBox1.Items.Add("Nullable - Firsts & Lasts");

            foreach (var item in lineasNFL)
            {
                listBox1.Items.Add(item);
            }

            // Impresion de Follows

            listBox1.Items.Add("");
            listBox1.Items.Add("FOLLOWS");

            foreach (var item in follows)
            {
                listBox1.Items.Add(item);
            }

            EscribirScanner();

        }


        // ESCRITURA DEL ARCHIVO .CS "SCANNER"

        private void EscribirScanner()
        {
            // "C:\\Users\\Admin\\Documents\\GitHub\\Generador-de-Scanner\\Scanner\\Scanner\\Program.cs"
            StreamWriter sw = new StreamWriter("C:\\Users\\Admin\\Desktop\\Program.cs");

            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using System.IO;");
            sw.WriteLine("using System.Linq;");
            sw.WriteLine("using System.Text;");
            sw.WriteLine("using System.Threading.Tasks;");
            sw.WriteLine("");
            sw.WriteLine("namespace Scanner");
            sw.WriteLine("{");
            sw.WriteLine("\tclass Program");
            sw.WriteLine("\t{");
            sw.WriteLine("\t\tpublic static int simboloTerminal = 0;");
            sw.WriteLine("\t\tpublic static Procesos procesos = new Procesos();");
            sw.WriteLine("\t\t");
            sw.WriteLine("\t\tstatic void Main(string[] args)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\t");

            EscribirDeclaracionVariables(ref sw);
            EscribirLecturaArchivo(ref sw);
            EscribirClasificador(ref sw);

            sw.WriteLine("\t\t\tConsole.ReadKey();");
            sw.WriteLine("\t\t}");

            EscribirGenerarAutomataToken(ref sw);

            sw.WriteLine("\t}");
            sw.WriteLine("\t");

            EscribirClases(ref sw);

            sw.WriteLine("");
            sw.WriteLine("}");
            sw.WriteLine("");

            sw.Close();

            var programPath = "C:\\Users\\Admin\\Desktop\\Scanner.exe"; //RUTA Y NOMBRE DE DONDE SE GENERARÁ EL .EXE"
            var csc = new CSharpCodeProvider();
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.Data.dll", "System.dll", "System.Collections.dll"}, programPath, true); parameters.GenerateExecutable = true;
            parameters.GenerateExecutable = true;

            //RUTA DEL CODIGO QUE SE VA A COMPILAR"
            var code = File.ReadAllText("C:\\Users\\Admin\\Desktop\\Program.cs");
            CompilerResults result = csc.CompileAssemblyFromSource(parameters, code);

            FolderBrowserDialog folder = new FolderBrowserDialog();
            System.Diagnostics.Process.Start(@"C:\\Users\\Admin\\Desktop\\Scanner.exe");

        }

        private void EscribirDeclaracionVariables(ref StreamWriter sw)
        {
            sw.WriteLine("\t\t\tint error = " + iError + ";");
            sw.WriteLine("\t\t");

            // Declaración de la lista de Sets

            sw.WriteLine("\t\t\tList<Set> Sets = new List<Set>();");
            sw.WriteLine("\t\t\tSet SetTemp;");
            sw.WriteLine("\t\t\tList<string> elementos;");

            foreach (var set in Sets)
            {
                sw.WriteLine("\t\t\t");
                sw.WriteLine("\t\t\tSetTemp = new Set(\"" + set.getNombre() + "\");");

                sw.WriteLine("\t\t\telementos = new List<string>();");
                sw.WriteLine("\t\t\t");

                foreach (var elemento in set.getElementos())
                {
                    char[] letra = elemento.ToCharArray();

                    if (letra.Length == 1)
                        sw.WriteLine("\t\t\telementos.Add(Convert.ToString(Convert.ToChar(" + Convert.ToInt16(letra[0]) + ")));");
                    else
                        sw.WriteLine("\t\t\telementos.Add(\"" + elemento + "\");");
                }

                sw.WriteLine("\t\t\tSetTemp.setElementos(elementos);");
                sw.WriteLine("\t\t\tSets.Add(SetTemp);");

                sw.WriteLine("\t\t\tSetTemp = new Set();");
                sw.WriteLine("\t\t\telementos = new List<String>();");

                sw.WriteLine("\t\t\t");
            }

            sw.WriteLine("\t\t\t");

            // Declaración de un diccionario para los Tokens
            sw.WriteLine("\t\t\tDictionary<string, int> Tokens = new Dictionary<string, int>();");
            sw.WriteLine("\t\t\t");
            foreach (var token in Tokens)
            {
                string elementos = "";
                char[] caracteres = token.getElementos().ToCharArray();

                foreach (var item in caracteres)
                {
                    if (item == '"')
                        elementos += Convert.ToString(Convert.ToChar(92)) + "\"";
                    else
                        elementos += item;
                }

                sw.WriteLine("\t\t\tTokens.Add(\"" + elementos + "\", " + token.getNumeroToken() + ");");
            }
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t");

            // Declaración de un diccionario para los Actions
            sw.WriteLine("\t\t\tDictionary<string, int> Actions = new Dictionary<string, int>();");
            sw.WriteLine("\t\t\t");
            foreach (var action in Actions)
            {
                sw.WriteLine("\t\t\tActions.Add(\"" + action.Key + "\", " + action.Value + ");");
            }
            sw.WriteLine("\t\t\t");

        }

        private void EscribirLecturaArchivo(ref StreamWriter sw)
        {
            openFileDialog1.Title = "Archivo con Entradas <<Segunda Fase>>";
            openFileDialog1.Filter = "Archivos de texto|*.txt";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            PathArchivo = openFileDialog1.FileName;

            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tstring line;");
            sw.WriteLine("\t\t\t");
            // \"C:\\\\Users\\\\Admin\\\\Desktop\\\\Prueba.txt\"

            char[] Path = PathArchivo.ToCharArray();
            PathArchivo = "";

            foreach (var item in Path)
            {
                if (item == Convert.ToChar(92))
                    PathArchivo += (Convert.ToString(Convert.ToChar(92)) + Convert.ToString(Convert.ToChar(92)));
                else
                    PathArchivo += Convert.ToString(item);
            }

            sw.WriteLine("\t\t\tStreamReader sr = new StreamReader(\"" + PathArchivo + "\");");
            sw.WriteLine("\t\t\tline = sr.ReadLine();");
            sw.WriteLine("\t\t\t");
        }

        private void EscribirClasificador(ref StreamWriter sw)
        {
            sw.WriteLine("\t\t\tstring[] fragmentos = line.Split(' ');");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Se separa la entrada por espacios y se analiza cada fragmento individualmente.");
            sw.WriteLine("\t\t\tforeach (var fragmento in fragmentos)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tint TokenInmediato = -1;");
            sw.WriteLine("\t\t\t\tint TokenLargo = -1;");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\t// Analiza si la entrada forma parte de las Actions");
            sw.WriteLine("\t\t\t\tif (Actions.ContainsKey(fragmento))");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tConsole.WriteLine(fragmento + \"\t\t\" + Actions[fragmento]);");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\telse");
            sw.WriteLine("\t\t\t\t{");

            sw.WriteLine("\t\t\t\t\t// Analiza token por token");
            sw.WriteLine("\t\t\t\t\tforeach (var item in Tokens)");
            sw.WriteLine("\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\tList<String> TokenDirecto = procesos.VerificarTokenDirecto(item.Key);");
            sw.WriteLine("\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\tif (TokenDirecto.Count() != 0)");
            sw.WriteLine("\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\tchar[] caracteres = fragmento.ToCharArray();");
            sw.WriteLine("\t\t\t\t\t\t\tbool coincide = true;");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\ttry");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tfor (int i = 0; i < caracteres.Length; i++)");
            sw.WriteLine("\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tif (!Convert.ToString(caracteres[i]).Equals(TokenDirecto[i]))");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tcoincide = false;");
            sw.WriteLine("\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\tcatch");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tcoincide = false;");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tif (coincide)");
            sw.WriteLine("\t\t\t\t\t\t\t\tTokenInmediato = item.Value;");
            sw.WriteLine("\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\telse");
            sw.WriteLine("\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\tQueue<string> Expresion = procesos.ConvertToSets(Sets, fragmento);");
            sw.WriteLine("\t\t\t\t\t\t\tColumna[] Encabezado = new Columna[100];");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tTransicion[,] TablaDeTransiciones = OperarArchivo(Sets, item.Key, ref Encabezado);");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tif (procesos.AnalizarEntrada(Encabezado, TablaDeTransiciones, Expresion, simboloTerminal))");
            sw.WriteLine("\t\t\t\t\t\t\t\tTokenLargo = item.Value;");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tif (TokenInmediato != -1)");
            sw.WriteLine("\t\t\t\t\t\tConsole.WriteLine(fragmento + \"\t\t\" + TokenInmediato);");
            sw.WriteLine("\t\t\t\t\telse if (TokenLargo != -1)");
            sw.WriteLine("\t\t\t\t\t\tConsole.WriteLine(fragmento + \"\t\t\" + TokenLargo);");
            sw.WriteLine("\t\t\t\t\telse");
            sw.WriteLine("\t\t\t\t\t\tConsole.WriteLine(\"ERROR\t\t\" + error);");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");

        }

        private void EscribirGenerarAutomataToken(ref StreamWriter sw)
        {
            sw.WriteLine("\t\t");
            sw.WriteLine("\t\tstatic private Transicion[,] OperarArchivo(List<Set> Sets, string Token, ref Columna[] Encabezado)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tint leaf = 1;");
            sw.WriteLine("\t\t\tint cont = 1;");
            sw.WriteLine("\t\t\tStack<Node> Posfijo = new Stack<Node>();");
            sw.WriteLine("\t\t\tList<Node> Leafs = new List<Node>();");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Se opera el token");
            sw.WriteLine("\t\t\tstring ER = Token;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tProcesos procesos = new Procesos();");
            sw.WriteLine("\t\t\tprocesos.ObtenerPosfijo(ref Posfijo, ref Leafs, Sets, ER, ref cont, ref leaf);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tif (Posfijo.Count == 2)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tNode Operador = new Node();");
            sw.WriteLine("\t\t\t\tOperador.setContenido(Convert.ToString('|'));");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tNode C2 = Posfijo.Pop();");
            sw.WriteLine("\t\t\t\tNode C1 = Posfijo.Pop();");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\t// Nulable");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tif (C1.getNulable() || C2.getNulable())");
            sw.WriteLine("\t\t\t\t\tOperador.setNulable(true);");
            sw.WriteLine("\t\t\t\telse");
            sw.WriteLine("\t\t\t\t\tOperador.setNulable(false);");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tOperador.setFirst(C1.getFirst() + \", \" + C2.getFirst());");
            sw.WriteLine("\t\t\t\tOperador.setLast(C1.getLast() + \", \" + C2.getLast());");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tOperador.setC1(C1);");
            sw.WriteLine("\t\t\t\tOperador.setC2(C2);");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tOperador.setExpresionAcumulada(C1.getExpresionAcumulada() + Operador.getContenido() + C2.getExpresionAcumulada());");
            sw.WriteLine("\t\t\t\tPosfijo.Push(Operador);");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tcont = 1;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tprocesos.ObtenerPosfijo(ref Posfijo, ref Leafs, Sets, \"(#)\", ref cont, ref leaf);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Concatenacion del resultante con #");
            sw.WriteLine("\t\t\tNode FOperador = new Node();");
            sw.WriteLine("\t\t\tFOperador.setContenido(Convert.ToString('.'));");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tNode FC2 = Posfijo.Pop();");
            sw.WriteLine("\t\t\tNode FC1 = Posfijo.Pop();");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Nulable");
            sw.WriteLine("\t\t\tif (FC1.getNulable() || FC2.getNulable())");
            sw.WriteLine("\t\t\t\tFOperador.setNulable(true);");
            sw.WriteLine("\t\t\telse");
            sw.WriteLine("\t\t\t\tFOperador.setNulable(false);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// First Last");
            sw.WriteLine("\t\t\tif (FC1.getNulable())");
            sw.WriteLine("\t\t\t\tFOperador.setFirst(FC1.getFirst() + \", \" + FC2.getFirst());");
            sw.WriteLine("\t\t\telse");
            sw.WriteLine("\t\t\t\tFOperador.setFirst(FC1.getFirst());");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tif (FC2.getNulable())");
            sw.WriteLine("\t\t\t\tFOperador.setLast(FC1.getLast() + \", \" + FC2.getLast());");
            sw.WriteLine("\t\t\telse");
            sw.WriteLine("\t\t\t\tFOperador.setLast(FC2.getLast());");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tFOperador.setC1(FC1);");
            sw.WriteLine("\t\t\tFOperador.setC2(FC2);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tFOperador.setExpresionAcumulada(FC1.getExpresionAcumulada() + FOperador.getContenido() + FC2.getExpresionAcumulada());");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Operador Final");
            sw.WriteLine("\t\t\tPosfijo.Push(FOperador);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// -----------------------------------");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tDictionary<int, List<int>> Follows = new Dictionary<int, List<int>>();");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tfor (int i = 1; i < leaf; i++)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tList<int> Follow = new List<int>();");
            sw.WriteLine("\t\t\t\tFollows.Add(i, Follow);");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Busqueda de Follows");
            sw.WriteLine("\t\t\tInOrden(Posfijo.Peek(), ref Follows);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tforeach (var item in Follows)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tstring elemento = \"\";");
            sw.WriteLine("\t\t\t\tforeach (var elementos in item.Value)");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\telemento += elementos + \", \";");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Se transforma el first en una lista");
            sw.WriteLine("\t\t\tList<string> FirstPadre = new List<string>();");
            sw.WriteLine("\t\t\tstring[] FPadre = FOperador.getFirst().Split(',');");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tforeach (var item in FPadre)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tFirstPadre.Add(item);");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tTransicion[,] TablaDeTransiciones = TablaTransiciones(FirstPadre, Leafs, Follows, ref Encabezado);");
            sw.WriteLine("\t\t\treturn TablaDeTransiciones;");
            sw.WriteLine("\t\t}");
            sw.WriteLine("\t\t");
            sw.WriteLine("\t\tstatic private void InOrden(Node nodoAuxiliar, ref Dictionary<int, List<int>> Follows)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tif (nodoAuxiliar != null)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tInOrden(nodoAuxiliar.getC1(), ref Follows);");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tstring contNodo = nodoAuxiliar.getContenido();");
            sw.WriteLine("\t\t\t\tif (contNodo == \".\" && (nodoAuxiliar.getC1() != null && nodoAuxiliar.getC2() != null))");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tstring[] lasts = nodoAuxiliar.getC1().getLast().Split(',');");
            sw.WriteLine("\t\t\t\t\tstring[] first = nodoAuxiliar.getC2().getFirst().Split(',');");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tList<int> Lasts = new List<int>();");
            sw.WriteLine("\t\t\t\t\tList<int> First = new List<int>();");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tforeach (var item in lasts)");
            sw.WriteLine("\t\t\t\t\t\tLasts.Add(Convert.ToInt16(item));");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tforeach (var item in first)");
            sw.WriteLine("\t\t\t\t\t\tFirst.Add(Convert.ToInt16(item));");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tforeach (var itemLast in Lasts)");
            sw.WriteLine("\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\tforeach (var itemFirst in First)");
            sw.WriteLine("\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\tif (!Follows[itemLast].Contains(itemFirst))");
            sw.WriteLine("\t\t\t\t\t\t\t\tFollows[itemLast].Add(itemFirst);");
            sw.WriteLine("\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\telse if ((nodoAuxiliar.getContenido() == \"*\" || nodoAuxiliar.getContenido() == \"+\") && (nodoAuxiliar.getC1() != null))");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tstring[] lasts = nodoAuxiliar.getC1().getLast().Split(',');");
            sw.WriteLine("\t\t\t\t\tstring[] first = nodoAuxiliar.getC1().getFirst().Split(',');");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tList<int> Lasts = new List<int>();");
            sw.WriteLine("\t\t\t\t\tList<int> First = new List<int>();");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tforeach (var item in lasts)");
            sw.WriteLine("\t\t\t\t\t\tLasts.Add(Convert.ToInt16(item));");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tforeach (var item in first)");
            sw.WriteLine("\t\t\t\t\t\tFirst.Add(Convert.ToInt16(item));");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tforeach (var itemLast in Lasts)");
            sw.WriteLine("\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\tforeach (var itemFirst in First)");
            sw.WriteLine("\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\tif (!Follows[itemLast].Contains(itemFirst))");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tFollows[itemLast].Add(itemFirst);");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\tInOrden(nodoAuxiliar.getC2(), ref Follows);");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\tstatic private Transicion[,] TablaTransiciones(List<string> FirstPadre, List<Node> Leafs, Dictionary<int, List<int>> Follows, ref Columna[] Encabezado)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tEncabezado = ConstruirColumnas(Leafs);");
            sw.WriteLine("\t\t\tint filas = CantFilas(FirstPadre, Encabezado, Follows);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tTransicion[,] TablaDeTransiciones = new Transicion[filas, Encabezado.Length + 1];");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tint caracter = 65;");
            sw.WriteLine("\t\t\tchar letra = Convert.ToChar(caracter);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Registra todas los estados en la tabla de tranciciones");
            sw.WriteLine("\t\t\tList<Transicion> Transiciones = new List<Transicion>();");
            sw.WriteLine("\t\t\t// Registra los estados pendientes por analizar en la tabla de trancisiones");
            sw.WriteLine("\t\t\tQueue<Transicion> Pendientes = new Queue<Transicion>();");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Inicializacion de la matriz");
            sw.WriteLine("\t\t\tfor (int i = 0; i < filas; i++)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tfor (int j = 0; j < (Encabezado.Length + 1); j++)");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tTablaDeTransiciones[i, j] = new Transicion();");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tTablaDeTransiciones[0, 0].setElementos(FirstPadre);");
            sw.WriteLine("\t\t\tTablaDeTransiciones[0, 0].setIdentificador(Convert.ToString(letra));");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Punto inicial de la tabla de transiciones");
            sw.WriteLine("\t\t\tTransicion temp = new Transicion(Convert.ToString(letra), FirstPadre);");
            sw.WriteLine("\t\t\tTransiciones.Add(temp);");
            sw.WriteLine("\t\t\tletra++;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tint fila = 0;");
            sw.WriteLine("\t\t\tbool analizar = true;");
            sw.WriteLine("\t\t\tbool UltimaIteracion = false;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tdo");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\t// Gatillo para ya no seguir analizando (Solo la ultima iteracion <<Actual>>)");
            sw.WriteLine("\t\t\t\tif (UltimaIteracion == true)");
            sw.WriteLine("\t\t\t\t\tanalizar = false;");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\t// Por cada Columna (Hoja)");
            sw.WriteLine("\t\t\t\tforeach (var Columna in Encabezado)");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t// Se obteienen los elementos del first acutal");
            sw.WriteLine("\t\t\t\t\tforeach (var Elemento in temp.getElementos())");
            sw.WriteLine("\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\tif (Columna.getHojas().Contains(Convert.ToInt16(Elemento)))");
            sw.WriteLine("\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\tList<int> ElementosFollow = Follows[Convert.ToInt16(Elemento)];");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t// Convierte los elementos del follow de int a string");
            sw.WriteLine("\t\t\t\t\t\t\tList<string> Elementos = new List<string>();");
            sw.WriteLine("\t\t\t\t\t\t\tforeach (var item in ElementosFollow)");
            sw.WriteLine("\t\t\t\t\t\t\t\tElementos.Add(Convert.ToString(item));");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tint posicion = Columna.getNumColumna();");
            sw.WriteLine("\t\t\t\t\t\t\tTablaDeTransiciones[fila, posicion].setElementos(Elementos);");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tbool añadir = true;");
            sw.WriteLine("\t\t\t\t\t\t\tforeach (var item in Transiciones)");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tif (TablaDeTransiciones[fila, Columna.getNumColumna()].getElementos() == item.getElementos())");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tañadir = false;");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tif (añadir)");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tbool letraExistente = false;");
            sw.WriteLine("\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\tstring letraAnterior = \"\";");
            sw.WriteLine("\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\tforeach (var item in Transiciones)");
            sw.WriteLine("\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t// Se convierten en strings para comarar si son el mismo emento en la tabla");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tstring cadena1 = \"\";");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tstring cadena2 = \"\";");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tforeach (var elemento in item.getElementos())");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tcadena1 += elemento;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tforeach (var elemento in Elementos)");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tcadena2 += elemento;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tif (cadena1.Equals(cadena2))");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tletraExistente = true;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tletraAnterior = item.getIdentificador();");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tbreak;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\tif (letraExistente)");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tTablaDeTransiciones[fila, Columna.getNumColumna()].setIdentificador(letraAnterior);");
            sw.WriteLine("\t\t\t\t\t\t\t\telse");
            sw.WriteLine("\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tTablaDeTransiciones[fila, Columna.getNumColumna()].setIdentificador(Convert.ToString(letra));");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tletra++;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tTransiciones.Add(TablaDeTransiciones[fila, Columna.getNumColumna()]);");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tPendientes.Enqueue(TablaDeTransiciones[fila, Columna.getNumColumna()]);");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tif (Pendientes.Count != 0 && analizar == false)");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tanalizar = true;");
            sw.WriteLine("\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\ttry");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tfila++;");
            sw.WriteLine("\t\t\t\t\ttemp = Pendientes.Dequeue();");
            sw.WriteLine("\t\t\t\t\tFirstPadre = temp.getElementos();");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tTablaDeTransiciones[fila, 0].setIdentificador(temp.getIdentificador());");
            sw.WriteLine("\t\t\t\t\tTablaDeTransiciones[fila, 0].setElementos(temp.getElementos());");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tstring identificador = temp.getIdentificador();");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\tcatch (Exception e) { }");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Cuando ya no hayan elementos en la cola se activa el trigger UltimaIteracion");
            sw.WriteLine("\t\t\tif (Pendientes.Count == 0)");
            sw.WriteLine("\t\t\tUltimaIteracion = true;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t} while (analizar);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Cual es el simbolo terminal");
            sw.WriteLine("\t\t\tsimboloTerminal = Leafs.Count;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\treturn TablaDeTransiciones;");
            sw.WriteLine("\t\t}");
            sw.WriteLine("\t\t");
            sw.WriteLine("\t\tstatic private Columna[] ConstruirColumnas(List<Node> Leafs)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tList<string> nombres = new List<string>();");
            sw.WriteLine("\t\t\tforeach (var item in Leafs)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tif (!nombres.Contains(item.getContenido()) && item.getContenido() != \"#\")");
            sw.WriteLine("\t\t\t\t\tnombres.Add(item.getContenido());");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tColumna[] Encabezado = new Columna[nombres.Count];");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tfor (int i = 0; i < Encabezado.Length; i++)");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tEncabezado[i] = new Columna();");

            sw.WriteLine("\t\t\t\tif (nombres[i] == \"α\")");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(\"(\");");
            sw.WriteLine("\t\t\t\telse if (nombres[i] == \"β\")");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(\")\");");
            sw.WriteLine("\t\t\t\telse if (nombres[i] == \"ɣ\")");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(\".\");");
            sw.WriteLine("\t\t\t\telse if (nombres[i] == \"δ\")");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(\"*\");");
            sw.WriteLine("\t\t\t\telse if (nombres[i] == \"ε\")");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(\"+\");");
            sw.WriteLine("\t\t\t\telse if (nombres[i] == \"ϑ\")");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(\"?\");");
            sw.WriteLine("\t\t\t\telse");
            sw.WriteLine("\t\t\t\t\tEncabezado[i].setNombre(nombres[i]);");

            sw.WriteLine("\t\t\t\tEncabezado[i].setNombre(nombres[i]);");

            sw.WriteLine("\t\t\t\tEncabezado[i].setNumColumna(i + 1);");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tList<int> hojas = new List<int>();");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tforeach (var item in Leafs)");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tif (item.getContenido() == Encabezado[i].getNombre())");
            sw.WriteLine("\t\t\t\t\t\thojas.Add(Convert.ToInt16(item.getFirst()));");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\tEncabezado[i].setHojas(hojas);");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t//for (int i = 0; i < Encabezado.Length; i++)");
            sw.WriteLine("\t\t\t\t//dgv_TablaTrancisiones.Columns.Add(i.ToString(), Encabezado[i].getNombre());");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\treturn Encabezado;");
            sw.WriteLine("\t\t}");
            sw.WriteLine("\t\t");
            sw.WriteLine("\t\t");
            sw.WriteLine("\t\tstatic private int CantFilas(List<string> FirstPadre, Columna[] Encabezado, Dictionary<int, List<int>> Follows)");
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tint cant = 1;");
            sw.WriteLine("\t\t\tint caracter = 65;");
            sw.WriteLine("\t\t\tchar letra = Convert.ToChar(caracter);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tList<Transicion> Transiciones = new List<Transicion>();");
            sw.WriteLine("\t\t\tQueue<Transicion> Pendientes = new Queue<Transicion>();");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tTransicion[] Linea = new Transicion[Encabezado.Length + 1];");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Inicializacion de la lista");
            sw.WriteLine("\t\t\tfor (int i = 0; i < Linea.Length; i++)");
            sw.WriteLine("\t\t\t\tLinea[i] = new Transicion();");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tLinea[0].setElementos(FirstPadre);");
            sw.WriteLine("\t\t\tLinea[0].setIdentificador(Convert.ToString(letra));");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\t// Punto inicial de la tabla de transiciones");
            sw.WriteLine("\t\t\tTransicion temp = new Transicion(Convert.ToString(letra), FirstPadre);");
            sw.WriteLine("\t\t\tTransiciones.Add(temp);");
            sw.WriteLine("\t\t\tletra++;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tbool analizar = true;");
            sw.WriteLine("\t\t\tbool UltimaIteracion = false;");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\tdo");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\t// Gatillo para ya no seguir analizando (Solo la ultima iteracion <<Actual>>)");
            sw.WriteLine("\t\t\t\tif (UltimaIteracion == true)");
            sw.WriteLine("\t\t\t\t\tanalizar = false;");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\t// Por cada Columna (Hoja)");
            sw.WriteLine("\t\t\t\tforeach (var Columna in Encabezado)");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t// Se obteienen los elementos del first acutal");
            sw.WriteLine("\t\t\t\t\tforeach (var Elemento in temp.getElementos())");
            sw.WriteLine("\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\tif (Columna.getHojas().Contains(Convert.ToInt16(Elemento)))");
            sw.WriteLine("\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\tList<int> ElementosFollow = Follows[Convert.ToInt16(Elemento)];");
            sw.WriteLine("\t\t\t\t\t\t\t// Convierte los elementos del follow de int a string");
            sw.WriteLine("\t\t\t\t\t\t\tList<string> Elementos = new List<string>();");
            sw.WriteLine("\t\t\t\t\t\t\tforeach (var item in ElementosFollow)");
            sw.WriteLine("\t\t\t\t\t\t\t\tElementos.Add(Convert.ToString(item));");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tint posicion = Columna.getNumColumna();");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tLinea[posicion].setElementos(Elementos);");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tbool añadir = true;");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tforeach (var item in Transiciones)");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tif (Linea[Columna.getNumColumna()].getElementos() == item.getElementos())");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tañadir = false;");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\tif (añadir)");
            sw.WriteLine("\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\tbool letraExistente = false;");
            sw.WriteLine("\t\t\t\t\t\t\t\tstring letraAnterior = \"\";");
            sw.WriteLine("\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\tforeach (var item in Transiciones)");
            sw.WriteLine("\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t// Se convierten en strings para comarar si son el mismo emento en la tabla");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tstring cadena1 = \"\";");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tstring cadena2 = \"\";");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tforeach (var elemento in item.getElementos())");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tcadena1 += elemento;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tforeach (var elemento in Elementos)");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tcadena2 += elemento;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tif (cadena1.Equals(cadena2))");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tletraExistente = true;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tletraAnterior = item.getIdentificador();");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tbreak;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\tif (letraExistente)");
            sw.WriteLine("\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tLinea[Columna.getNumColumna()].setIdentificador(letraAnterior);");
            sw.WriteLine("\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t\telse");
            sw.WriteLine("\t\t\t\t\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tLinea[Columna.getNumColumna()].setIdentificador(Convert.ToString(letra));");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tletra++;");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tTransiciones.Add(Linea[Columna.getNumColumna()]);");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tPendientes.Enqueue(Linea[Columna.getNumColumna()]);");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t\t\t\t\tif (Pendientes.Count != 0 && analizar == false)");
            sw.WriteLine("\t\t\t\t\t\t\t\t\t\tanalizar = true;");
            sw.WriteLine("\t\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t\t}");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\ttry");
            sw.WriteLine("\t\t\t\t{");
            sw.WriteLine("\t\t\t\t\tcant++;");
            sw.WriteLine("\t\t\t\t\ttemp = Pendientes.Dequeue();");
            sw.WriteLine("\t\t\t\t\tFirstPadre = temp.getElementos();");
            sw.WriteLine("\t\t\t\t\tstring identificador = temp.getIdentificador();");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tLinea = new Transicion[Encabezado.Length + 1];");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\t// Inicializacion de la lista");
            sw.WriteLine("\t\t\t\t\tfor (int i = 0; i < Linea.Length; i++)");
            sw.WriteLine("\t\t\t\t\t\tLinea[i] = new Transicion();");
            sw.WriteLine("\t\t\t\t\t");
            sw.WriteLine("\t\t\t\t\tLinea[0].setElementos(FirstPadre);");
            sw.WriteLine("\t\t\t\t\tLinea[0].setIdentificador(identificador);");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\tcatch (Exception e) { }");
            sw.WriteLine("\t\t\t\t");
            sw.WriteLine("\t\t\t\t// Cuando ya no hayan elementos en la cola se activa el trigger UltimaIteracion");
            sw.WriteLine("\t\t\t\tif (Pendientes.Count == 0)");
            sw.WriteLine("\t\t\t\t\tUltimaIteracion = true; ");
            sw.WriteLine("\t\t\t} while (analizar);");
            sw.WriteLine("\t\t\t");
            sw.WriteLine("\t\t\treturn cant;");
            sw.WriteLine("\t\t}");
            sw.WriteLine("\t\t");


        }

        private void EscribirClases(ref StreamWriter sw)
        {
            StreamReader sr = new StreamReader("C:\\Users\\Admin\\Documents\\GitHub\\Generador-de-Scanner\\Generador de Scanner\\Clases.txt");
            sw.Write(sr.ReadToEnd());
        }

        // OPERACION DEL ARCHIVO PARA EL DESARROLLO DEL AUTOMATA

        /// <summary>
        /// Se obtienen los First Lasts operando los Tokens... Finalmente se obtienen los Follows
        /// </summary>
        /// <param name="Sets">Sets</param>
        /// <param name="Tokens">Tokens</param>
        /// <param name="follows">Variable por Referencia</param>
        private void OperarArchivo(List<Set> Sets, List<Token> Tokens, ref List<string> follows, ref List<string> lineasNFL)
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

                    Operador.setExpresionAcumulada(C1.getExpresionAcumulada() + Operador.getContenido() + C2.getExpresionAcumulada());

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

            FOperador.setExpresionAcumulada(FC1.getExpresionAcumulada() + FOperador.getContenido() + FC2.getExpresionAcumulada());


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
            PostOrden(Posfijo.Peek(), ref lineasNFL);

            foreach (var item in Follows)
            {
                string elemento = "";
                foreach (var elementos in item.Value)
                {
                    elemento += elementos + ",";
                }

                follows.Add(Convert.ToString(item.Key) + " --> " + elemento);
            }

            // Se transforma el first en una lista
            List<string> FirstPadre = new List<string>();
            string[] FPadre = FOperador.getFirst().Split(',');

            foreach (var item in FPadre)
            {
                FirstPadre.Add(item);
            }

            Transicion[,] TablaDeTransiciones = TablaTransiciones(FirstPadre, Leafs, Follows);

            //MessageBox.Show("Todo OKKKK");

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


        /// <summary>
        /// Metodo que recorre el Arbol en PostOrden y obtiene los firsts y lasts para imprimirlos
        /// </summary>
        /// <param name="nodoAuxiliar">Nodo Auxiliar</param>
        /// <param name="Follows"> Diccionario con los follows </param>
        private void PostOrden(Node nodoAuxiliar, ref List<string> lineasNFL)
        {
            if (nodoAuxiliar != null)
            {
                PostOrden(nodoAuxiliar.getC1(), ref lineasNFL);
                PostOrden(nodoAuxiliar.getC2(), ref lineasNFL);

                string contNodo = nodoAuxiliar.getExpresionAcumulada();

                string last = nodoAuxiliar.getLast();
                string first = nodoAuxiliar.getFirst();

                if (nodoAuxiliar.getNulable())
                    lineasNFL.Add("N \t" + contNodo + "\t\t " + "F(" + first + ")" + "   " + "L(" + last + ")                                                        ");
                else
                    lineasNFL.Add("NN \t" + contNodo + "\t\t " + "F(" + first + ")" + "   " + "L(" + last + ")                                                        ");

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

            bool analizar = true;
            bool UltimaIteracion = false;

            do
            {
                // Gatillo para ya no seguir analizando (Solo la ultima iteracion <<Actual>>)
                if (UltimaIteracion == true)
                    analizar = false;

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

                                    if (Pendientes.Count != 0 && analizar == false)
                                        analizar = true;
                                }
                            }
                        }
                    }
                }

                try
                {
                    fila++;
                    temp = Pendientes.Dequeue();
                    FirstPadre = temp.getElementos();

                    TablaDeTransiciones[fila, 0].setIdentificador(temp.getIdentificador());
                    TablaDeTransiciones[fila, 0].setElementos(temp.getElementos());

                    string identificador = temp.getIdentificador();
                }
                catch (Exception e) { }

                // Cuando ya no hayan elementos en la cola se activa el trigger UltimaIteracion
                if (Pendientes.Count == 0)
                    UltimaIteracion = true;

            } while (analizar);

            // Cual es el simbolo terminal
            int simboloTerminal = Leafs.Count;

            // Impresion en el DataGridView
            for (int i = 0; i < filas - 1; i++)
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

                if (nombres[i] == "α")
                    Encabezado[i].setNombre("(");
                else if (nombres[i] == "β")
                    Encabezado[i].setNombre(")");
                else if (nombres[i] == "ɣ")
                    Encabezado[i].setNombre(".");
                else if (nombres[i] == "δ")
                    Encabezado[i].setNombre("*");
                else if (nombres[i] == "ε")
                    Encabezado[i].setNombre("+");
                else if (nombres[i] == "ϑ")
                    Encabezado[i].setNombre("?");
                else 
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

            bool analizar = true;
            bool UltimaIteracion = false;

            do
            {
                // Gatillo para ya no seguir analizando (Solo la ultima iteracion <<Actual>>)
                if (UltimaIteracion == true)
                    analizar = false;

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

                                    if (Pendientes.Count != 0 && analizar == false)
                                        analizar = true;
                                }
                            }
                        }
                    }
                }

                try
                {
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
                }
                catch (Exception e) { }

                // Cuando ya no hayan elementos en la cola se activa el trigger UltimaIteracion
                if (Pendientes.Count == 0)
                    UltimaIteracion = true;

            } while (analizar);

            dgv_TablaTrancisiones.Rows.Add(cant - 2);

            return cant;
        }
    }
}
