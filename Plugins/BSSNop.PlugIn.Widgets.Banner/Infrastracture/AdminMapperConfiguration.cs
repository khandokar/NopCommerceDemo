using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BSSNop.PlugIn.Widgets.Banner.Domain;
using BSSNop.PlugIn.Widgets.Banner.Models;
using Nop.Core.Infrastructure.Mapper;
using Nop.Services.Cms;

namespace BSSNop.PlugIn.Widgets.Banner.Infrastracture;
public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
{
    public AdminMapperConfiguration()
    {
        CreateBssBanner();
    }

    protected virtual void CreateBssBanner()
    {
        CreateMap<HomepageBanner, BannerModel>();
        CreateMap<BannerModel, HomepageBanner>();
    }
    public int Order => 0;
}
