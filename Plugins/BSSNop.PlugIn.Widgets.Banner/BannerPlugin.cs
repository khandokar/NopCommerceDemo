using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Infrastructure;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using BSSNop.PlugIn.Widgets.Banner.Components;
using Nop.Services.Cms;
using Nop.Web.Framework.Menu;
using Nop.Services.Security;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using BSSNop.PlugIn.Widgets.Banner.Security;
using Nop.Web.Framework.Infrastructure;
using BSSNop.PlugIn.Widgets.Banner.Services;

namespace BSSNop.PlugIn.Widgets.Banner;
public class BannerPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
{
    protected readonly ILocalizationService _localizationService;
    protected readonly IPictureService _pictureService;
    protected readonly ISettingService _settingService;
    protected readonly IWebHelper _webHelper;
    protected readonly INopFileProvider _fileProvider;
    private readonly IPermissionService _permissionService;
    protected readonly IUrlHelperFactory _urlHelperFactory;
    protected readonly IActionContextAccessor _actionContextAccessor;
    protected readonly IBannerService _bannerService;
    public BannerPlugin(ILocalizationService localizationService,
        IPictureService pictureService,
        ISettingService settingService,
        IWebHelper webHelper,
        INopFileProvider fileProvider,
        IPermissionService permissionService,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IBannerService bannerService)
    {
        _localizationService = localizationService;
        _pictureService = pictureService;
        _settingService = settingService;
        _webHelper = webHelper;
        _fileProvider = fileProvider;
        _permissionService = permissionService;
        _urlHelperFactory = urlHelperFactory;
        _actionContextAccessor = actionContextAccessor;
        _bannerService = bannerService;
    }

    public override async Task InstallAsync()
    {
        //pictures
        var sampleImagesPath = _fileProvider.MapPath("~/Plugins/Widgets.Banner/Content/banner/sample-images/");

        ////settings
        var settings = new BannerSettings
        {
            PictureId = (await _pictureService.InsertPictureAsync(await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "banner_01.webp")), MimeTypes.ImageWebp, "banner_1")).Id,
            Title = "",
            Url = _webHelper.GetStoreLocation(),
        };
        await _settingService.SaveSettingAsync(settings);

        //await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        //{
        //    ["Plugins.Widgets.Banner.Picture"] = "Picture",
        //    ["Plugins.Widgets.Banner.Picture.Hint"] = "Upload picture.",
        //    ["Plugins.Widgets.Banner.NavigationUrl"] = "Navigation URL",
        //    ["Plugins.Widgets.Banner.NavigationUrl.Hint"] = "Ridirect it to a certain page when click",
        //    ["Plugins.Widgets.Banner.Url"] = "URL",
        //    ["Plugins.Widgets.Banner.Url.Hint"] = "Enter URL. Leave empty if you don't want this picture to be clickable.",
        //    ["Plugins.Widgets.Banner.Title"] = "Banner Title",
        //    ["Plugins.Widgets.Banner.Title.Hint"] = "Enter a title of the banner."

        //});

        await _permissionService.InstallPermissionsAsync(new PermissionProvider());
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Admin.Banners.Manage"] = "Manage Admin Banners"
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        ////settings
        await _settingService.DeleteSettingAsync<BannerSettings>();

        await _permissionService.UninstallPermissionsAsync(new PermissionProvider());

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.Banner");
        await _localizationService.DeleteLocaleResourcesAsync(new[] { "Admin.Banners.Manage", });

        await base.UninstallAsync();
    }

    public async Task<IList<string>> GetWidgetZonesAsync()
    {
        //return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBottom });
        //var settings = await _settingService.LoadSettingAsync<BannerSettings>();
        //return new List<string> { settings.SelectedWidgetZone };

        var banners = await _bannerService.GetBanners();
        var zones = banners.Select(b => b.SelectedWidgetZone).ToList();
        return zones;

        //return new List<string> { "home_page_top" };
    }

    public override string GetConfigurationPageUrl()
    {
        //return _webHelper.GetStoreLocation() + "Admin/WidgetBanner/Configure";
        return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(BannerDefaults.ConfigurationRouteName);
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(WidgetBannerViewComponent);
    }

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    public async Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        if (await _permissionService.AuthorizeAsync(PermissionProvider.ManageBanners))
        {
            var menuItem = new SiteMapNode
            {
                Title = "BSS Plugin",
                Visible = true,
                IconClass = "fas fa-banners",
                ChildNodes = [new SiteMapNode
                    {
                        Title = "Banner",
                        Url = "/Admin/WidgetBanner/List",
                        Visible = true,
                        IconClass = "far fa-circle"
                    }]
            };
            rootNode.ChildNodes.Add(menuItem);
        }

    }

}
