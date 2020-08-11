using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BuildCubeTask : TaskBase
{
    private bool readyToSet = false;
    private GameSceneRes sceneRes;
    private string deviceName;
    private VRTK_ControllerEvents controllerEvents;
    private Vector3 cameraPosition;
    private GameObject cameraObject;
    private GameObject setCube;
    private GameObject center;

    public override IEnumerator TaskInit()
    {
        sceneRes = GameEntityManager.Instance.GetCurrentSceneRes<GameSceneRes>();
        sceneRes.BuildCubeTask = this;
        GameEventCenter.AddEvent("BuildStart", BuildStart);
        // 產生一個MainCubic作為Parent
        center = Object.Instantiate(sceneRes.MainCubicPrefab);
        // 等待準備完成後才設置方塊
        yield return new WaitUntil(() => readyToSet);

        // 取得對應裝置的手把及攝影機
        deviceName = VRTK_SDKManager.instance.loadedSetup.gameObject.name;
        if (deviceName == "VRSimulator")
        {
            center.transform.parent = sceneRes.SimulatorRightHand.transform;
            cameraObject = sceneRes.SimulatorCamera.gameObject;
        }
        if (deviceName == "SteamVR")
        {
            center.transform.parent = sceneRes.SteamVRRightHand.transform;
            cameraObject = sceneRes.SteamVRCamera.gameObject;
        }

        // 產生第一個方塊掛在右手把
        GameObject centerCube = CreateAndSetCube(new Vector3(0, 0, 0));
        centerCube.GetComponent<Renderer>().material.mainTexture = CubeCreator.instance.Fill(Color.cyan);
        centerCube.name = "Cube0";
        center.transform.localPosition = new Vector3(0, 0, 0);
        controllerEvents = Object.FindObjectOfType<VRTK_ControllerEvents>();
    }

    public override IEnumerator TaskStart()
    {
        while (sceneRes.MainTask.CurrentState == GameState.Running)
        {
            // 創造被移動的方塊(SetCube)
            if (setCube == null && sceneRes.EditUI.CurrentMode == Mode.Edit)
            {
                setCube = Object.Instantiate(sceneRes.SetCubePrefab, Vector3.zero, Quaternion.identity);
                setCube.GetComponent<Renderer>().material.mainTexture = CubeCreator.instance.Fill(Color.white);
                setCube.transform.localScale = new Vector3(1, 1, 1) * 0.05f;
            }

            // 取得攝影機位置並讓SetCube跟隨攝影機
            cameraPosition = cameraObject.transform.position;
            if (setCube != null && setCube.GetComponentsInChildren<Transform>().Length <= 1)   // 如果還沒拿起setCube(白色方塊)
            {
                setCube.transform.position = cameraPosition + new Vector3(-0.2f, 0, 0.5f);
            }
            yield return null;
        }
    }

    public override IEnumerator TaskStop()
    {
        yield return null;
    }

    /// <summary>
    /// 呼叫此函示後將方塊設置在手把上
    /// </summary>
    private void BuildStart()
    {
        readyToSet = true;
    }

    /// <summary>
    /// 將方塊放置於MainCubic下
    /// </summary>
    public GameObject CreateAndSetCube(Vector3 pos)
    {
        GameObject cube = CubeCreator.instance.CreateCube();
        cube.transform.position = pos;
        cube.transform.rotation = center.transform.rotation;
        cube.transform.parent = center.transform;
        cube.transform.localScale = new Vector3(1, 1, 1) * 0.05f;
        return cube;
    }

    /// <summary>
    /// 編輯模式初始化
    /// </summary>
    public void EditModeInitialize()
    {
        foreach (Renderer renderer in center.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.name != "Cube0")
            {
                renderer.material.mainTexture = CubeCreator.instance.Fill(Color.white);
            }
        }
        sceneRes.Pointer.enabled = false;
    }

    /// <summary>
    /// 刪除模式初始化
    /// </summary>
    public void DeleteModeInitialize()
    {
        Object.Destroy(setCube);
        sceneRes.Pointer.enabled = true;
    }
}
