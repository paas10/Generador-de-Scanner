using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_de_Scanner
{
    // Celda de la matriz
    class Transicion
    {
        private string identificador;
        private List<string> elementos;

        public Transicion()
        {
            identificador = "";
            elementos = null;
        }

        public Transicion(string identificador, List<string> elementos)
        {
            this.identificador = identificador;
            this.elementos = elementos;
        }

        public void setIdentificador(string identificador)
        {
            this.identificador = identificador;
        }
        public string getIdentificador()
        {
            return identificador;
        }

        public void setElementos(List<string> elementos)
        {
            if (this.elementos != null)
            {
                foreach (var item in elementos)
                {
                    this.elementos.Add(item);
                }

                this.elementos.Sort();
            }
            else
            {
                this.elementos = elementos;
            }
        }
        public List<string> getElementos()
        {
            return elementos;
        }
        public string getElementosCadena()
        {
            if (elementos != null)
            {
                string cadena = "(";

                for (int i = 0; i < elementos.Count; i++)
                {
                    if (i != (elementos.Count - 1))
                        cadena += elementos[i] + ",";
                    else
                        cadena += elementos[i];
                }

                cadena += ")";

                return cadena;
            }
            else
            {
                return "";
            }
        }
    }
}
