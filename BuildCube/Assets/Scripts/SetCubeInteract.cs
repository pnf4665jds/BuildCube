using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SetCubeInteract : VRTK_InteractableObject
{
    [Header("Pick up SE")]
    public AudioClip PickupSE;       // 撿起方塊的音效

    /// <summary>
    /// 自動抓取方塊
    /// </summary>
    /// <param name="currentTouchingObject"></param>
    public override void StartTouching(VRTK_InteractTouch currentTouchingObject = null)
    {
        //if (PickupSE != null && currentTouchingObject.gameObject.name == "LeftControllerScriptAlias")
            //GameAudioController.Instance.PlayOneShot(PickupSE);
        base.StartTouching(currentTouchingObject);
        VRTK_InteractGrab myGrab = currentTouchingObject.GetComponent<VRTK_InteractGrab>();
        myGrab.AttemptGrab();
    }
}
