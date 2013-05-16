using Microsoft.master2.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Microsoft.master2.messagebus
{
    public class ClassMessageReceiver
    {
        TinyMessengerHub messageHub;
        RuleEngine ruleEngine;

        public ClassMessageReceiver(TinyMessengerHub messageHub, RuleEngine ruleEngine)
        {
            this.messageHub = messageHub;
            this.ruleEngine = ruleEngine;
        }

        public void subscibe()
        {
            messageHub.Subscribe<ClassMessage>((m) =>
            {
                ruleEngine.classSelected(m.CSharpClass);
            });
        }
    }


}
