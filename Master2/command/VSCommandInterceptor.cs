using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;

namespace Microsoft.master2
{
    public class VSCommandInterceptor : IDisposable
    {
        private IServiceProvider serviceProvider;
        private Guid guid;
        private int id;
        private bool isDisposed;

        public VSCommandInterceptor(IServiceProvider serviceProvider, Guid commandGuid, int commandId)
        {
            this.serviceProvider = serviceProvider;
            this.guid = commandGuid;
            this.id = commandId;

            if (CommandEvents != null)
            {
                CommandEvents.AfterExecute += new _dispCommandEvents_AfterExecuteEventHandler(OnAfterExecute);
                CommandEvents.BeforeExecute += new _dispCommandEvents_BeforeExecuteEventHandler(OnBeforeExecute);
            }
        }

        public event EventHandler<EventArgs> AfterExecute;
        public event EventHandler<EventArgs> BeforeExecute;

        private CommandEvents commandEvents;
        protected CommandEvents CommandEvents
        {
            get
            {
                if (commandEvents == null)
                {
                    DTE dte = this.serviceProvider.GetService(typeof(DTE)) as DTE;

                    if (dte != null)
                    {
                        commandEvents = dte.Events.get_CommandEvents(guid.ToString("B"), id) as CommandEvents;
                    }
                }

                return commandEvents;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                if (CommandEvents != null)
                {
                    CommandEvents.AfterExecute -= OnAfterExecute;
                    CommandEvents.BeforeExecute -= OnBeforeExecute;
                }

                this.isDisposed = true;
            }
        }

        private void OnAfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            if (AfterExecute != null)
            {
                AfterExecute(this, new EventArgs());
            }
        }

        private void OnBeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            if (BeforeExecute != null)
            {
                BeforeExecute(this, new EventArgs());
            }
        }
    }
}
