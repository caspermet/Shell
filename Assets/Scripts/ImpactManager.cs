using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    public static ImpactManager instance;
    ImpactLibrary impactLibrary;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CreateImpact(string soundName, Vector3 pos)
    {
        
    }
}
