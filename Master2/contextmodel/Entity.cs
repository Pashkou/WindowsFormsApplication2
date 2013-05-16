using Microsoft.master2.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.master2.contextmodel
{
    /* public class Entity : DependencyObject
     {
         ArrayList cSharpClasses = new ArrayList();
         ArrayList edges = new ArrayList();
      

         public ArrayList Edges
         {
             get { return edges; }
             set { edges = value; }
         }
         public ArrayList CSharpClasses
         {
             get { return cSharpClasses; }
             set { cSharpClasses = value; }
         }
         private int commonThreshold = 3;

         public int CommonThreshold
         {
             get { return commonThreshold; }
             set { commonThreshold = value; }
         }

         public override string ToString()
         {
             string result = "CLASSES:\r\n ";
             cSharpClasses.Sort();
             foreach (CSharpClass cSharpClass in cSharpClasses)
             {
                 result += " Name: "+cSharpClass.Name + "\r\n";
                 result += "  Relevance: " + String.Format("{0:0.0}",  cSharpClass.countRelevance()) + "\r\n";
             }
             result += "\r\n";
             result += "THRESHOLD: " + commonThreshold;
             return result;
         }
     }*/



    class EntityEdge
    {
        private string nameOfTheConnection = null;

        public string NameOfTheConnection
        {
            get { return nameOfTheConnection; }
            set { nameOfTheConnection = value; }
        }
        private string source;

        public string Source
        {
            get { return source; }
            set { source = value; }
        }
        private string target;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        private int weight;

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public EntityEdge(string source, string target)
        {
            this.source = source;
            this.target = target;
            this.Weight = 1;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            EntityEdge p = obj as EntityEdge;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (source == p.source) && (target == p.target);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public override string ToString()
        {
            if (nameOfTheConnection == null)
            {
                return weight.ToString();
            }
            else { 
                int toDispaly = weight - 1;
                return nameOfTheConnection + " ("+toDispaly.ToString()+")";
            }
            
        }
    }
}
