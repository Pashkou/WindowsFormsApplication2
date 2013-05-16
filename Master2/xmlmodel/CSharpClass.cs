using Company.Master2.xmlmodel;
using Microsoft.master2.contextmodel;
using Microsoft.master2.rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.model
{
    public class CSharpClass : IComparable
    {

        public string SelectedMethod;
        public int RelevanceOfSelectedMethod = 0;
        private string name;
        private string parent;
        private double relevance = 0;
        private int threshold = 3;
        private bool isAddedToEntity = false;
        private ArrayList interfaces = new ArrayList();
        private ArrayList methods = new ArrayList();
        private ArrayList fields = new ArrayList();
        private ArrayList navigationElements = new ArrayList();
        public int NumberInThePast = 0;
        private bool isSelected = false;
        public int NumerOfEdges = 1;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        private bool isSecondSelected = false;

        public bool IsSecondSelected
        {
            get { return isSecondSelected; }
            set { isSecondSelected = value; }
        }


        public ArrayList NavigationElements
        {
            get { return navigationElements; }
            set { navigationElements = value; }
        }

        public ArrayList Fields
        {
            get { return fields; }
            set { fields = value; }
        }
        private ArrayList incomingEdges = new ArrayList();
        private ArrayList outgoingEdges = new ArrayList();

        public static CSharpClass calculateEdges(CSharpClass cSharpClass)
        {
            ArrayList methods = cSharpClass.Methods;
            foreach (CSharpMethod cSharpMethod in methods)
            {
                calculateForIncomingCalls(cSharpClass, cSharpMethod);
                calculateForOutgoingCalls(cSharpClass, cSharpMethod);
            }
            return cSharpClass;
        }

        private static void calculateForIncomingCalls(CSharpClass cSharpClass, CSharpMethod cSharpMethod)
        {

            ArrayList incomingCalls = cSharpMethod.IncomingCalls;
            foreach (IncomingCall incomingCall in incomingCalls)
            {
                string className = incomingCall.ClassName;
                CSharpClassEdge classEdge = new CSharpClassEdge(cSharpClass.Name, incomingCall.ClassName);
                if (!cSharpClass.incomingEdges.Contains(classEdge))
                {
                    cSharpClass.incomingEdges.Add(classEdge);
                }
                else
                {
                    IEnumerator enumeratorEdges = cSharpClass.incomingEdges.GetEnumerator();
                    while (enumeratorEdges.MoveNext())
                    {
                        CSharpClassEdge currentEdge = (CSharpClassEdge)enumeratorEdges.Current;

                        if (currentEdge.Equals(classEdge))
                        {
                            currentEdge.Weight++;
                        }
                    }
                }
            }

        }

        public double countRelevance()
        {
            double result = Relevance;
            foreach (CSharpMethod cSharpMethod in Methods)
            {
                result += cSharpMethod.Relevance;
            }
            foreach (CSharpField cSharpField in Fields)
            {
                result += cSharpField.Relevance;
            }
            return result;
        }
        private static void calculateForOutgoingCalls(CSharpClass cSharpClass, CSharpMethod cSharpMethod)
        {
            //  ArrayList outgoingEdges = new ArrayList();
            ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
            foreach (MethodTypeCall outgoingCall in outgoingCalls)
            {
                string className = outgoingCall.ClassName;
                CSharpClassEdge classEdge = new CSharpClassEdge(cSharpClass.Name, outgoingCall.ClassName);
                if (!cSharpClass.outgoingEdges.Contains(classEdge))
                {
                    cSharpClass.outgoingEdges.Add(classEdge);
                }
                else
                {
                    IEnumerator enumeratorEdges = cSharpClass.outgoingEdges.GetEnumerator();
                    while (enumeratorEdges.MoveNext())
                    {
                        CSharpClassEdge currentEdge = (CSharpClassEdge)enumeratorEdges.Current;

                        if (currentEdge.Equals(classEdge))
                        {
                            currentEdge.Weight++;
                        }
                    }
                }
            }
            //   return outgoingEdges;
        }


        public ArrayList OutgoingEdges
        {
            get { return outgoingEdges; }
            set { outgoingEdges = value; }
        }

        public ArrayList IncomingEdges
        {
            get { return incomingEdges; }
            set { incomingEdges = value; }
        }

        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        public bool IsAddedToEntity
        {
            get { return isAddedToEntity; }
            set { isAddedToEntity = value; }
        }

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


        public string Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        public ArrayList Interfaces
        {
            get { return interfaces; }
            set { interfaces = value; }
        }


        public ArrayList Methods
        {
            get { return methods; }
            set { methods = value; }
        }

        public override string ToString()
        {
            return TransformClassText.CSharpClass(this);
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            CSharpClass p = obj as CSharpClass;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Name == p.Name) && (Parent == p.Parent);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        private class sortRelecvanceHelper : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                CSharpClass c1 = (CSharpClass)a;
                CSharpClass c2 = (CSharpClass)b;

                if (c1.Relevance < c2.Relevance)
                    return 1;

                if (c1.Relevance > c2.Relevance)
                    return -1;

                else
                    return 0;
            }
        }
        // Implement IComparable CompareTo to provide default sort order.
        int IComparable.CompareTo(object obj)
        {
            CSharpClass c1 = this;
            CSharpClass c2 = (CSharpClass)obj;
            if (c1.Relevance < c2.Relevance)
                return 1;

            if (c1.Relevance > c2.Relevance)
                return -1;

            else
                return 0;
        }

        // Method to return IComparer object for sort helper.
        public static IComparer sortYearAscending()
        {
            return (IComparer)new sortRelecvanceHelper();
        }

        public static void resetModel(ArrayList cSharpClasses)
        {
            foreach (CSharpClass cSharpClass in cSharpClasses)
            {
                cSharpClass.isAddedToEntity = false;
            }
        }
    }

    public class CSharpClassEdge
    {
        private string source;

        public CSharpClassEdge(string source, string target)
        {
            this.source = source;
            this.target = target;
            this.weight = 1;
        }
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

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            CSharpClassEdge p = obj as CSharpClassEdge;
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
    }
}
