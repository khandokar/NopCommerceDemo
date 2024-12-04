using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSNop.PlugIn.Widgets.Banner.Domain;
using Nop.Core;

namespace BSSNop.PlugIn.Widgets.Banner.Services;
public interface IBannerService
{
    Task InsertBanner(HomepageBanner homepageBannerBanner);
    Task DeleteBanner(HomepageBanner homepageBannerBanner);
    Task DeleteBannersAsync(IList<HomepageBanner> homepageBanners);
    Task UpdateBanner(HomepageBanner homepageBanner);
    Task<HomepageBanner> HomepageBannerId(int Id);
    Task<IList<HomepageBanner>> GetBannerByIdsAsync(int[] homepageBannerIds);
    Task<IPagedList<HomepageBanner>> GetBanners(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
}
