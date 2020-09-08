using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float livetime = 4;
    float fadetime = 2;

    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        myRigidbody.AddForce(transform.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(livetime);

        float percent = 0;
        float fadeSpeed = 1 / fadetime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColour = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColour, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
