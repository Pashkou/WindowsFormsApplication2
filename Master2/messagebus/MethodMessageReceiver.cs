using Microsoft.master2.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Microsoft.master2.messagebus
{

    public class MethodMessageReceiver
    {
        TinyMessengerHub messageHub;
        RuleEngine ruleEngine;

        public MethodMessageReceiver(TinyMessengerHub messageHub, RuleEngine ruleEngine)
        {
            this.messageHub = messageHub;
            this.ruleEngine = ruleEngine;
        }

        public void subscibe()
        {
            messageHub.Subscribe<MethodMessage>((m) =>
            {
                ruleEngine.methodSelected(m.CSharpMethod);
            });
        }
    }
}
