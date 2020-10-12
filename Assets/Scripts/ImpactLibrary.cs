using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactLibrary : MonoBehaviour
{
    public ImpactGroup[] soundGroups;

    [System.Serializable]
    public class ImpactGroup
    {
        public string groupID;
        public Transform[] group;
    }
}
