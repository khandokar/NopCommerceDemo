﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Domain.Configuration;
using Nop.Core.Events;
using Nop.Services.Events;

namespace BSSNop.PlugIn.Widgets.Banner.Infrastracture.Cache;
public class ModelCacheEventConsumer : IConsumer<EntityInsertedEvent<Setting>>,
    IConsumer<EntityUpdatedEvent<Setting>>,
    IConsumer<EntityDeletedEvent<Setting>>
{
    // <summary>
    /// Key for caching
    /// </summary>
    /// <remarks>
    /// {0} : picture id
    /// {1} : connection type (http/https)
    /// </remarks>
    public static CacheKey PICTURE_URL_MODEL_KEY = new("Nop.plugins.widgets.banner.pictureurl-{0}-{1}", PICTURE_URL_PATTERN_KEY);
    public const string PICTURE_URL_PATTERN_KEY = "Nop.plugins.widgets.banner";

    protected readonly IStaticCacheManager _staticCacheManager;

    public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(PICTURE_URL_PATTERN_KEY);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(PICTURE_URL_PATTERN_KEY);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(PICTURE_URL_PATTERN_KEY);
    }
}
