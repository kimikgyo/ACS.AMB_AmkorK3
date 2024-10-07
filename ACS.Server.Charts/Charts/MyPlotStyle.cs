using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    internal class MyPlotStyle : ScottPlot.Styles.Monospace
    {
        new public string AxisLabelFontName => "Arial";
        new public string TitleFontName => "Arial";
        new public string TickLabelFontName => "Arial";
    }
}
