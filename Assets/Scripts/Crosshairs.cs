using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHightLightColour;
    Color originalDotColour;

    private void Start()
    {
        Cursor.visible = false;
        originalDotColour = dot.color;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }

    public void DetectTartgets(Ray ray)
    {

        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHightLightColour;
        }
        else
        {
            dot.color = originalDotColour;
        }
    }
}
