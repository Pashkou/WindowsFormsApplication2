using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Master2.xmlmodel
{
    class CSharpField : IComparable
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private double relevance = 0;

        public double Relevance
        {
            get { return relevance; }
            set { relevance = value; }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            CSharpField p = obj as CSharpField;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Name == p.Name);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        // Implement IComparable CompareTo to provide default sort order.
        int IComparable.CompareTo(object obj)
        {
            CSharpField c1 = this;
            CSharpField c2 = (CSharpField)obj;
            if (c1.Relevance < c2.Relevance)
                return 1;

            if (c1.Relevance > c2.Relevance)
                return -1;

            else
                return 0;
        }
    }
}
