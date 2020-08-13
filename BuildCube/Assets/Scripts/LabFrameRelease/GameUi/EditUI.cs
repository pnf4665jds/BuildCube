using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;
using LabData;

public class EditUI : MonoBehaviour
{
    public Mode CurrentMode;    // 目前的模式(編輯或刪除模式)
    public Text InstructionText;    // 遊戲說明
    public Text TimerText;      // 計時器
    public Text ResultText;     // 顯示結果

    private GameSceneRes sceneRes;
    private IEnumerator timer;
    private float time = 0;

    private void Start()
    {
        GameEventCenter.AddEvent("StartTimer", StartTimer);
        GameEventCenter.AddEvent("StopTimer", StopTimer);
        sceneRes = GameEntityManager.Instance.GetCurrentSceneRes<GameSceneRes>();
    }

    /// <summary>
    /// 切換模式
    /// </summary>
    public void SwitchMode()
    {
        if (sceneRes.MainTask.CurrentState != GameState.Running)
            return;

        CurrentMode = (CurrentMode == Mode.Edit) ? Mode.Delete : Mode.Edit;
        if (CurrentMode == Mode.Delete)
        {
            sceneRes.BuildCubeTask.DeleteModeInitialize();
        }
        else
        {
            sceneRes.BuildCubeTask.EditModeInitialize();
        }
    }

    /// <summary>
    /// 開始Timer
    /// </summary>
    private void StartTimer()
    {
        InstructionText.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(true);
        timer = Timer();
        StartCoroutine(timer);
    }

    /// <summary>
    /// 停止Timer
    /// </summary>
    private void StopTimer()
    {
        StopCoroutine(timer);
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
            time += Time.deltaTime;
            TimerText.text = time.ToString("0.00");
        }
    }

    /// <summary>
    /// 顯示結果UI
    /// </summary>
    public void ShowResult()
    {
        ResultText.transform.parent.gameObject.SetActive(true);
        ResultText.text = "花費時間：" + TimerText.text + "秒";
        TimerText.gameObject.SetActive(false);

        GameDataManager.LabDataManager.SendData(new OutputData(float.Parse(TimerText.text)));
    }
}
