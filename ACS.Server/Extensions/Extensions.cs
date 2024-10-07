using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

internal static class Extensions
{
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        Type dgvType = dgv.GetType();
        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
            BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(dgv, setting, null);
    }

    public static string GetFullMessage(this Exception ex)
    {
        return ex.InnerException == null
                ? ex.Message
                : ex.Message + " --> " + ex.InnerException.GetFullMessage();
    }

    public static BindingList<T> ToBindingList<T>(this IList<T> source)
    {
        return new BindingList<T>(source);
    }

}
