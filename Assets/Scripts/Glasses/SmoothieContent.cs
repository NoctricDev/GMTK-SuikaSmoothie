using System.Collections.Generic;
using System.Linq;
using Fruits;
using JetBrains.Annotations;
using UnityEngine;

namespace Glasses
{
    public class SmoothieContent : IGlassContent
    {
        public IReadOnlyDictionary<FruitSO, int> FruitsInSmoothie { get; private set; }
        
        public int TotalFruitsCount => FruitsInSmoothie.Values.Sum();

        private bool _isDirty = true;

        [CanBeNull] private FruitSO _firstFruitCache;
        [CanBeNull] private FruitSO _firstFruit
        {
            get
            {
                if (_isDirty)
                {
                    _firstFruitCache = FruitsInSmoothie?.Keys?.FirstOrDefault();
                    _isDirty = false;
                }
                return _firstFruitCache;
            }
        }
        
        public SmoothieContent(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitsInSmoothie = fruitsInSmoothie;
        }

        public void SetContent(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitsInSmoothie = fruitsInSmoothie;
            _isDirty = true;
        }

        public Color GetTopColor()
        {
            return _firstFruit?.SmoothieTopColor ?? Color.white;
        }

        public Color GetSideColor()
        {
            return _firstFruit?.SmoothieSideColor ?? Color.white;
        }
    }
}