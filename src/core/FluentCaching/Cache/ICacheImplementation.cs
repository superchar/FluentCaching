﻿using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache;

public interface ICacheImplementation
{
    ValueTask<TEntity> RetrieveAsync<TEntity>(string key);

    ValueTask CacheAsync<TEntity>(string key, TEntity targetObject, CacheOptions options);

    ValueTask RemoveAsync(string key);
}