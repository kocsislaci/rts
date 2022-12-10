using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RTS.Object
{
    public class ObjectController : MonoBehaviour
    {
        public GUID Uuid { get; private set; }
        public ObjectType Type { get; private set; }
        public UnityEvent OnDying { get; } = new();

        public void InitialiseObject(ObjectType type)
        {
            Uuid = GUID.Generate();
            Type = type;
        }
        
        public virtual void Destroy()
        {
            OnDying.Invoke();
            Destroy(gameObject);
        }
    }
}
