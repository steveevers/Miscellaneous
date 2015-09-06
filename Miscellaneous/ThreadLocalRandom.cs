using System;
using System.Threading;

namespace Miscellaneous
{
    /// <summary> 
    /// Convenience class for dealing with randomness. 
    /// </summary> 
    public static class ThreadLocalRandom
    {
        /// <summary> 
        /// Random number generator used to generate seeds, 
        /// which are then used to create new random number 
        /// generators on a per-thread basis. 
        /// </summary> 
        private static readonly Random globalRandom = new Random();
        private static readonly object globalLock = new object();

        /// <summary> 
        /// Random number generator 
        /// </summary> 
        private static readonly ThreadLocal<Random> threadRandom = new ThreadLocal<Random>(NewRandom);

        /// <summary> 
        /// Creates a new instance of Random. The seed is derived 
        /// from a global (static) instance of Random, rather 
        /// than time. 
        /// </summary> 
        public static Random NewRandom()
        {
            lock (globalLock)
            {
                return new Random(globalRandom.Next());
            }
        }

        /// <summary> 
        /// Returns an instance of Random which can be used freely 
        /// within the current thread. 
        /// </summary> 
        public static Random Instance { get { return threadRandom.Value; } }

        public static int Next()
        {
            return Instance.Next();
        }
        
        public static int Next(int maxValue)
        {
            return Instance.Next(maxValue);
        }
        
        public static int Next(int minValue, int maxValue)
        {
            return Instance.Next(minValue, maxValue);
        }
        
        public static double NextDouble()
        {
            return Instance.NextDouble();
        }
        
        public static void NextBytes(byte[] buffer)
        {
            Instance.NextBytes(buffer);
        }
    }
}
