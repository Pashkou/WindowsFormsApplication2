using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.model
{
    public class CSharpMethod : IComparable
    {
        private string name;
        private double relevance = 0;

        public double Relevance
        {
            get { return relevance; }
            set { relevance = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private ArrayList incomingCalls = new ArrayList();

        public ArrayList IncomingCalls
        {
            get { return incomingCalls; }
            set { incomingCalls = value; }
        }

        private ArrayList outgoingCalls = new ArrayList();

        public ArrayList OutgoingCalls
        {
            get { return outgoingCalls; }
            set { outgoingCalls = value; }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            CSharpMethod p = obj as CSharpMethod;
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
            CSharpMethod c1 = this;
            CSharpMethod c2 = (CSharpMethod)obj;
            if (c1.Relevance < c2.Relevance)
                return 1;

            if (c1.Relevance > c2.Relevance)
                return -1;

            else
                return 0;
        }
    }
}
