using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteract : MonoBehaviour
{
    public AudioClip DeleteSound;   // 刪除方塊音效

    public void DestroyCube()
    {
        if (this.name != "Cube0")
        {
            Destroy(this.gameObject);
            GameAudioManager.instance.PlaySound(DeleteSound);
        }
    }
}