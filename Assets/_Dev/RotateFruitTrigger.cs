using DG.Tweening;
using Fruits;
using UnityEngine;

public class RotateFruitTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.TryGetComponent(out Fruit fruit))
            return;
        
        fruit.transform.DORotate(Vector3.zero, 1);
    }
}
