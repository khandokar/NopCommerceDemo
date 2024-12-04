using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace BSSNop.PlugIn.Widgets.Banner.Models;
public record BannerSearchModel : BaseSearchModel
{
    #region Ctor
    public BannerSearchModel()
    {
        AvailableStores = new List<SelectListItem>();
    }
    #endregion

    #region Properties
    [NopResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.List.SearchStore")]
    public int SearchStoreId { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }
    #endregion
}
