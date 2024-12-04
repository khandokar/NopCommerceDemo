using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSNop.PlugIn.Widgets.Banner.Models;

namespace BSSNop.PlugIn.Widgets.Banner.Factories;
public interface IBannerFactory
{
    Task<BannerListModel> PrepareBannerListModel(BannerSearchModel searchModel);
    Task InsertBanner(BannerModel bannerModel);
    Task<BannerModel> GetBannerById(int id);
    Task UpdateBanner(BannerModel bannerModel);
}
