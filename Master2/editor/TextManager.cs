using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2
{
    class TextManager : IVsTextManagerEvents
    {
        public void OnRegisterView(IVsTextView pView)
        {
            CommandFilter filter = new CommandFilter();
            IOleCommandTarget nextCommandTarget;
            pView.AddCommandFilter(filter, out nextCommandTarget);
            filter.NextCommandTarget = nextCommandTarget;
        }

        public void OnRegisterMarkerType(int iMarkerType)
        {
        }

        public void OnUnregisterView(IVsTextView pView) { }
        public void OnUserPreferencesChanged(VIEWPREFERENCES[] pViewPrefs, FRAMEPREFERENCES[] pFramePrefs, LANGPREFERENCES[] pLangPrefs, FONTCOLORPREFERENCES[] pColorPrefs)
        {
        }
    }
}
