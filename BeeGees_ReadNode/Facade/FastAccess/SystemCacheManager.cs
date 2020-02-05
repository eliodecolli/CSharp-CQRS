using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BeeGees_ReadNode.Facade.FastAccess
{
    public static class SystemCacheManager
    {
        private static FastAccess fastAccess;
        private static Dictionary<string, FastUserStatistics> memory;
        private static int threshold;
        private static bool autoCleanup;
        private static Timer timer;

        public static void Initialize(int requestsThreshold, bool fastAccessAutoCleanup = false, int fastAccessAutoCleanupInterval = 60000)
        {
            fastAccess = new FastAccess();
            autoCleanup = fastAccessAutoCleanup;
            memory = new Dictionary<string, FastUserStatistics>();
            threshold = requestsThreshold;

            timer = new Timer(fastAccessAutoCleanupInterval);
            timer.Elapsed += Timer_Elapsed;

            timer.Start();
        }

        public static void RemoveQuery<T>(CachedQueryString query)
        {
            fastAccess.RemoveQuery<T>(query);
        }

        public static bool RegisterStatistic(string sender, Type msg)
        {
            if (memory.ContainsKey(sender))
            {
                memory[sender].RegisterStatistics(msg);
                return memory[sender].HasReachedThreshold(msg);
            }
            else
            {
                memory.Add(sender, new FastUserStatistics(threshold));
                memory[sender].RegisterStatistics(msg);
                return false;
            }
        }

        public static void RegisterQuery(CachedQueryString query, object value, bool replace = true)
        {
            fastAccess.RegisterQuery(query, value, replace);
        }

        public static List<T> RetrieveBestMatch<T>(CachedQueryString query)
        {
            return fastAccess.RetrieveBestMatch<T>(query);
        }

        public static void Reset()
        {
            fastAccess.ResetCache();
            memory.Clear();
        }

        public static void SetAutoCleanup(bool autoCleanup)
        {
            SystemCacheManager.autoCleanup = autoCleanup;
        }

        public static void SetAutoCleanupInterval(int autoCleanUpInterval)
        {
            timer.Stop();
            timer.Interval = autoCleanUpInterval;

            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (autoCleanup)
            {
                fastAccess.ResetCache();
                Log.Info(" [x] FastAccess -> Automtaic Cache reset");
            }
        }
    }
}
