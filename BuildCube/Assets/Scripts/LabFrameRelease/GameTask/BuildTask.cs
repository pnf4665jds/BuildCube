using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class BuildTask : TaskBase
{
    public GameState CurrentState { get; private set; } = GameState.Prepare;

    private GameSceneRes sceneRes;      // 管理目前場景的資源
    private GameObject TargetCube;      // 要蓋的目標方塊
    private GameObject BuildingCube;    // 手上的方塊
    private List<float> TargetDistanceList;     // 紀錄目標方塊中兩兩小方塊的彼此距離，並排序
    private List<float> BuildingDistanceList;   // 紀錄操作方塊中兩兩小方塊的彼此距離，並排序

    public override IEnumerator TaskInit()
    {
        sceneRes = GameEntityManager.Instance.GetCurrentSceneRes<GameSceneRes>();
        sceneRes.MainTask = this;
        yield return null;
    }

    /// <summary>
    /// 遊戲運行
    /// </summary>
    /// <returns></returns>
    public override IEnumerator TaskStart()
    {
        SetTargetCube();
        CurrentState = GameState.Running;
        GameEventCenter.DispatchEvent("StartTimer");
        GameEventCenter.DispatchEvent("BuildStart");

        BuildingCube = GameObject.FindWithTag("MainCubic");
        while (CurrentState == GameState.Running)
        {
            // 下面這行是避免CheckIfSame回傳False時進入無限迴圈
            yield return new WaitUntil(() => TargetCube.transform.childCount != BuildingCube.transform.childCount);
            // 等到玩家拼出同樣數量的小方塊時才判斷
            yield return new WaitUntil(() => TargetCube.transform.childCount == BuildingCube.transform.childCount);
            CheckIfSame();
        }
        Debug.Log("win");
        GameEventCenter.DispatchEvent("StopTimer");
    }

    public override IEnumerator TaskStop()
    {
        // 結束後關閉VRTK_ControllerEvents，讓玩家無法再進行操作
        sceneRes.LeftEvent.enabled = false;
        sceneRes.RightEvent.enabled = false;

        sceneRes.EditUI.ShowResult();
        GameAudioController.Instance.PlayOneShot(sceneRes.FinishSound);
        yield return new WaitForSeconds(sceneRes.FinishSound.length);
        SceneManager.LoadScene("MainUI");
    }

    /// <summary>
    /// 設置TargetCube及數據
    /// </summary>
    private void SetTargetCube()
    {
        TargetCube = CubeCreator.instance.GetCubic(GameDataManager.FlowData.TargetCubeName);
        TargetCube.AddComponent<TargetCubeEntity>().EntityInit();    // 添加Entity腳本使其能夠跟隨頭盔
        TargetCube.transform.localScale = Vector3.one;
        TargetDistanceList = new List<float>();

        Transform[] transforms = TargetCube.GetComponentsInChildren<Transform>();
        for(int i = 1; i < transforms.Length - 1; i++)
        {
            for(int j = i + 1; j < transforms.Length; j++)
            {
                TargetDistanceList.Add(Mathf.Round(Vector3.Distance(transforms[i].position, transforms[j].position) * 10000));
            }
        }
        TargetDistanceList.Sort();
    }

    /// <summary>
    /// 檢查資料是否相同
    /// </summary>
    private void CheckIfSame()
    {
        BuildingDistanceList = new List<float>();

        CheckCollision[] checkCollisions = BuildingCube.GetComponentsInChildren<CheckCollision>();
        for (int i = 0; i < checkCollisions.Length - 1; i++)
        {
            for (int j = i + 1; j < checkCollisions.Length; j++)
            {
                BuildingDistanceList.Add(Mathf.Round(Vector3.Distance(checkCollisions[i].transform.position, checkCollisions[j].transform.position) * 10000));
            }
        }
        BuildingDistanceList.Sort();

        for(int i = 0; i < BuildingDistanceList.Count; i++)
        {
            if (TargetDistanceList[i] != BuildingDistanceList[i])
                return;
        }

        CurrentState = GameState.Finish;
    }
}
