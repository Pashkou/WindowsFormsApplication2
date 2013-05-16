using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Master2.rules
{
    class NavigationElement
    {
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private string classMethod;

        public string ClassMethod
        {
            get { return classMethod; }
            set { classMethod = value; }
        }

        private ArrayList navigationElements = new ArrayList();

        public ArrayList NavigationElements
        {
            get { return navigationElements; }
            set { navigationElements = value; }
        }
    }
}
