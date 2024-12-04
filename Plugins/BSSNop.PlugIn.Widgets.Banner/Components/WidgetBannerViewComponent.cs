using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Caching;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
using BSSNop.PlugIn.Widgets.Banner.Models;
using BSSNop.PlugIn.Widgets.Banner.Infrastracture.Cache;
using BSSNop.PlugIn.Widgets.Banner.Services;
using static BSSNop.PlugIn.Widgets.Banner.Models.BannerPublicModel;

namespace BSSNop.PlugIn.Widgets.Banner.Components;
[ViewComponent(Name = "WidgetsBannerViewComponent")]
public class WidgetBannerViewComponent : NopViewComponent
{
    protected readonly IStoreContext _storeContext;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly ISettingService _settingService;
    protected readonly IPictureService _pictureService;
    protected readonly IBannerService _bannerService;
    protected readonly IWebHelper _webHelper;


    public WidgetBannerViewComponent(IStoreContext storeContext,
        IStaticCacheManager staticCacheManager,
        ISettingService settingService,
        IPictureService pictureService,
        IBannerService bannerService,
        IWebHelper webHelper)
    {
        _storeContext = storeContext;
        _staticCacheManager = staticCacheManager;
        _settingService = settingService;
        _pictureService = pictureService;
        _bannerService = bannerService;
        _webHelper = webHelper;
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var banners = await _bannerService.GetBanners();
        var bannersByWidgetZone = banners.Where(b => b.SelectedWidgetZone == widgetZone);
        var bannerPublicModels = new List<BannerPublicModel>();
        var bannerSetting = new BannerSettings();
        foreach (var banner in bannersByWidgetZone)
        {
            var bannerPublicModel = new BannerPublicModel();
            string picturIdKey = _settingService.GetSettingKey(bannerSetting, x => x.PictureId);
            picturIdKey = picturIdKey + banner.Serial;
            var pictureValue = _settingService.GetSetting(picturIdKey).Value;
            bannerPublicModel.PictureUrl =  await GetPictureUrlAsync(Convert.ToInt32(pictureValue));

            string titleKey = _settingService.GetSettingKey(bannerSetting, x => x.Title);
            titleKey = titleKey + banner.Serial;
            var titleValue = _settingService.GetSetting(titleKey).Value;
            bannerPublicModel.Title = titleValue;

            bannerPublicModel.NavigationUrl = banner.NavigationUrl;
            bannerPublicModels.Add(bannerPublicModel);
            
        }

        //var bannerSettings = await _settingService.LoadSettingAsync<BannerSettings>(store.Id);
        //var model = new BannerPublicModel
        //{
        //    PictureUrl = await GetPictureUrlAsync(bannerSettings.PictureId),
        //    Title = bannerSettings.Title,
        //    NavigationUrl = bannerSettings.NavigationUrl
        //};
        //if (string.IsNullOrEmpty(model.PictureUrl))
        //    //no pictures uploaded
        //    return Content("");

        return View("~/Plugins/Widgets.Banner/Views/BannerPublicInfo.cshtml", bannerPublicModels);

    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected async Task<string> GetPictureUrlAsync(int pictureId)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId, _webHelper.IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

        return await _staticCacheManager.GetAsync(cacheKey, async () => {
            //little hack here. nulls aren't cacheable so set it to ""
            var url = await _pictureService.GetPictureUrlAsync(pictureId, showDefaultPicture: false) ?? "";
            return url;
        });

        _staticCacheManager.RemoveAsync(cacheKey);
    }
}
