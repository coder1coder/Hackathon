using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Hackathon.DAL.Extensions;

public static class EfCoreExtensions
{
    public static IQueryable<TEntity> ConditionalThenInclude<TEntity, TSourceProperty>(
        this IIncludableQueryable<TEntity, TSourceProperty> source,
        Func<IIncludableQueryable<TEntity, TSourceProperty>, IQueryable<TEntity>> transform,
        bool condition) where TEntity : class => condition ? transform(source) : source;
}
