using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteract : MonoBehaviour
{
    public void DestroyCube()
    {
        if (this.name != "Cube0")
            Destroy(this.gameObject);
    }
}