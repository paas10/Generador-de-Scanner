using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_de_Scanner
{
    class Token
    {
        private int numeroToken;
        private string elementos;

        // Constructor
        public Token()
        {
            numeroToken = 0;
            elementos = ""; 
        }

        //Setters
        public void setNumeroToken(int numeroToken)
        {
            this.numeroToken = numeroToken;
        }
        public void setElementos(string elementos)
        {
            this.elementos = elementos;
        }

        //Getters
        public int getNumeroToken()
        {
            return numeroToken;
        }
        public string getElementos()
        {
            return elementos;
        }
    }
}
