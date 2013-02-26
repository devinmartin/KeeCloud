using KeePassLib;
using System;
using System.Collections.Generic;

namespace KeeCloud.Utilities
{
    public static class DatabaseHelpers
    {
        public static IEnumerable<PwGroup> GetAllGroups(this PwDatabase database)
        {
            return Flatten(database.RootGroup, _ => _.Groups);
        }

        public static IEnumerable<T> Flatten<T>(T node, Func<T, IEnumerable<T>> getSubEnumerable)
        {
            yield return node;

            if (getSubEnumerable != null)
            {
                foreach (var subNode in getSubEnumerable(node))
                    yield return subNode;
            }
        }
    }
}
