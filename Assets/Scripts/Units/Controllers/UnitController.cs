using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        RtsGameManager.GameManager.UNITS.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
