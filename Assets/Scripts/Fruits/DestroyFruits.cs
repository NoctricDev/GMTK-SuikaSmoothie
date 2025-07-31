using UnityEngine;

namespace Fruits
{
    public class DestroyFruits : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // ask for parent because the collider is on the child object
            if (!other.transform.parent.TryGetComponent(out Fruit fruit))
                return;
            
            Destroy(fruit.gameObject);
        }
    }
}
