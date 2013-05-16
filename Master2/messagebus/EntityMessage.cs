using Microsoft.master2.contextmodel;
using Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Company.Master2.messagebus
{
    class EntityMessage : ITinyMessage
    {
        private MyVertexBase entity;

        public MyVertexBase Entity
        {
            get { return entity; }
            set { entity = value; }
        }



        public object Sender { get; private set; }
    }
}
