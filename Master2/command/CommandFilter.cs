using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.master2
{
    public class CommandFilter : IOleCommandTarget
    {

        public IOleCommandTarget NextCommandTarget
        {
            get;
            set;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            NextCommandTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            return VSConstants.S_OK;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
            {
                switch (nCmdID)
                {
                    case (uint)VSConstants.VSStd2KCmdID.RETURN:
                        MessageBox.Show("enter");
                        break;

                }
            }
            if (pguidCmdGroup == typeof(VSConstants.VSStd97CmdID).GUID)
            {
                switch (nCmdID)
                {
                    case (uint)VSConstants.VSStd97CmdID.GotoDefn:
                        MessageBox.Show("goto");
                        break;
                }

            }



            NextCommandTarget.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            return VSConstants.S_OK;
        }
    }
}
