using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class EditUI : MonoBehaviour
{
    /// <summary>
    /// 操作模式
    /// </summary>
    public enum Mode
    {
        Edit,   // 編輯模式，可添加方塊
        Delete  // 刪除模式，可移除方塊
    }

    public Mode CurrentMode;
    public BuildCube BuildCube;
    public VRTK_Pointer Pointer;
    public GameObject SimulatorCamera, SteamVRCamera;
    public Text TimerText;

    private float timer = 0;

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

    /// <summary>
    /// 花費時間Timer
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        while (true)
        {
            yield return null;
            timer += Time.deltaTime;
            TimerText.text = timer.ToString("0.00");
        }
    }
}
