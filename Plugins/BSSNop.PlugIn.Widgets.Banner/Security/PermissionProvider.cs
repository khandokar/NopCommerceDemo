using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace BSSNop.PlugIn.Widgets.Banner.Security;
public class PermissionProvider : IPermissionProvider
{
    public static readonly PermissionRecord ManageBanners = new PermissionRecord
    {
        Name = "Admin area. Manage Banners",
        SystemName = "ManageBanners",
        Category = "Widgets"
    };
    public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
    {
        return new HashSet<(string systemRoleName, PermissionRecord[] permissions)>
        {
            ("Administrators", new[] { ManageBanners })
        };
    }

    public IEnumerable<PermissionRecord> GetPermissions()
    {
        return new[]
         {
            ManageBanners
        };
    }
}
