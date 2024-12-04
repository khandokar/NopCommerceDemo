using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSNop.PlugIn.Widgets.Banner.Domain;
using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;
using Nop.Data.Migrations;
using Nop.Core.Domain.Media;

namespace BSSNop.PlugIn.Widgets.Banner.Migrations;
[NopMigration("2024/11/20 06:00:00", "Widgets.Banner base schema", MigrationProcessType.Installation)]
public class BannerSchemaInstallationMigration : AutoReversingMigration
{
    public override void Up()
    {

        Create.Table(nameof(HomepageBanner))
            .WithColumn(nameof(HomepageBanner.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(HomepageBanner.PictureId)).AsInt32()
            .WithColumn(nameof(HomepageBanner.Url)).AsString(500).Nullable()
            .WithColumn(nameof(HomepageBanner.NavigationUrl)).AsString(500).Nullable()
            .WithColumn(nameof(HomepageBanner.Title)).AsString(500).Nullable()
            .WithColumn(nameof(HomepageBanner.Visibility)).AsString(500)
            .WithColumn(nameof(HomepageBanner.WidgetZones)).AsString(500).Nullable()
            .WithColumn(nameof(HomepageBanner.SelectedWidgetZone)).AsString(500)
            .WithColumn(nameof(HomepageBanner.Serial)).AsString(500);

    }
}
