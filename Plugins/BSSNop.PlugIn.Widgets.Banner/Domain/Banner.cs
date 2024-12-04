using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace BSSNop.PlugIn.Widgets.Banner.Domain
{
    public partial class Banner : BaseEntity
    {
        public int PictureId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string NavigationUrl { get; set; }
        public bool Visibility { get; set; }
        //public IList<string> WidgetZones { get; set; }
        //public string SelectedWidgetZone { get; set; }
        
    }
}
