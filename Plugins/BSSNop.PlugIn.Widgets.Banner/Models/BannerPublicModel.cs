using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace BSSNop.PlugIn.Widgets.Banner.Models;
public record  BannerPublicModel : BaseNopEntityModel
{
    public BannerPublicModel()
    {
       bannerImageModels = new List<BannerImageModel>();
    }
    public List<BannerImageModel> bannerImageModels { get; set; }

    public record BannerImageModel : BaseNopEntityModel
    {
        public string PictureUrl { get; set; }
        public string ImageTitle { get; set; }
        public string NavigationUrl { get; set; }
    }

    public string PictureUrl { get; set; }
    public string NavigationUrl { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }

}
