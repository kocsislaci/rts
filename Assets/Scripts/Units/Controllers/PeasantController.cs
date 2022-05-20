using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantController : MonoBehaviour
{
    [SerializeField]
    private PeasantSettings _settings;
    public PeasantDescription description;
    void Start()
    {
        _settings = Resources.Load<PeasantSettings>("Units/PeasantSettings");
        description = this.gameObject.GetComponent<PeasantDescription>();
        description.Speed = _settings.speed;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
