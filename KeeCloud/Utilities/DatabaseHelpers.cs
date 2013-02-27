using KeePassLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeeCloud.Utilities
{
    public static class DatabaseHelpers
    {
        public static IEnumerable<PwGroup> GetAllGroups(this PwDatabase database)
        {
            return Flatten<PwGroup>(database.RootGroup, node => node.Groups);
        }

        public static IEnumerable<PwEntry> GetAllPasswords(this PwDatabase database)
        {
            return from g in database.GetAllGroups()
                   from pe in g.Entries
                   select pe;
        }

        public static IEnumerable<T> Flatten<T>(T node, Func<T, IEnumerable<T>> getSubEnumerable)
        {
            yield return node;
            if (getSubEnumerable != null)
            {
                foreach (var subNode in getSubEnumerable(node))
                {
                    yield return subNode;
                    foreach (var subSubNode in Flatten<T>(subNode, getSubEnumerable))
                        yield return subSubNode;
                }
            }
            else
                throw new Exception();
        }

        public static bool EntryStringEquals(this PwEntry entry, string key, string expectedValue)
        {
            return entry.EntryStringExistsAndContidtionMet(key, value =>
            {
                return value.Equals(expectedValue, StringComparison.InvariantCultureIgnoreCase);
            });
        }

        public static bool EntryStringNotNullOrEmpty(this PwEntry entry, string key)
        {
            return entry.EntryStringExistsAndContidtionMet(key, value => !string.IsNullOrEmpty(value));
        }

        public static bool EntryStringExistsAndContidtionMet(this PwEntry entry, string key, Func<string, bool> expectation)
        {
            if (expectation == null)
                return false;
            else
            {
                var value = entry.Strings.Get(key);
                if (value != null && !value.IsEmpty)
                {
                    var rawValue = value.ReadString();
                    return expectation(rawValue);
                }
                else
                    return false;
            }
        }
    }
}
