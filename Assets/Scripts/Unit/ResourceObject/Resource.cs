using UnityEngine;

namespace Unit.ResourceObject
{
    public abstract class Resource
    {
        public GameObject gameObject;
        
        public virtual void Destroy()
        {
            Object.Destroy(gameObject);
        }
    }
}
