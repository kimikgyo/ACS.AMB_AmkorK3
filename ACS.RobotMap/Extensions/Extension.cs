using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

internal static class Extension
{
    public static string GetFullMessage(this Exception ex)
    {
        return ex.InnerException == null
                ? ex.Message
                : ex.Message + " --> " + ex.InnerException.GetFullMessage();
    }


    public static void DoubleBuffered(this Control control, bool setting)
    {
        PropertyInfo pi = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(control, setting, null);
    }

}
