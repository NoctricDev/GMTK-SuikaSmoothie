using UnityEngine;

namespace CustomerScene
{
    [CreateAssetMenu(fileName = "CustomerSprites_SO", menuName = "Scriptable Objects/CustomerSprites_SO")]
    public class CustomerSprites : ScriptableObject
    {
        public Sprite CustomerSprite;
        public Sprite CustomerIcon;
    }
}