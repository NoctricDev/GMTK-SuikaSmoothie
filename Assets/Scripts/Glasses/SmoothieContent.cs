using System.Collections.Generic;
using System.Linq;
using Fruits;

namespace Glasses
{
    public class SmoothieContent : IGlassContent
    {
        public Dictionary<FruitSO, int> FruitsInSmoothie { get; private set; }
        
        public int TotalFruitsCount => FruitsInSmoothie.Values.Sum();
        
        public SmoothieContent(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitsInSmoothie = fruitsInSmoothie;
        }

        public void SetContent(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitsInSmoothie = fruitsInSmoothie;
        }
    }
}