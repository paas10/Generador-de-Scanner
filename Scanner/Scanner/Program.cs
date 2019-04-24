using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
	class Program
	{
		public static List<int> EstadosAceptacion = new List<int>();
		Procesos procesos = new Procesos();
		
		static void Main(string[] args)
		{
			
			int error = 54;
		
			List<Set> Sets = new List<Set>();
			Set SetTemp;
			List<string> elementos;
			
			SetTemp = new Set("LETRA");
			elementos = new List<string>();
			
			elementos.Add(Convert.ToString(Convert.ToChar(65)));
			elementos.Add(Convert.ToString(Convert.ToChar(66)));
			elementos.Add(Convert.ToString(Convert.ToChar(67)));
			elementos.Add(Convert.ToString(Convert.ToChar(68)));
			elementos.Add(Convert.ToString(Convert.ToChar(69)));
			elementos.Add(Convert.ToString(Convert.ToChar(70)));
			elementos.Add(Convert.ToString(Convert.ToChar(71)));
			elementos.Add(Convert.ToString(Convert.ToChar(72)));
			elementos.Add(Convert.ToString(Convert.ToChar(73)));
			elementos.Add(Convert.ToString(Convert.ToChar(74)));
			elementos.Add(Convert.ToString(Convert.ToChar(75)));
			elementos.Add(Convert.ToString(Convert.ToChar(76)));
			elementos.Add(Convert.ToString(Convert.ToChar(77)));
			elementos.Add(Convert.ToString(Convert.ToChar(78)));
			elementos.Add(Convert.ToString(Convert.ToChar(79)));
			elementos.Add(Convert.ToString(Convert.ToChar(80)));
			elementos.Add(Convert.ToString(Convert.ToChar(81)));
			elementos.Add(Convert.ToString(Convert.ToChar(82)));
			elementos.Add(Convert.ToString(Convert.ToChar(83)));
			elementos.Add(Convert.ToString(Convert.ToChar(84)));
			elementos.Add(Convert.ToString(Convert.ToChar(85)));
			elementos.Add(Convert.ToString(Convert.ToChar(86)));
			elementos.Add(Convert.ToString(Convert.ToChar(87)));
			elementos.Add(Convert.ToString(Convert.ToChar(88)));
			elementos.Add(Convert.ToString(Convert.ToChar(89)));
			elementos.Add(Convert.ToString(Convert.ToChar(90)));
			elementos.Add(Convert.ToString(Convert.ToChar(97)));
			elementos.Add(Convert.ToString(Convert.ToChar(98)));
			elementos.Add(Convert.ToString(Convert.ToChar(99)));
			elementos.Add(Convert.ToString(Convert.ToChar(100)));
			elementos.Add(Convert.ToString(Convert.ToChar(101)));
			elementos.Add(Convert.ToString(Convert.ToChar(102)));
			elementos.Add(Convert.ToString(Convert.ToChar(103)));
			elementos.Add(Convert.ToString(Convert.ToChar(104)));
			elementos.Add(Convert.ToString(Convert.ToChar(105)));
			elementos.Add(Convert.ToString(Convert.ToChar(106)));
			elementos.Add(Convert.ToString(Convert.ToChar(107)));
			elementos.Add(Convert.ToString(Convert.ToChar(108)));
			elementos.Add(Convert.ToString(Convert.ToChar(109)));
			elementos.Add(Convert.ToString(Convert.ToChar(110)));
			elementos.Add(Convert.ToString(Convert.ToChar(111)));
			elementos.Add(Convert.ToString(Convert.ToChar(112)));
			elementos.Add(Convert.ToString(Convert.ToChar(113)));
			elementos.Add(Convert.ToString(Convert.ToChar(114)));
			elementos.Add(Convert.ToString(Convert.ToChar(115)));
			elementos.Add(Convert.ToString(Convert.ToChar(116)));
			elementos.Add(Convert.ToString(Convert.ToChar(117)));
			elementos.Add(Convert.ToString(Convert.ToChar(118)));
			elementos.Add(Convert.ToString(Convert.ToChar(119)));
			elementos.Add(Convert.ToString(Convert.ToChar(120)));
			elementos.Add(Convert.ToString(Convert.ToChar(121)));
			elementos.Add(Convert.ToString(Convert.ToChar(122)));
			elementos.Add(Convert.ToString(Convert.ToChar(95)));
			SetTemp.setElementos(elementos);
			Sets.Add(SetTemp);
			SetTemp = new Set();
			elementos = new List<String>();
			
			
			SetTemp = new Set("DIGITO");
			elementos = new List<string>();
			
			elementos.Add(Convert.ToString(Convert.ToChar(48)));
			elementos.Add(Convert.ToString(Convert.ToChar(49)));
			elementos.Add(Convert.ToString(Convert.ToChar(50)));
			elementos.Add(Convert.ToString(Convert.ToChar(51)));
			elementos.Add(Convert.ToString(Convert.ToChar(52)));
			elementos.Add(Convert.ToString(Convert.ToChar(53)));
			elementos.Add(Convert.ToString(Convert.ToChar(54)));
			elementos.Add(Convert.ToString(Convert.ToChar(55)));
			elementos.Add(Convert.ToString(Convert.ToChar(56)));
			elementos.Add(Convert.ToString(Convert.ToChar(57)));
			SetTemp.setElementos(elementos);
			Sets.Add(SetTemp);
			SetTemp = new Set();
			elementos = new List<String>();
			
			
			SetTemp = new Set("CHARSET");
			elementos = new List<string>();
			
			elementos.Add(Convert.ToString(Convert.ToChar(32)));
			elementos.Add(Convert.ToString(Convert.ToChar(33)));
			elementos.Add(Convert.ToString(Convert.ToChar(34)));
			elementos.Add(Convert.ToString(Convert.ToChar(35)));
			elementos.Add(Convert.ToString(Convert.ToChar(36)));
			elementos.Add(Convert.ToString(Convert.ToChar(37)));
			elementos.Add(Convert.ToString(Convert.ToChar(38)));
			elementos.Add(Convert.ToString(Convert.ToChar(39)));
			elementos.Add(Convert.ToString(Convert.ToChar(40)));
			elementos.Add(Convert.ToString(Convert.ToChar(41)));
			elementos.Add(Convert.ToString(Convert.ToChar(42)));
			elementos.Add(Convert.ToString(Convert.ToChar(43)));
			elementos.Add(Convert.ToString(Convert.ToChar(44)));
			elementos.Add(Convert.ToString(Convert.ToChar(45)));
			elementos.Add(Convert.ToString(Convert.ToChar(46)));
			elementos.Add(Convert.ToString(Convert.ToChar(47)));
			elementos.Add(Convert.ToString(Convert.ToChar(48)));
			elementos.Add(Convert.ToString(Convert.ToChar(49)));
			elementos.Add(Convert.ToString(Convert.ToChar(50)));
			elementos.Add(Convert.ToString(Convert.ToChar(51)));
			elementos.Add(Convert.ToString(Convert.ToChar(52)));
			elementos.Add(Convert.ToString(Convert.ToChar(53)));
			elementos.Add(Convert.ToString(Convert.ToChar(54)));
			elementos.Add(Convert.ToString(Convert.ToChar(55)));
			elementos.Add(Convert.ToString(Convert.ToChar(56)));
			elementos.Add(Convert.ToString(Convert.ToChar(57)));
			elementos.Add(Convert.ToString(Convert.ToChar(58)));
			elementos.Add(Convert.ToString(Convert.ToChar(59)));
			elementos.Add(Convert.ToString(Convert.ToChar(60)));
			elementos.Add(Convert.ToString(Convert.ToChar(61)));
			elementos.Add(Convert.ToString(Convert.ToChar(62)));
			elementos.Add(Convert.ToString(Convert.ToChar(63)));
			elementos.Add(Convert.ToString(Convert.ToChar(64)));
			elementos.Add(Convert.ToString(Convert.ToChar(65)));
			elementos.Add(Convert.ToString(Convert.ToChar(66)));
			elementos.Add(Convert.ToString(Convert.ToChar(67)));
			elementos.Add(Convert.ToString(Convert.ToChar(68)));
			elementos.Add(Convert.ToString(Convert.ToChar(69)));
			elementos.Add(Convert.ToString(Convert.ToChar(70)));
			elementos.Add(Convert.ToString(Convert.ToChar(71)));
			elementos.Add(Convert.ToString(Convert.ToChar(72)));
			elementos.Add(Convert.ToString(Convert.ToChar(73)));
			elementos.Add(Convert.ToString(Convert.ToChar(74)));
			elementos.Add(Convert.ToString(Convert.ToChar(75)));
			elementos.Add(Convert.ToString(Convert.ToChar(76)));
			elementos.Add(Convert.ToString(Convert.ToChar(77)));
			elementos.Add(Convert.ToString(Convert.ToChar(78)));
			elementos.Add(Convert.ToString(Convert.ToChar(79)));
			elementos.Add(Convert.ToString(Convert.ToChar(80)));
			elementos.Add(Convert.ToString(Convert.ToChar(81)));
			elementos.Add(Convert.ToString(Convert.ToChar(82)));
			elementos.Add(Convert.ToString(Convert.ToChar(83)));
			elementos.Add(Convert.ToString(Convert.ToChar(84)));
			elementos.Add(Convert.ToString(Convert.ToChar(85)));
			elementos.Add(Convert.ToString(Convert.ToChar(86)));
			elementos.Add(Convert.ToString(Convert.ToChar(87)));
			elementos.Add(Convert.ToString(Convert.ToChar(88)));
			elementos.Add(Convert.ToString(Convert.ToChar(89)));
			elementos.Add(Convert.ToString(Convert.ToChar(90)));
			elementos.Add(Convert.ToString(Convert.ToChar(91)));
			elementos.Add(Convert.ToString(Convert.ToChar(92)));
			elementos.Add(Convert.ToString(Convert.ToChar(93)));
			elementos.Add(Convert.ToString(Convert.ToChar(94)));
			elementos.Add(Convert.ToString(Convert.ToChar(95)));
			elementos.Add(Convert.ToString(Convert.ToChar(96)));
			elementos.Add(Convert.ToString(Convert.ToChar(97)));
			elementos.Add(Convert.ToString(Convert.ToChar(98)));
			elementos.Add(Convert.ToString(Convert.ToChar(99)));
			elementos.Add(Convert.ToString(Convert.ToChar(100)));
			elementos.Add(Convert.ToString(Convert.ToChar(101)));
			elementos.Add(Convert.ToString(Convert.ToChar(102)));
			elementos.Add(Convert.ToString(Convert.ToChar(103)));
			elementos.Add(Convert.ToString(Convert.ToChar(104)));
			elementos.Add(Convert.ToString(Convert.ToChar(105)));
			elementos.Add(Convert.ToString(Convert.ToChar(106)));
			elementos.Add(Convert.ToString(Convert.ToChar(107)));
			elementos.Add(Convert.ToString(Convert.ToChar(108)));
			elementos.Add(Convert.ToString(Convert.ToChar(109)));
			elementos.Add(Convert.ToString(Convert.ToChar(110)));
			elementos.Add(Convert.ToString(Convert.ToChar(111)));
			elementos.Add(Convert.ToString(Convert.ToChar(112)));
			elementos.Add(Convert.ToString(Convert.ToChar(113)));
			elementos.Add(Convert.ToString(Convert.ToChar(114)));
			elementos.Add(Convert.ToString(Convert.ToChar(115)));
			elementos.Add(Convert.ToString(Convert.ToChar(116)));
			elementos.Add(Convert.ToString(Convert.ToChar(117)));
			elementos.Add(Convert.ToString(Convert.ToChar(118)));
			elementos.Add(Convert.ToString(Convert.ToChar(119)));
			elementos.Add(Convert.ToString(Convert.ToChar(120)));
			elementos.Add(Convert.ToString(Convert.ToChar(121)));
			elementos.Add(Convert.ToString(Convert.ToChar(122)));
			elementos.Add(Convert.ToString(Convert.ToChar(123)));
			elementos.Add(Convert.ToString(Convert.ToChar(124)));
			elementos.Add(Convert.ToString(Convert.ToChar(125)));
			elementos.Add(Convert.ToString(Convert.ToChar(126)));
			elementos.Add(Convert.ToString(Convert.ToChar(127)));
			elementos.Add(Convert.ToString(Convert.ToChar(128)));
			elementos.Add(Convert.ToString(Convert.ToChar(129)));
			elementos.Add(Convert.ToString(Convert.ToChar(130)));
			elementos.Add(Convert.ToString(Convert.ToChar(131)));
			elementos.Add(Convert.ToString(Convert.ToChar(132)));
			elementos.Add(Convert.ToString(Convert.ToChar(133)));
			elementos.Add(Convert.ToString(Convert.ToChar(134)));
			elementos.Add(Convert.ToString(Convert.ToChar(135)));
			elementos.Add(Convert.ToString(Convert.ToChar(136)));
			elementos.Add(Convert.ToString(Convert.ToChar(137)));
			elementos.Add(Convert.ToString(Convert.ToChar(138)));
			elementos.Add(Convert.ToString(Convert.ToChar(139)));
			elementos.Add(Convert.ToString(Convert.ToChar(140)));
			elementos.Add(Convert.ToString(Convert.ToChar(141)));
			elementos.Add(Convert.ToString(Convert.ToChar(142)));
			elementos.Add(Convert.ToString(Convert.ToChar(143)));
			elementos.Add(Convert.ToString(Convert.ToChar(144)));
			elementos.Add(Convert.ToString(Convert.ToChar(145)));
			elementos.Add(Convert.ToString(Convert.ToChar(146)));
			elementos.Add(Convert.ToString(Convert.ToChar(147)));
			elementos.Add(Convert.ToString(Convert.ToChar(148)));
			elementos.Add(Convert.ToString(Convert.ToChar(149)));
			elementos.Add(Convert.ToString(Convert.ToChar(150)));
			elementos.Add(Convert.ToString(Convert.ToChar(151)));
			elementos.Add(Convert.ToString(Convert.ToChar(152)));
			elementos.Add(Convert.ToString(Convert.ToChar(153)));
			elementos.Add(Convert.ToString(Convert.ToChar(154)));
			elementos.Add(Convert.ToString(Convert.ToChar(155)));
			elementos.Add(Convert.ToString(Convert.ToChar(156)));
			elementos.Add(Convert.ToString(Convert.ToChar(157)));
			elementos.Add(Convert.ToString(Convert.ToChar(158)));
			elementos.Add(Convert.ToString(Convert.ToChar(159)));
			elementos.Add(Convert.ToString(Convert.ToChar(160)));
			elementos.Add(Convert.ToString(Convert.ToChar(161)));
			elementos.Add(Convert.ToString(Convert.ToChar(162)));
			elementos.Add(Convert.ToString(Convert.ToChar(163)));
			elementos.Add(Convert.ToString(Convert.ToChar(164)));
			elementos.Add(Convert.ToString(Convert.ToChar(165)));
			elementos.Add(Convert.ToString(Convert.ToChar(166)));
			elementos.Add(Convert.ToString(Convert.ToChar(167)));
			elementos.Add(Convert.ToString(Convert.ToChar(168)));
			elementos.Add(Convert.ToString(Convert.ToChar(169)));
			elementos.Add(Convert.ToString(Convert.ToChar(170)));
			elementos.Add(Convert.ToString(Convert.ToChar(171)));
			elementos.Add(Convert.ToString(Convert.ToChar(172)));
			elementos.Add(Convert.ToString(Convert.ToChar(173)));
			elementos.Add(Convert.ToString(Convert.ToChar(174)));
			elementos.Add(Convert.ToString(Convert.ToChar(175)));
			elementos.Add(Convert.ToString(Convert.ToChar(176)));
			elementos.Add(Convert.ToString(Convert.ToChar(177)));
			elementos.Add(Convert.ToString(Convert.ToChar(178)));
			elementos.Add(Convert.ToString(Convert.ToChar(179)));
			elementos.Add(Convert.ToString(Convert.ToChar(180)));
			elementos.Add(Convert.ToString(Convert.ToChar(181)));
			elementos.Add(Convert.ToString(Convert.ToChar(182)));
			elementos.Add(Convert.ToString(Convert.ToChar(183)));
			elementos.Add(Convert.ToString(Convert.ToChar(184)));
			elementos.Add(Convert.ToString(Convert.ToChar(185)));
			elementos.Add(Convert.ToString(Convert.ToChar(186)));
			elementos.Add(Convert.ToString(Convert.ToChar(187)));
			elementos.Add(Convert.ToString(Convert.ToChar(188)));
			elementos.Add(Convert.ToString(Convert.ToChar(189)));
			elementos.Add(Convert.ToString(Convert.ToChar(190)));
			elementos.Add(Convert.ToString(Convert.ToChar(191)));
			elementos.Add(Convert.ToString(Convert.ToChar(192)));
			elementos.Add(Convert.ToString(Convert.ToChar(193)));
			elementos.Add(Convert.ToString(Convert.ToChar(194)));
			elementos.Add(Convert.ToString(Convert.ToChar(195)));
			elementos.Add(Convert.ToString(Convert.ToChar(196)));
			elementos.Add(Convert.ToString(Convert.ToChar(197)));
			elementos.Add(Convert.ToString(Convert.ToChar(198)));
			elementos.Add(Convert.ToString(Convert.ToChar(199)));
			elementos.Add(Convert.ToString(Convert.ToChar(200)));
			elementos.Add(Convert.ToString(Convert.ToChar(201)));
			elementos.Add(Convert.ToString(Convert.ToChar(202)));
			elementos.Add(Convert.ToString(Convert.ToChar(203)));
			elementos.Add(Convert.ToString(Convert.ToChar(204)));
			elementos.Add(Convert.ToString(Convert.ToChar(205)));
			elementos.Add(Convert.ToString(Convert.ToChar(206)));
			elementos.Add(Convert.ToString(Convert.ToChar(207)));
			elementos.Add(Convert.ToString(Convert.ToChar(208)));
			elementos.Add(Convert.ToString(Convert.ToChar(209)));
			elementos.Add(Convert.ToString(Convert.ToChar(210)));
			elementos.Add(Convert.ToString(Convert.ToChar(211)));
			elementos.Add(Convert.ToString(Convert.ToChar(212)));
			elementos.Add(Convert.ToString(Convert.ToChar(213)));
			elementos.Add(Convert.ToString(Convert.ToChar(214)));
			elementos.Add(Convert.ToString(Convert.ToChar(215)));
			elementos.Add(Convert.ToString(Convert.ToChar(216)));
			elementos.Add(Convert.ToString(Convert.ToChar(217)));
			elementos.Add(Convert.ToString(Convert.ToChar(218)));
			elementos.Add(Convert.ToString(Convert.ToChar(219)));
			elementos.Add(Convert.ToString(Convert.ToChar(220)));
			elementos.Add(Convert.ToString(Convert.ToChar(221)));
			elementos.Add(Convert.ToString(Convert.ToChar(222)));
			elementos.Add(Convert.ToString(Convert.ToChar(223)));
			elementos.Add(Convert.ToString(Convert.ToChar(224)));
			elementos.Add(Convert.ToString(Convert.ToChar(225)));
			elementos.Add(Convert.ToString(Convert.ToChar(226)));
			elementos.Add(Convert.ToString(Convert.ToChar(227)));
			elementos.Add(Convert.ToString(Convert.ToChar(228)));
			elementos.Add(Convert.ToString(Convert.ToChar(229)));
			elementos.Add(Convert.ToString(Convert.ToChar(230)));
			elementos.Add(Convert.ToString(Convert.ToChar(231)));
			elementos.Add(Convert.ToString(Convert.ToChar(232)));
			elementos.Add(Convert.ToString(Convert.ToChar(233)));
			elementos.Add(Convert.ToString(Convert.ToChar(234)));
			elementos.Add(Convert.ToString(Convert.ToChar(235)));
			elementos.Add(Convert.ToString(Convert.ToChar(236)));
			elementos.Add(Convert.ToString(Convert.ToChar(237)));
			elementos.Add(Convert.ToString(Convert.ToChar(238)));
			elementos.Add(Convert.ToString(Convert.ToChar(239)));
			elementos.Add(Convert.ToString(Convert.ToChar(240)));
			elementos.Add(Convert.ToString(Convert.ToChar(241)));
			elementos.Add(Convert.ToString(Convert.ToChar(242)));
			elementos.Add(Convert.ToString(Convert.ToChar(243)));
			elementos.Add(Convert.ToString(Convert.ToChar(244)));
			elementos.Add(Convert.ToString(Convert.ToChar(245)));
			elementos.Add(Convert.ToString(Convert.ToChar(246)));
			elementos.Add(Convert.ToString(Convert.ToChar(247)));
			elementos.Add(Convert.ToString(Convert.ToChar(248)));
			elementos.Add(Convert.ToString(Convert.ToChar(249)));
			elementos.Add(Convert.ToString(Convert.ToChar(250)));
			elementos.Add(Convert.ToString(Convert.ToChar(251)));
			elementos.Add(Convert.ToString(Convert.ToChar(252)));
			elementos.Add(Convert.ToString(Convert.ToChar(253)));
			elementos.Add(Convert.ToString(Convert.ToChar(254)));
			SetTemp.setElementos(elementos);
			Sets.Add(SetTemp);
			SetTemp = new Set();
			elementos = new List<String>();
			
			
			Dictionary<string, int> Tokens = new Dictionary<string, int>();
			
			Tokens.Add("(DIGITO.(DIGITO*))", 1);
			
			
			Dictionary<string, int> Actions = new Dictionary<string, int>();
			
			Actions.Add("PROGRAM", 18);
			Actions.Add("INCLUDE", 19);
			Actions.Add("CONST", 20);
			Actions.Add("TYPE", 21);
			Actions.Add("VAR", 22);
			
			
			string line;
			
			StreamReader sr = new StreamReader("C:\\Users\\Admin\\Desktop\\Prueba.txt");
			line = sr.ReadLine();
			
			string[] fragmentos = line.Split(' ');
			
			foreach (var fragmento in fragmentos)
			{
				if (Actions.ContainsKey(fragmento))
				{
					Console.WriteLine(Actions[fragmento]);
				}
				else
				{
					
					foreach (var item in Tokens)
					{
						Transicion[,] TablaDeTransiciones = OperarArchivo(Sets, item.Key);
						EstadosAceptacion = new List<int>();
					}
				}
			}
			
			Console.ReadKey();
		}
		
		static private Transicion[,] OperarArchivo(List<Set> Sets, string Token)
		{
			int leaf = 1;
			int cont = 1;
			Stack<Node> Posfijo = new Stack<Node>();
			List<Node> Leafs = new List<Node>();
			
			// Se opera el token
			string ER = Token;
			
			Procesos procesos = new Procesos();
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
				
				Operador.setFirst(C1.getFirst() + ", " + C2.getFirst());
				Operador.setLast(C1.getLast() + ", " + C2.getLast());
				
				Operador.setC1(C1);
				Operador.setC2(C2);
				
				Operador.setExpresionAcumulada(C1.getExpresionAcumulada() + Operador.getContenido() + C2.getExpresionAcumulada());
				Posfijo.Push(Operador);
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
				FOperador.setFirst(FC1.getFirst() + ", " + FC2.getFirst());
			else
				FOperador.setFirst(FC1.getFirst());
			
			if (FC2.getNulable())
				FOperador.setLast(FC1.getLast() + ", " + FC2.getLast());
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
			
			foreach (var item in Follows)
			{
				string elemento = "";
				foreach (var elementos in item.Value)
				{
					elemento += elementos + ", ";
				}
			}
			
			// Se transforma el first en una lista
			List<string> FirstPadre = new List<string>();
			string[] FPadre = FOperador.getFirst().Split(',');
			
			foreach (var item in FPadre)
			{
				FirstPadre.Add(item);
			}
			
			Transicion[,] TablaDeTransiciones = TablaTransiciones(FirstPadre, Leafs, Follows);
			return TablaDeTransiciones;
		}
		
		static private void InOrden(Node nodoAuxiliar, ref Dictionary<int, List<int>> Follows)
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
								Follows[itemLast].Add(itemFirst);
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
			
			
		static private Transicion[,] TablaTransiciones(List<string> FirstPadre, List<Node> Leafs, Dictionary<int, List<int>> Follows)
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
			bool añadido = false;
			
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
									TablaDeTransiciones[fila, Columna.getNumColumna()].setIdentificador(letraAnterior);
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
				
				if (añadido == false)
				{
					if (Pendientes.Count == 0)
					{
						Transicion Adicional = new Transicion();
						Pendientes.Enqueue(Adicional);
						añadido = true;
					}
				}
			} while (Pendientes.Count != 0);
			
			// Cual es el simbolo terminal
			int simboloTerminal = Leafs.Count;
			
			for (int i = 0; i < filas - 1; i++)
			{
				if (TablaDeTransiciones[i, 0].getElementos().Contains(Convert.ToString(simboloTerminal)))
					EstadosAceptacion.Add(i);
			}
			
			return TablaDeTransiciones;
		}
		
		static private Columna[] ConstruirColumnas(List<Node> Leafs)
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
			
			//for (int i = 0; i < Encabezado.Length; i++)
				//dgv_TablaTrancisiones.Columns.Add(i.ToString(), Encabezado[i].getNombre());
			
			return Encabezado;
		}
		
		
		static private int CantFilas(List<string> FirstPadre, Columna[] Encabezado, Dictionary<int, List<int>> Follows)
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
			
			bool añadido = false;
			
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
				
				if (añadido == false)
				{
					if (Pendientes.Count == 0)
					{
						Transicion Adicional = new Transicion();
						Pendientes.Enqueue(Adicional);
						añadido = true;
					}
				}
			} while (Pendientes.Count != 0);
			
			return cant;
		}
		
	}
}

