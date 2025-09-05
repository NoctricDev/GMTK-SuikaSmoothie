using UnityEngine;

namespace CustomerScene
{
    [CreateAssetMenu(fileName = "CustomerPool_SO", menuName = "Scriptable Objects/CustomerPool_SO")]
    public class CustomerPool_SO : ScriptableObject
    {
        public CustomerNames_SO[] customerNamePool;
        public CustomerSprites[] customerSpritePool;
    }
}
