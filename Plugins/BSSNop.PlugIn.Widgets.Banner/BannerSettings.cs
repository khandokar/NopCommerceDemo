using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace BSSNop.PlugIn.Widgets.Banner;
public class BannerSettings : ISettings
{
    public int PictureId { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string NavigationUrl { get; set; }
    public bool Visibility { get; set; }
    public IList<string> WidgetZones { get; set; }
    public string SelectedWidgetZone { get; set; }

    // New property to map widget zones to picture IDs
    public Dictionary<string, int> WidgetZoneImages { get; set; } = new Dictionary<string, int>();
}
