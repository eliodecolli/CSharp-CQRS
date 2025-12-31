using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_ReadNode.Facade.FastAccess
{
    public class CacheNode
    {
        public CacheNode Parent { get; private set; }

        public Dictionary<string, CacheNode> Children { get; set; }

        public List<object> Data { get; private set; }

        public string Key { get; private set; }

        public CacheNode()   // a new Cache root most likely
        {
            Parent = null;
            Children = new Dictionary<string, CacheNode>();
            Data = null;
            Key = "root";
        }

        public CacheNode(string key, CacheNode parent)
        {
            Parent = parent;
            Data = new List<object>();
            Children = new Dictionary<string, CacheNode>();
            Key = key;

            /*if(!parent.Children.TryAdd(key, this))
            {
                // maaaayybee we should throw??
                Log.Error($"An error has occured while adding a new cache node under {parent.Key} with key {key}. Probably it's already there :/");
                return;
            }*/
        }

        public void SetData(object data)
        {
            Data = new List<object>();

            if (data is IEnumerable<object> || data is IEnumerable)   // meeh we must be careful here...
                Data.AddRange((IEnumerable<object>)data);
            else
                Data.Add(data);
        }

        public void AddData(object data)
        {
            Data.Add(data);
        }
    }
}
