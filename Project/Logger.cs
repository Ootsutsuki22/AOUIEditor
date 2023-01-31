using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    public static class Logger
    {
        public static OutputForm output;
        public static int ErrorCount { get; set; } = 0;

        public static void Log(string message, Color? color = null)
        {
            if (output == null)
                return;
            output.Log(message, color);
        }

        public static void LogError(string message, Color? color = null)
        {
            ErrorCount++;
            if (output == null)
                return;
            output.Log("[ ERROR ] " + message, color);
        }

        public static void Clear()
        {
            ErrorCount = 0;
            if (output == null)
                return;
            output.Clear();
        }
    }
}
