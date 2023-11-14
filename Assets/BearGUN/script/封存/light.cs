using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    Light mylight;
    // Start is called before the first frame update
    void Start()
    {
      mylight = GetComponent<Light>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            mylight.enabled = !mylight.enabled;
        }
        else
        {
            mylight.enabled = false;
        }
        
    }
}
