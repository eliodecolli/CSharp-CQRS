using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_ReadNode.Facade.FastAccess
{
    public class FastUserStatistics
    {
        private readonly Dictionary<Type, int> statistics;
        private int threshold;

        public FastUserStatistics(int threshold)
        {
            statistics = new Dictionary<Type, int>();
            this.threshold = threshold;
        }

        public void RegisterStatistics(Type type)
        {
            if (statistics.ContainsKey(type))
                statistics[type]++;
            else
                statistics[type] = 1;
        }

        public bool HasReachedThreshold(Type type)
        {
            return statistics.ContainsKey(type) && statistics[type] >= threshold;
        }

        public void Reset()
        {
            statistics.Clear();
        }

        public void Reset(Type type)
        {
            if(statistics.ContainsKey(type))
                statistics.Remove(type);
        }
    }
}
