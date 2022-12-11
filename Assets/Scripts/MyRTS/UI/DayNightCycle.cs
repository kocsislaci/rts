using UnityEngine;

namespace MyRTS.UI
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private Transform sunTransform;
        [SerializeField] private Transform moonTransform;

        [SerializeField]
        [Range(0.0f, 1.2f)] private float cycleSpeed = 0.6f;
    
        void Update()
        {
            var deltaRotation = Quaternion.Euler(new Vector3(0.0f, cycleSpeed * Time.deltaTime, 0.0f));
            sunTransform.localRotation = deltaRotation * sunTransform.localRotation;
            moonTransform.localRotation = deltaRotation * moonTransform.localRotation;
        }
    }
}
