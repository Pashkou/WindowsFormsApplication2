using Microsoft.master2.model;
using Microsoft.master2.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Microsoft.master2.messagebus
{
    class ClassMessage : ITinyMessage
    {
        private CSharpClass cSharpClass;

        public CSharpClass CSharpClass
        {
            get { return cSharpClass; }
            set { cSharpClass = value; }
        }

        public object Sender { get; private set; }

    }

}
