using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_de_Scanner
{
    class Node
    {
        private string contenido;
        private string expresionAcumulada;
        private bool nulable;
        private string first;
        private string last;
        private Node C1;
        private Node C2;

        public Node()
        {
            contenido = "";
            expresionAcumulada = "";
            nulable = false;
            first = "";
            last = "";
            C1 = null;
            C2 = null;
        }

        public Node(string contenido, string expresionAcumulada, bool nulable, string first, string last)
        {
            this.contenido = contenido;
            this.expresionAcumulada = expresionAcumulada;
            this.nulable = nulable;
            this.first = first;
            this.last = last;
            C1 = null;
            C2 = null;
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

        public void setC1(Node C1)
        {
            this.C1 = C1;
        }
        public Node getC1()
        {
            return C1;
        }

        public void setC2(Node C2)
        {
            this.C2 = C2;
        }
        public Node getC2()
        {
            return C2;
        }

        public void setExpresionAcumulada(string expresionAcumulada)
        {
            this.expresionAcumulada = expresionAcumulada;
        }
        public string getExpresionAcumulada()
        {
            return expresionAcumulada;
        }
    }
}
