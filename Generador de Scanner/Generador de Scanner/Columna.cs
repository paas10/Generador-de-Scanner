using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_de_Scanner
{
    class Columna
    {
        private int numColumna;
        private string nombre;
        private List<int> hojas;

        public Columna()
        {
            numColumna = 0;
            nombre = "";
            hojas = new List<int>();
        }

        public void setNumColumna(int numColumna)
        {
            this.numColumna = numColumna;
        }
        public int getNumColumna()
        {
            return numColumna;
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }
        public string getNombre()
        {
            return nombre;
        }

        public void setHojas(List<int> hojas)
        {
            this.hojas = hojas; 
        }
        public List<int> getHojas()
        {
            return hojas; 
        }
    }

}
