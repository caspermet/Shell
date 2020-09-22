using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPhoton : MonoBehaviour
{
    public string menuName;
    public bool open;

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Debug.Log("closing menu");
        open = false;
        gameObject.SetActive(false);
    }
}
