using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.model
{
    public class MethodTypeCall
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

    }

    public class IncomingCall : MethodTypeCall
    {

    }

    public class OutgoingCall : MethodTypeCall
    {

    }
}
