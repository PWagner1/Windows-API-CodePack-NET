// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Windows.Forms;

namespace Transliterator
{
    public class ScrollbarTextBox : TextBox
    {
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;

        public event ScrollEventHandler OnHorizontalScroll = null;
        public event ScrollEventHandler OnVerticalScroll = null;

        private static readonly ScrollEventType[] ScrollEventType = {
            System.Windows.Forms.ScrollEventType.SmallDecrement,
            System.Windows.Forms.ScrollEventType.SmallIncrement,
            System.Windows.Forms.ScrollEventType.LargeDecrement,
            System.Windows.Forms.ScrollEventType.LargeIncrement,
            System.Windows.Forms.ScrollEventType.ThumbPosition,
            System.Windows.Forms.ScrollEventType.ThumbTrack,
            System.Windows.Forms.ScrollEventType.First,
            System.Windows.Forms.ScrollEventType.Last,
            System.Windows.Forms.ScrollEventType.EndScroll
        };

        private ScrollEventType GetEventType(uint wParam) => wParam < ScrollEventType.Length ? ScrollEventType[wParam] : System.Windows.Forms.ScrollEventType.EndScroll;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HSCROLL)
            {
                if (OnHorizontalScroll != null)
                {
                    var wParam = (uint)m.WParam.ToInt32();
                    OnHorizontalScroll(this, new ScrollEventArgs(GetEventType(wParam & 0xffff), (int)(wParam >> 16)));
                }
            }
            else if (m.Msg == WM_VSCROLL)
            {
                if (OnVerticalScroll != null)
                {
                    var wParam = (uint)m.WParam.ToInt32();
                    OnVerticalScroll(this, new ScrollEventArgs(GetEventType(wParam & 0xffff), (int)(wParam >> 16)));
                }
            }
        }
    }
}
