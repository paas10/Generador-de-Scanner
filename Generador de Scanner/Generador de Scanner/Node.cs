using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_de_Scanner
{
    class Node
    {
        private bool operador;
        private string contenido;
        private bool nulable;
        private string first;
        private string last;

        public Node()
        {
            operador = false;
            contenido = "";
            nulable = false;
            first = "";
            last = "";
        }

        public Node(bool operador, string contenido, bool nulable, string first, string last)
        {
            this.operador = operador;
            this.contenido = contenido;
            this.nulable = nulable;
            this.first = first;
            this.last = last;
        }


        public void setOperador(bool operador)
        {
            this.operador = operador; 
        }
        public bool getOperador()
        {
            return operador;
        }

        public void setContenido (string contenido)
        {
            this.contenido = contenido;
        }
        public string getContenido()
        {
            return contenido;
        }

        public void setNulable(bool nulable)
        {
            this.nulable = nulable;
        }
        public bool getNulable()
        {
            return nulable;
        }

        public void setFirst(string first)
        {
            this.first = first;
        }
        public string getFirst()
        {
            return first;
        }

        public void setLast(string last)
        {
            this.last = last;
        }
        public string getLast()
        {
            return last;
        }

    }
}
