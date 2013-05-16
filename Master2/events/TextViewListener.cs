using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Master2.events
{
    class TextViewListener : IVsTextViewEvents
    {
        #region IVsTextViewEvents Members

        public void OnChangeCaretLine(IVsTextView pView, int iNewLine, int iOldLine)
        {
            int t = 0;
        }

        public void OnChangeScrollInfo(IVsTextView pView, int iBar, int iMinUnit, int iMaxUnits, int iVisibleUnits, int iFirstVisibleUnit)
        {
        }

        public void OnKillFocus(IVsTextView pView)
        {
        }

        public void OnSetBuffer(IVsTextView pView, IVsTextLines pBuffer)
        {
        }

        public void OnSetFocus(IVsTextView pView)
        {
        }

        #endregion
    }
}
