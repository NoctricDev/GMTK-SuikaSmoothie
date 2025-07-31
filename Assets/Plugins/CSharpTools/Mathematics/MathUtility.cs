using System;
using System.Numerics;

namespace CSharpTools.Mathematics
{
    public static class MathUtility
    {
        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static uint Clamp(uint value, uint min, uint max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
            return value < min ? min : value > max ? max : value;
        }
        
        public static BigInteger RandomBigInteger(BigInteger max)
        {
            Random rng = new(Guid.NewGuid().GetHashCode());
            byte[] bytes = new byte[max.ToByteArray().LongLength - 1];
            BigInteger a;
            
            do
            {
                rng.NextBytes(bytes);
                a = new BigInteger(bytes);
            } 
            while (a < 2 || a >= max - 2);
            
            return a;
        }

        public static bool IsProbablePrime(this BigInteger n, int certainty)
        {
            if (n == 2 || n == 3)
                return true;
            
            if (n < 2 || n % 2 == 0)
                return false;

            BigInteger d = n - 1;
            int s = 0;
            
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            for (int i = 0; i < certainty; i++)
            {
                BigInteger a = RandomBigInteger(n);
                BigInteger x = BigInteger.ModPow(a, d, n);
                
                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    
                    if (x == 1)
                        return false;
                    
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            return true;
        }

        public static BigInteger GetBigPrime(int certainty = 30)
        {
            BigInteger start = RandomBigInteger(BigInteger.Pow(10, 100));
            
            if (start % 2 == 0)
                start -= 1;
            
            while (!start.IsProbablePrime(certainty))
                start += 2;
            
            return start;
        }
    }
}