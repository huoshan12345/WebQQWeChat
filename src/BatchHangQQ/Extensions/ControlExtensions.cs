using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iQQ.Net.BatchHangQQ.Extensions
{
    public static class ControlExtensions
    {
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            // See Update 2 for edits Mike de Klerk suggests to insert here.

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static int FindFirstItemIndex(this ListView lv, string cellText, int[] subItemIndexes)
        {
            foreach (ListViewItem item in lv.Items)
            {
                if (subItemIndexes.Any(subItemIndex => item.SubItems[subItemIndex].Text == cellText))
                {
                    return item.Index;
                }
            }
            return -1;
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
