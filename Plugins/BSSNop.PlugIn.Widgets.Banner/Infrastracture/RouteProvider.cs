using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace BSSNop.PlugIn.Widgets.Banner.Infrastracture;
public class RouteProvider : IRouteProvider
{
    public int Priority => 1000;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
               name: BannerDefaults.ConfigurationRouteName,
               pattern: "Admin/WidgetBanner/Configure",
               defaults: new
               {
                   controller = "WidgetBanner",
                   action = "Configure"
               });

        endpointRouteBuilder.MapControllerRoute(
           name: BannerDefaults.AdminBannerListRouteName,
           pattern: "Admin/WidgetBanner/List",
           defaults: new
           {
               controller = "WidgetBanner",
               action = "List"
           });

        endpointRouteBuilder.MapControllerRoute(
           name: BannerDefaults.AdminBannerCreateRouteName,
           pattern: "Admin/WidgetBanner/Create",
           defaults: new
           {
               controller = "WidgetBanner",
               action = "Create"
           });

        endpointRouteBuilder.MapControllerRoute(
          name: BannerDefaults.AdminBannerCreateRouteName,
          pattern: "Admin/WidgetBanner/Edit{Id}",
          defaults: new
          {
              controller = "WidgetBanner",
              action = "Edit"
          });

        endpointRouteBuilder.MapControllerRoute(
           name: BannerDefaults.AdminBannerDeleteRouteName,
           pattern: "Admin/WidgetBanner/Delete",
           defaults: new
           {
               controller = "WidgetBanner",
               action = "Delete"
           });

        endpointRouteBuilder.MapControllerRoute(
          name: BannerDefaults.AdminBannerDeleteSelectedRouteName,
          pattern: "Admin/WidgetBanner/DeleteSelected",
          defaults: new
          {
              controller = "WidgetBanner",
              action = "DeleteSelected"
          });
    }
}
