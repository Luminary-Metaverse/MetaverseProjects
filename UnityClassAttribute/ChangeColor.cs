using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MyClass : using UnityEngine;

public class ChangeColor : MonoBehaviour {
     void Update() {
        gameObject.GetComponent<Renderer>().sharedMaterial.color = new Color (
            Random.Range(0f,1f),
            Random.Range(0f,1f),
            Random.Range(0f,1f),
            Random.Range(0f,1f)
        );
    }
}
