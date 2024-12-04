using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace BSSNop.PlugIn.Widgets.Banner.Infrastracture;
public class PluginViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        // No need to add values here for this use case
    }
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        // Define your custom plugin view path
        var customLocations = new[]
        {
            "/Plugins/Widgets.Banner/Views/{1}/{0}.cshtml",
            "/Plugins/Widgets.Banner/Views/{0}.cshtml",
        };

        // Return custom locations followed by default locations
        return customLocations.Concat(viewLocations);
    }
}

