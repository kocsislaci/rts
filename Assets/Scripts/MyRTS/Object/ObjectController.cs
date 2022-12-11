using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MyRTS.Object
{
    public class ObjectController : MonoBehaviour
    {
        public GUID Uuid { get; private set; }
        public ObjectType Type { get; private set; }
        public UnityEvent<GUID> OnDying { get; } = new();

        public void InitialiseObject(ObjectType type)
        {
            Uuid = GUID.Generate();
            Type = type;
        }
        
        public virtual void Destroy()
        {
            OnDying.Invoke(Uuid);
            Destroy(gameObject);
        }
    }
}
