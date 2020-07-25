using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EditUI : MonoBehaviour
{
    public enum Mode
    {
        Edit,
        Delete
    }

    public Mode CurrentMode;
    public BuildCube BuildCube;
    public VRTK_Pointer Pointer;

    /// <summary>
    /// 切換模式
    /// </summary>
    public void SwitchMode()
    {
        //ModeButton.GetComponentInChildren<Text>().text = (currentMode == EditUI.Mode.Edit) ? "模 式 : 刪 除" : "模 式 : 編 輯";
        //VRModeText.text = ModeButton.GetComponentInChildren<Text>().text;
        CurrentMode = (CurrentMode == EditUI.Mode.Edit) ? EditUI.Mode.Delete : EditUI.Mode.Edit;
        //GameUIManager.Instance.EditUi.currentMode = currentMode;
        if (CurrentMode == EditUI.Mode.Delete)
        {
            BuildCube.DeleteModeInitialize();
            //SaveButton.interactable = false;
            //LoadButton.interactable = false;
            Pointer.enabled = true;
        }
        else
        {
            BuildCube.EditModeInitialize();
            //SaveButton.interactable = true;
            //LoadButton.interactable = true;
            Pointer.enabled = false;
        }
    }
}
