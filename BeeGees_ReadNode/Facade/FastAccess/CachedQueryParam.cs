using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_ReadNode.Facade.FastAccess
{
    public class CachedQueryParam
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public CachedQueryParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
