using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSNop.PlugIn.Widgets.Banner.Domain;
using Nop.Core.Caching;
using Nop.Core;
using Nop.Data;

namespace BSSNop.PlugIn.Widgets.Banner.Services;
public class BannerService : IBannerService
{
    private readonly IRepository<HomepageBanner> _homepageBanner;
    private readonly IStaticCacheManager _staticCacheManager;
    public BannerService(IRepository<HomepageBanner> homepageBanner, IStaticCacheManager staticCacheManager)
    {
        _homepageBanner = homepageBanner;
        _staticCacheManager = staticCacheManager;
    }

    public async Task InsertBanner(HomepageBanner homepageBanner)
    {
        await _homepageBanner.InsertAsync(homepageBanner);
        await _staticCacheManager.RemoveAsync(new CacheKey("Nop.homepages.custom.banner"));
    }

    public async Task UpdateBanner(HomepageBanner homepageBanner)
    {
        await _homepageBanner.UpdateAsync(homepageBanner);
        await _staticCacheManager.RemoveAsync(new CacheKey("Nop.homepages.custom.banner"));
    }
    public async Task DeleteBanner(HomepageBanner homepageBanner)
    {
        await _homepageBanner.DeleteAsync(homepageBanner);
        await _staticCacheManager.RemoveAsync(new CacheKey("Nop.homepages.custom.banner"));
    }

    public async Task DeleteBannersAsync(IList<HomepageBanner> homepageBanners)
    {
        if (homepageBanners == null)
            throw new ArgumentNullException(nameof(homepageBanners));
        foreach (var homepageBanner in homepageBanners)
            await _homepageBanner.DeleteAsync(homepageBanner);
    }

    public async Task<IList<HomepageBanner>> GetBannerByIdsAsync(int[] homepageBannerIds)
    {
        return await _homepageBanner.GetByIdsAsync(homepageBannerIds, includeDeleted: false);
    }

    public async Task<IPagedList<HomepageBanner>> GetBanners(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    {
        var query = await _homepageBanner.GetAllAsync(query =>
        {
            if (!showHidden)
                query = query.Where(b => b.Visibility);
            query = query.OrderByDescending(b => b.Id);
            return query;
        });
        return new PagedList<HomepageBanner>(query, pageIndex, pageSize);
    }

    public async Task<HomepageBanner> HomepageBannerId(int Id)
    {
        return await _homepageBanner.GetByIdAsync(Id);
    }

}
