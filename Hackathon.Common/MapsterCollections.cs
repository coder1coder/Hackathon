using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;

namespace Hackathon.Common;

public static class MapsterCollections
{
    public static void MapCollections<TSourceItem, TDestinationItem>(
        this TypeAdapterSetter<IEnumerable<TSourceItem>, ICollection<TDestinationItem>> adapter,
        Func<TSourceItem, TDestinationItem, bool> compare)
        => adapter.MapToTargetWith((sourceCollection, destinationCollection) =>
            Map(sourceCollection, destinationCollection, compare));

    private static ICollection<TDestinationItem> Map<TSourceItem, TDestinationItem>(
        IEnumerable<TSourceItem> sourceList,
        ICollection<TDestinationItem> destinationList,
        Func<TSourceItem, TDestinationItem, bool> compare)
    {
        sourceList ??= Array.Empty<TSourceItem>();
        destinationList ??= new List<TDestinationItem>();

        var toRemoveFromDestination = destinationList
            .Where(x => sourceList.All(z => !compare(z, x)))
            .ToArray();

        foreach (var toRemoveItem in toRemoveFromDestination)
        {
            destinationList.Remove(toRemoveItem);
        }

        foreach (var sourceItem in sourceList)
        {
            var destinationItem = destinationList.FirstOrDefault(x => compare(sourceItem, x));
            if (destinationItem == null)
            {
                destinationList.Add(sourceItem.Adapt<TDestinationItem>());
            }
            else
            {
                sourceItem.Adapt(destinationItem);
            }
        }

        return destinationList;
    }
}
