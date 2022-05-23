using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        // mock start
        RtsGameManager.GameManager.Resources[0].Amount = 1000;
        RtsGameManager.GameManager.Resources[1].Amount = 900;
        RtsGameManager.GameManager.Resources[2].Amount = 800;
    }
}
