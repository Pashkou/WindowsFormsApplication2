using EnvDTE;
using Microsoft.master2.assembly;
using Microsoft.Master2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.Utils
{
    class InvestigateElement
    {
        public CLassName currentClassA = null;



        public String getCurrentElement(FileCodeModel fileCM)
        {
            String currentClass = null;
            InvestigateElement invElem = new InvestigateElement();
            if (fileCM != null)
            {
                CodeElements elts = null;
                elts = fileCM.CodeElements;
                CodeElement elt = null;
                int i = 0;
                string elementsList = "";
                for (i = 1; i <= fileCM.CodeElements.Count; i++)
                {
                    elt = elts.Item(i);
                    invElem.CollapseElt(elt, elts, i);
                }
               // currentClass = invElem.currentClass;
            }
            return currentClassA.name;
        }


        //check class at runtime
        private string CollapseElt(CodeElement elt, CodeElements elts, long loc)
        {
            string result = "";
            EditPoint epStart = null;
            EditPoint epEnd = null;
            epStart = elt.StartPoint.CreateEditPoint();
            // Do this because we move it later.
            epEnd = elt.EndPoint.CreateEditPoint();
            epStart.EndOfLine();
            if (((elt.IsCodeType) & (elt.Kind != vsCMElement.vsCMElementDelegate)))
            {
                if (currentClassA == null)
                {
                    currentClassA = new CLassName();
                   currentClassA.name =  elt.FullName.ToString();
                }
                 return elt.FullName;

            }
            else if ((elt.Kind == vsCMElement.vsCMElementNamespace))
            {
                CodeNamespace cns = null;
                cns = ((EnvDTE.CodeNamespace)(elt));
                CodeElements mems_vb = null;
                mems_vb = cns.Members;
                int i = 0;

                for (i = 1; i <= cns.Members.Count; i++)
                {
                    CollapseElt(mems_vb.Item(i), mems_vb, i);
                }
            }
            return result;
        }




        /* private string checkChildren(Type parentType)
         {

             string result = "";
             Type[] types = Assembly.GetAssembly(parentType).GetTypes();
             foreach (Type type in types)
             {

                 if ((type.BaseType != null) && (type.BaseType.Equals(parentType)))
                 {
                     result += type.FullName;
                 }
             }
             //  }
             return result;

         }*/

    }


}
