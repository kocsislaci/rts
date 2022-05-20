using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : UnitController
{
    [SerializeField]
    private SoldierSettings _settings;
    public SoldierDescription description;
    void Start()
    {
        _settings = Resources.Load<SoldierSettings>("Units/SoldierSettings");
        description = this.gameObject.GetComponent<SoldierDescription>();
        description.Speed = _settings.speed;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
