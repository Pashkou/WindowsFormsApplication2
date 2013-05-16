using Company.Master2.xmlmodel;
using Microsoft.master2.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.rules
{
    class Heuristics
    {

        public static ArrayList updateRelevanceForSelect(ArrayList cSharpModel, string selectedClass,string selectedMethod){
            foreach (CSharpClass existedClass in cSharpModel)
            {
  

                if (existedClass.Name == selectedClass)
                {
                    ArrayList existedMethods = existedClass.Methods;
                    foreach (CSharpMethod cSharpExistedMethod in existedMethods)
                    {
                        if (cSharpExistedMethod.Name == selectedMethod) {
                            cSharpExistedMethod.Relevance += 0.5;
                        }
                    }

                }
            }
            return cSharpModel;
        
        }

        public static ArrayList updateRelevance(ArrayList cSharpModel, CSharpClass cSharpClass, bool isRefreshButton)
        {
            foreach (CSharpClass existedClass in cSharpModel)
            {
                if (existedClass.Equals(cSharpClass))
                {
                    if (!isRefreshButton)
                    {
                        existedClass.Relevance += 1;//cSharpClass.Relevance;
                    }
                    ArrayList existedMethods = existedClass.Methods;
                    if (isRefreshButton)
                    {
                        foreach (CSharpMethod cSharpExistedMethod in existedMethods)
                        {
                            ArrayList methods = cSharpClass.Methods;
                            if (cSharpExistedMethod.Name.Equals(cSharpClass.SelectedMethod))
                            {
                                cSharpExistedMethod.Relevance += 0.1;
                            }
                           /* foreach (CSharpMethod cSharpMethod in methods)
                            {
                                if (cSharpExistedMethod.Name.Equals(cSharpClass.SelectedMethod))
                                {
                                    cSharpExistedMethod.Relevance += 0.1;
                                }
                            }*/
                        }

                        ArrayList existedFields = existedClass.Fields;
                        foreach (CSharpField cSharpExistedField in existedFields)
                        {
                            ArrayList fields = cSharpClass.Fields;
                            foreach (CSharpField cSharpField in fields)
                            {
                                if (cSharpExistedField.Equals(cSharpField))
                                {
                                    cSharpExistedField.Relevance += cSharpField.Relevance;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (existedClass.Relevance > 0)
                    {
                        existedClass.Relevance -= 0.1;
                    }
                }
            }

            if (!cSharpModel.Contains(cSharpClass))
            {
                cSharpClass.Relevance = 1;
                cSharpModel.Add(cSharpClass);
            }
            return cSharpModel;
        }
    }
}
