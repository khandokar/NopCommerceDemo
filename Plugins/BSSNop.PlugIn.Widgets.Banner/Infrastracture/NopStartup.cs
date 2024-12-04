using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSNop.PlugIn.Widgets.Banner.Factories;
using BSSNop.PlugIn.Widgets.Banner.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Web.Areas.Admin.Factories;

namespace BSSNop.PlugIn.Widgets.Banner.Infrastracture;
public class NopStartup :INopStartup
{
    public void Configure(IApplicationBuilder application)
    {
        application.UseRouting();
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IBannerFactory, BannerFactory>();
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new PluginViewLocationExpander());
        });
    }

    public int Order => 100;
}
