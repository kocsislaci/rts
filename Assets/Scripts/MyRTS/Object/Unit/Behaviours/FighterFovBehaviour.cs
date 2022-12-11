using UnityEngine;
using UnityEngine.Events;

namespace MyRTS.Object.Unit.Behaviours
{
    [RequireComponent(typeof(SphereCollider))]
    public class FighterFovBehaviour : MonoBehaviour
    {
        private SphereCollider fovCollider;
        public UnityEvent<UnitController> unitEntered = new();
        public UnityEvent<UnitController> unitExited = new();

        private void Awake()
        {
            fovCollider = GetComponent<SphereCollider>();
        }
        public void SetFOV(float range)
        {
            fovCollider.radius = range;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<UnitController>() != null)
                unitEntered.Invoke(other.GetComponent<UnitController>());
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<UnitController>() != null)
                unitExited.Invoke(other.GetComponent<UnitController>());
        }
    }
}
