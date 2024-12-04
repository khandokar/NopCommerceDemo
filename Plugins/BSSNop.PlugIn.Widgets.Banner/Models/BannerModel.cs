using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BSSNop.PlugIn.Widgets.Banner.Models;
public record BannerModel : BaseNopEntityModel
{
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.Banner.Picture")]
    [UIHint("Picture")]
    public int PictureId { get; set; }
    public bool PictureId_OverrideForStore { get; set; }
    [NopResourceDisplayName("Plugins.Widgets.Banner.Url")]
    public string Url { get; set; }
    public bool Url_OverrideForStore { get; set; }
    [NopResourceDisplayName("Plugins.Widgets.Banner.NavigationUrl")]
    public string NavigationUrl { get; set; }
    public bool NavigationUr_OverrideForStore { get; set; }
    [NopResourceDisplayName("Plugins.Widgets.Banner.Title")]
    public string Title { get; set; }
    public bool Title_OverrideForStore { get; set; }
    public bool Visibility { get; set; }
    public bool Visibility_OverrideForStore { get; set; }

    public IList<string> WidgetZones { get; set; }

    public string SelectedWidgetZone { get; set; }

    public bool WidgetZones_OverrideForStore { get; set; }

    public bool SelectedWidgetZones_OverrideForStore { get; set; }

    public string Serial { get; set; }
}
