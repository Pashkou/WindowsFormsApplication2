using Microsoft.master2.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.rules
{
    class TransformClassText
    {
        public static string CSharpClass(CSharpClass cSharpClass)
        {
            string result = "ClassName " + cSharpClass.Name;
            result += "\r\nParent " + cSharpClass.Parent;
            result += "\r\nMethods: ";
            ArrayList methods = cSharpClass.Methods;
            foreach (CSharpMethod cSharpMethod in methods)
            {
                result += "\r\n Name " + cSharpMethod.Name;
            }
            result += "\r\nRelevance " + cSharpClass.Relevance.ToString();
            return result;
        }
    }
}
