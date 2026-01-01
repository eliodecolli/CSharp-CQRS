using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace BeeGees_ReadNode.Facade.FastAccess
{
    public class FastAccess
    {
        private CacheNode rootNode;

        public FastAccess()
        {
            rootNode = new CacheNode();
            Log.Debug(" [x] FastAccess is now ready! (And probably very much memory-hungry)");
        }

        public void RegisterQuery(CachedQueryString query, object value, bool replace = true)
        {
            //START/{SENDER}/{REQUEST_TYPE}/{PARAM_NAME}={PARAM_VALUE}/.../.../END

            // better to use RegEx; but I suck at RegEx :(

            string[] splits = query.QueryString.Split('/');
            if(splits.First().ToLower() != "start"
                || splits.Last().ToLower() != "end")
            {
                Log.Error(" [x] Invalid query string " + query.QueryString);
                return;
            }

            // now start searching
            if(rootNode != null)
            {
                CacheNode lastChildren = rootNode;

                for (int i = 1; i < splits.Length - 1; i++)
                {
                    // get to the bottom leaf
                    if(lastChildren.Children.ContainsKey(splits[i]))
                    {
                        lastChildren = lastChildren.Children[splits[i]];
                        continue;
                    }

                    // when it gets here that IF above just consumes precious computing energy
                    lastChildren.Children.Add(splits[i], new CacheNode(splits[i], lastChildren));  // we just populate it
                    lastChildren = lastChildren.Children[splits[i]];
                }
                if (replace)
                    lastChildren.SetData(value);
                else
                    lastChildren.AddData(value);
            }
        }

        private List<object> RecursiveSearch(IEnumerable<object> list, string name, object val, Type type)
        {
            var retval = new List<object>();

            foreach (var x in list)
            {
                if (x is IEnumerable<object>)  // treat it as a data...
                {
                    retval.AddRange(RecursiveSearch((IEnumerable<object>)x, name, val, type));
                }
                else
                {
                    foreach (var member in type.GetProperties())
                    {
                        if (member.Name.ToLower() == name.ToLower() &&  // not case-sensitive
                                    member.GetValue(x).Equals(val))
                        {
                            retval.Add(x);
                        }
                    }
                }
            }

            return retval;
        }

        private int RecursiveDelete(IEnumerable<object> list, string name, object val, Type type)
        {
            var itemsRemoved = 0;

            for(int i = 0; i < list.Count(); i++)
            {
                if (list.ElementAt(i) is IEnumerable<object>)
                {
                    itemsRemoved += RecursiveDelete((IEnumerable<object>)list.ElementAt(i), name, val, type);
                }
                else
                {
                    foreach (var member in type.GetProperties())
                    {
                        if (member.Name.ToLower() == name.ToLower() &&
                                    member.GetValue(list.ElementAt(i)).Equals(val))
                        {
                            ((List<object>)list).RemoveAt(i);   // let's just pray this works....
                            itemsRemoved++;
                        }
                    }
                }
            }

            return itemsRemoved;
        }

        public int RemoveQuery<T>(CachedQueryString query)
        {
            var removed = 0;
            if(rootNode != null)
            {
                string[] splits = query.QueryString.Split('/');

                if (splits.First().ToLower() != "start"
                || splits.Last().ToLower() != "end")
                {
                    Log.Error(" [x] Invalid query string " + query.QueryString);
                    return 0;
                }

                var lastNode = rootNode;
                int lastMatch = 1;
                for (int i = 1; i < splits.Length - 1; i++)
                {
                    if (lastNode.Children.ContainsKey(splits[i]))
                    {
                        lastNode = lastNode.Children[splits[i]];   // set the bottom leaf
                        lastMatch++;   // current parameters being analyzed
                    }
                    else if (splits[i] == "*")
                    {
                        foreach (var child in lastNode.Children)
                        {
                            var splits2 = new string[splits.Length];
                            Array.Copy(splits, 0, splits2, 0, splits.Length);
                            splits2[i] = child.Key;
                            removed += RemoveQuery<T>(new CachedQueryString(typeof(T), splits2, query.PlainQueryParameters));
                        }
                        return removed;
                    }
                }

                List<object> retval = lastNode.Data;
                var paramsLen = splits.Length - 4;  // just the parameters of the query

                if (lastMatch >= 3 && lastMatch <= splits.Length - 2)  // do we have anything else to search for?
                {
                    for (int i = lastMatch; i < splits.Length - 1; i++)
                    {
                        var name = query.PlainQueryParameters[i - 3].Name;
                        var val = query.PlainQueryParameters[i - 3].Value;
                        //retval = RecursiveSearch(retval, name, val, typeof(T));
                        removed += RecursiveDelete(retval, name, val, typeof(T));
                    }
                    Log.Info(" [x] FastAccess -> Removed " + removed + " items in cache");
                }
                else
                {
                    Log.Info(" [x] FastAccess -> Removing one item from cache..." + lastNode.Key);
                    lastNode.Parent.Children.Remove(lastNode.Key);  // I really hope this works
                    removed = 1;
                }
            }
            return removed;
        }

        public List<T> RetrieveBestMatch<T>(CachedQueryString query)
        {
            var retval = new List<object>();

            if (rootNode != null)
            {
                string[] splits = query.QueryString.Split('/');

                if (splits.First().ToLower() != "start"
                || splits.Last().ToLower() != "end")
                {
                    Log.Error(" [x] Invalid query string " + query.QueryString);
                    return null;
                }

                var lastNode = rootNode;
                int lastMatch = 1;

                for (int i = 1; i < splits.Length - 1; i++)
                {
                    if (lastNode.Children.ContainsKey(splits[i]))
                    {
                        lastNode = lastNode.Children[splits[i]];   // set the bottom leaf
                        lastMatch++;   // current parameters being analyzed
                    }
                    else if(splits[i] == "*")
                    {
                        foreach(var child in lastNode.Children)
                        {
                            var splits2 = new string[splits.Length];
                            Array.Copy(splits, 0, splits2, 0, splits.Length);
                            splits2[i] = child.Key;
                            var result = RetrieveBestMatch<T>(new CachedQueryString(typeof(T), splits2, query.PlainQueryParameters));
                            if (result != null)
                                result.ForEach(x => retval.Add((T)x));
                        }
                        return retval?.Select(x => (T)x).ToList();
                    }
                }

                retval = lastNode.Data;
                var paramsLen = splits.Length - 4;  // just the parameters of the query

                if (lastMatch >= 3 && lastMatch <= splits.Length - 2)  // do we have anything else to search for?
                {
                        Log.Info(" [x] FastAccess -> " + lastNode.Key + " -> Querying...");
                        for (int i = lastMatch; i < splits.Length - 1; i++)
                        {
                            var name = query.PlainQueryParameters[i - 3].Name;
                            var val = query.PlainQueryParameters[i - 3].Value;

                            var result = RecursiveSearch(retval, name, val, typeof(T));
                            if(result != null)
                                retval.AddRange(result);
                        }
                        Log.Info(" [x] FastAccess -> Found " + retval.Count.ToString() + " items in cache");
                }
                else
                {
                    if(retval?.Count > 0)
                    {
                        Log.Info(" [x] FastAccess -> " + lastNode.Key);
                    }
                }
            }

            return retval?.Select(x => (T)x).ToList();
        }

        public void ResetCache()
        {
            //lock(dummy)
            //{
            rootNode.Children.Clear();
            //}
        }
    }
}
