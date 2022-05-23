using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public GameObject selectionCircle;
    
    public void Select()
    {
        Select(false);
    }

    public virtual void Select(bool clearSelection)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Deselect()
    {
        throw new System.NotImplementedException();
    }
}
