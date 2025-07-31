using System;
using System.Numerics;
using CSharpTools.Mathematics;

namespace CSharpTools.Cryptography
{
    public class DiffieHellman : IDisposable
    {
        public BigInteger P { get; private set; }
        public BigInteger G { get; private set; }
        public BigInteger Intermediate { get; private set; }
        
        private BigInteger _n;
        

        public DiffieHellman(BigInteger p, BigInteger g)
        {
            P = p;
            G = g;
            
            if (P == 0 || G == 0)
                throw new ArgumentOutOfRangeException();
            
            _n = MathUtility.RandomBigInteger(P);
            Intermediate = BigInteger.ModPow(G, _n, P);
        }

        public DiffieHellman()
        {
            P = MathUtility.GetBigPrime();
            G = MathUtility.RandomBigInteger(P);
            _n = MathUtility.RandomBigInteger(P);
            Intermediate = BigInteger.ModPow(G, _n, P);
        }

        public BigInteger GetKey(BigInteger otherIntermediate)
        {
            BigInteger bigInteger = BigInteger.ModPow(otherIntermediate, _n, P);
            return bigInteger;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            P = new BigInteger(0);
            G = new BigInteger(0);
            _n = new BigInteger(0);
            Intermediate = new BigInteger(0);
        }
    }
}