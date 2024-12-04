using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSNop.PlugIn.Widgets.Banner.Domain;
using BSSNop.PlugIn.Widgets.Banner.Models;
using BSSNop.PlugIn.Widgets.Banner.Services;
using ExCSS;
using Nop.Core.Caching;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace BSSNop.PlugIn.Widgets.Banner.Factories;
public class BannerFactory : IBannerFactory
{
    private readonly IBannerService _bannerService;
    private readonly IStaticCacheManager _cacheManager;
    private readonly IPictureService _pictureService;
    public BannerFactory(IBannerService bannerService, IStaticCacheManager cacheManager, IPictureService pictureService)
    {
        _bannerService = bannerService;
        _cacheManager = cacheManager;
        _pictureService = pictureService;
    }

    public async Task InsertBanner(BannerModel bannerModel)
    {
        var domain = bannerModel.ToEntity<HomepageBanner>();
        await _bannerService.InsertBanner(domain);
    }

    public async Task<BannerModel> GetBannerById(int id)
    {
        var bannerDomain = await _bannerService.HomepageBannerId(id);
        var bannerModel = bannerDomain.ToModel<BannerModel>();
        return bannerModel;
    }

    public async Task<BannerListModel> PrepareBannerListModel(BannerSearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));
        //get manufacturers
        var homepageBanners = await _bannerService.GetBanners(showHidden: true,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);
        var model = await new BannerListModel().PrepareToGridAsync(searchModel, homepageBanners, () =>
        {
            var bannerModel = homepageBanners.SelectAwait(async homepageBanner =>
            {
                var imgUrl = await _pictureService.GetPictureUrlAsync(homepageBanner.PictureId, 200);
                BannerModel bannerModel = new BannerModel();
                var homepageBannerModel = homepageBanner.ToModel(bannerModel);
                homepageBannerModel.Url = imgUrl;
                
                return homepageBannerModel;
            });
            return bannerModel;
        });
        return model;
    }

    public async Task UpdateBanner(BannerModel bannerModel)
    {
        var bannerUpdate = bannerModel.ToEntity<HomepageBanner>();
        var imgUrl = await _pictureService.GetPictureUrlAsync(bannerModel.PictureId);
        bannerUpdate.Url = imgUrl;
        bannerUpdate.Title = bannerModel.Title;
        bannerUpdate.Visibility = bannerModel.Visibility;
        bannerUpdate.WidgetZones = bannerModel.WidgetZones;
        bannerUpdate.SelectedWidgetZone = bannerModel.SelectedWidgetZone;
        await _bannerService.UpdateBanner(bannerUpdate);
    }
}
