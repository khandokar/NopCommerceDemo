using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Core;
using Nop.Services.Media;
using Nop.Services.Messages;
//using Nop.Services.PublicHomepageDiscountBanner.Extension;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
//using Nop.Web.Areas.Admin.Factories.Extension;
//using Nop.Web.Areas.Admin.Models.Extension.HomepageDiscountBannerAdmin;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc;
using BSSNop.PlugIn.Widgets.Banner.Services;
using BSSNop.PlugIn.Widgets.Banner.Factories;
using BSSNop.PlugIn.Widgets.Banner.Models;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework;
using BSSNop.PlugIn.Widgets.Banner.Security;
using Nop.Services.Configuration;

namespace BSSNop.PlugIn.Widgets.Banner.Controller;
public class WidgetBannerController : BaseAdminController
{
    private readonly IBannerService _bannerService;
    private readonly IPermissionService _permissionService;
    private readonly IBannerFactory _bannerFactory;
    private readonly IPictureService _pictureService;
    private readonly INotificationService _notificationService;
    private readonly IStaticCacheManager _staticCacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
    protected readonly IWorkContext _workContext;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    public WidgetBannerController(IBannerService bannerService, IPermissionService permissionService, IBannerFactory bannerFactory, IPictureService pictureService, INotificationService notificationService, IStaticCacheManager staticCacheManager, IWorkContext workContext, ISettingService settingService,
        IStoreContext storeContext)
    {
        _bannerService = bannerService;
        _permissionService = permissionService;
        _bannerFactory = bannerFactory;
        _pictureService = pictureService;
        _notificationService = notificationService;
        _staticCacheManager = staticCacheManager;
        _workContext = workContext;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    public async Task<IActionResult> Configure()
    {
        //if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBanners))
        //    return AccessDeniedView();
        var model = new BannerModel();

        return View("~/Plugins/Widgets.Banner/Views/BannerList.cshtml");
    }
    public virtual async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBanners))
            return AccessDeniedView();
        BannerSearchModel model = new BannerSearchModel();
        return View("~/Plugins/Widgets.Banner/Views/BannerList.cshtml", model);
    }
    [HttpPost]
    public virtual async Task<IActionResult> List(BannerSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBanners))
            return AccessDeniedView();
        //prepare model
        var model = await _bannerFactory.PrepareBannerListModel(searchModel);
        return Json(model);
    }
    public virtual async Task<IActionResult> Create()
    {
        if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBanners))
            return AccessDeniedView();
        BannerModel model = new BannerModel();
       

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var bannerSettings = await _settingService.LoadSettingAsync<BannerSettings>(storeScope);
        model.WidgetZones = await GetWidgetZonesAsync();
        model.SelectedWidgetZone = bannerSettings.SelectedWidgetZone;
        //model.Title = bannerSettings.Title;
        //model.Visibility = bannerSettings.Visibility;
        //bannerSettings.PictureId = model.PictureId;
        //if (storeScope > 0)
        //{
        //    model.PictureId_OverrideForStore = await _settingService.SettingExistsAsync(bannerSettings, x => x.PictureId, storeScope);
        //    model.Title_OverrideForStore = await _settingService.SettingExistsAsync(bannerSettings, x => x.Title, storeScope);
        //    model.SelectedWidgetZones_OverrideForStore = await _settingService.SettingExistsAsync(bannerSettings, x => x.SelectedWidgetZone, storeScope);
        //    model.Visibility_OverrideForStore = await _settingService.SettingExistsAsync(bannerSettings, x => x.Visibility, storeScope);

        //}


        return View("~/Plugins/Widgets.Banner/Views/BannerCreate.cshtml", model);
    }
    [HttpPost]
    public virtual async Task<IActionResult> Create(BannerModel model)
    {
        model.Url = await _pictureService.GetPictureUrlAsync(model.PictureId);

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var bannerSettings = await _settingService.LoadSettingAsync<BannerSettings>(storeScope);
        //model.SelectedWidgetZone = bannerSettings.SelectedWidgetZone;
        //model.WidgetZones = bannerSettings.WidgetZones;
        bannerSettings.Title = model.Title;
        bannerSettings.WidgetZones = await GetWidgetZonesAsync();
        bannerSettings.SelectedWidgetZone = model.SelectedWidgetZone;
        bannerSettings.Visibility = model.Visibility;



        //var previousPictureIds = new[]
        //{
        //    bannerSettings.PictureId
        //};
        //bannerSettings.PictureId = model.PictureId;
        model.Serial = DateTime.Now.ToString("yyyyMMddHHmmss");
        await _bannerFactory.InsertBanner(model);

        string picturIdKey = _settingService.GetSettingKey(bannerSettings, x => x.PictureId);
        picturIdKey = picturIdKey + model.Serial;
        await _settingService.SetSettingAsync(picturIdKey, model.PictureId, storeScope, false);

        string titleKey = _settingService.GetSettingKey(bannerSettings, x => x.Title);
        titleKey = titleKey + model.Serial;
        await _settingService.SetSettingAsync(titleKey, model.Title, storeScope, false);

        string selectedWidgetZoneKey = _settingService.GetSettingKey(bannerSettings, x => x.SelectedWidgetZone);
        selectedWidgetZoneKey = selectedWidgetZoneKey + model.Serial;
        await _settingService.SetSettingAsync(selectedWidgetZoneKey, model.SelectedWidgetZone, storeScope, false);

        string visibilityKey = _settingService.GetSettingKey(bannerSettings, x => x.Visibility);
        visibilityKey = visibilityKey + model.Serial;
        await _settingService.SetSettingAsync(visibilityKey, model.Visibility, storeScope, false);


        //await _settingService.SaveSettingOverridablePerStoreAsync(bannerSettings, x => x.PictureId, model.PictureId_OverrideForStore, storeScope, false);
        //await _settingService.SaveSettingOverridablePerStoreAsync(bannerSettings, x => x.Title, model.Title_OverrideForStore, storeScope, false);
        //await _settingService.SaveSettingOverridablePerStoreAsync(bannerSettings, x => x.SelectedWidgetZone, model.SelectedWidgetZones_OverrideForStore, storeScope, false);
        //await _settingService.SaveSettingOverridablePerStoreAsync(bannerSettings, x => x.Visibility, model.Visibility_OverrideForStore, storeScope, false);

        _notificationService.SuccessNotification("Banner Has Been Created");
        return RedirectToAction("List");
    }

    public virtual async Task<IActionResult> Edit(int Id)
    {
        if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBanners))
            return AccessDeniedView();
        var model = await _bannerFactory.GetBannerById(Id);
        return View(model);
    }
    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Edit(BannerModel model, bool continueEditing)
    {
        await _bannerFactory.UpdateBanner(model);
        _notificationService.SuccessNotification("Banner Has Been Updated");
        return RedirectToAction("Edit", new { Id = model.Id });
    }
    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a manufacturer with the specified id
        var homepageBanner = await _bannerService.HomepageBannerId(id);
        if (homepageBanner == null)
            return RedirectToAction("List");
        await _bannerService.DeleteBanner(homepageBanner);
        //await _staticCacheManager.RemoveByPrefixAsync("Nop.homepages.custom.sliderquery");
        _notificationService.SuccessNotification("Banner Has Been Deleted");
        return new NullJsonResult();
    }
    [HttpPost]
    public virtual async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
    {
        //try to get a manufacturer with the specified id
        var homepageBanner = await _bannerService.GetBannerByIdsAsync(selectedIds.ToArray());
        if (homepageBanner == null)
            return RedirectToAction("List");
        await _bannerService.DeleteBannersAsync(homepageBanner);
        await _staticCacheManager.RemoveByPrefixAsync("Nop.homepages.custom.sliderquery");
        return new NullJsonResult();
    }
    //public async Task<IList<string>> GetWidgetZonesAsync()
    //{
    //    return await Task.FromResult<IList<string>>(new List<string>
    //        {
    //          PublicWidgetZones.HomepageTop,
    //          PublicWidgetZones.HomepageBeforeCategories,
    //          PublicWidgetZones.HomepageBeforeProducts,
    //          PublicWidgetZones.HomepageBeforeBestSellers,
    //          PublicWidgetZones.HomepageBeforeNews,
    //          PublicWidgetZones.HomepageBeforePoll,
    //          PublicWidgetZones.HomepageBottom
    //        });
    //}

    public async Task<IList<string>> GetWidgetZonesAsync()
    {
        return new List<string>
    {
        PublicWidgetZones.HomepageTop,
        PublicWidgetZones.HomepageBeforeCategories,
        PublicWidgetZones.HomepageBeforeProducts,
        PublicWidgetZones.HomepageBeforeBestSellers,
        PublicWidgetZones.HomepageBeforeNews,
        PublicWidgetZones.HomepageBeforePoll,
        PublicWidgetZones.HomepageBottom
    };
    }


    //[HttpPost]
    //public async Task<IActionResult> Configure(BannerModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
    //        return AccessDeniedView();




    //    //_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

    //    return await Configure();
    //}

}
