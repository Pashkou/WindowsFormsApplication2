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
    class MethodMessage : ITinyMessage
    {
        private CSharpMethod cSharpMethod;

        public CSharpMethod CSharpMethod
        {
            get { return cSharpMethod; }
            set { cSharpMethod = value; }
        }

        public object Sender { get; private set; }

    }
}
