using Sirenix.OdinInspector;
using UnityEngine;

namespace Helper
{
    public class SetPosition : MonoBehaviour
    {
        [SerializeField] private Transform _targetPos;
        [SerializeField] private bool setX;
        [SerializeField] private bool setY;
        [SerializeField] private bool setZ;
        
        [Button, HideInPlayMode]
        private void Update()
        {
            transform.position = new Vector3(
                setX ? _targetPos.position.x : transform.position.x,
                setY ? _targetPos.position.y : transform.position.y,
                setZ ? _targetPos.position.z : transform.position.z
                );
        }
    }
}
