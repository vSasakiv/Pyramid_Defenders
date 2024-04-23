using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleTransform : MonoBehaviour
{

    public Vector3 scaleChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += scaleChange;
    }
}

