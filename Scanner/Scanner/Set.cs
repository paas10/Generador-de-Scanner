using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner
{
    public class Set
    {
        private string nombre;
        private List<string> elementos;

        public Set()
        {
            nombre = "";
            elementos = new List<string>();
        }

        public Set(string nombre)
        {
            this.nombre = nombre;
            elementos = new List<string>();
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }
        public void setElementos(List<string> elementos)
        {
            this.elementos = elementos;
        }

        public string getNombre()
        {
            return this.nombre;
        }
        public List<string> getElementos()
        {
            return this.elementos;
        }
    }
}
