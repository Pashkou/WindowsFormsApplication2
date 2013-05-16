using Microsoft.master2.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Company.Master2.messagebus
{
    class EntityMessageUpdate
    {
        TinyMessengerHub messageHub;
        RuleEngine ruleEngine;

        public EntityMessageUpdate(TinyMessengerHub messageHub, RuleEngine ruleEngine)
        {
            this.messageHub = messageHub;
            this.ruleEngine = ruleEngine;
        }

        public void subscibe()
        {
            messageHub.Subscribe<EntityMessage>((m) =>
            {
                ruleEngine.updateEntity(m.Entity);
            });
        }
    }
}
