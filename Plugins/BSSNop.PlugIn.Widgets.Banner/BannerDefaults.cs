using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSSNop.PlugIn.Widgets.Banner;
public class BannerDefaults 
{
    public static string ConfigurationRouteName => "Plugin.Widgets.Banner.Configure";
    public static string AdminBannerListRouteName => "Plugin.Widgets.Banner.List";
    public static string AdminBannerEditRouteName => "Plugin.Widgets.Banner.Edit";
    public static string AdminBannerCreateRouteName => "Plugin.Widgets.Banner.Create";
    public static string AdminBannerUpdateRouteName => "Plugin.Widgets.Banner.Update";
    public static string AdminBannerDeleteRouteName => "Plugin.Widgets.Banner.Delete";
    public static string AdminBannerDeleteSelectedRouteName => "Plugin.Widgets.Banner.DeleteSelected";

    //Admin Widget Zone
    public static string BannerListButtons => "banner_list_buttons";
}
