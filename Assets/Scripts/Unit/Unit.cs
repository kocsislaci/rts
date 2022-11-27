using GameManagers;
using UnityEditor;
using UnityEngine;

namespace Unit
{
    public abstract class Unit
    {
        public GUID uuid;
        public GameObject gameObject;
        
        protected Unit()
        {
            uuid = GUID.Generate();
            GameManager.MY_UNITS.Add(uuid ,this);
        }
        public virtual void Destroy()
        {
            Object.Destroy(gameObject);
            GameManager.MY_UNITS.Remove(uuid);
        }
    }
}
