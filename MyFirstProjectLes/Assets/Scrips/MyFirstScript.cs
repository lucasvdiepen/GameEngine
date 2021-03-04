using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstScript : MonoBehaviour
{
    public int getal;
    public string naam;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
        Debug.Log(gameObject.transform.position);
        Debug.LogError(naam);
    }

    // Update is called once per frame
    void Update()
    {
        getal++;
        Debug.Log(getal);
    }
}
