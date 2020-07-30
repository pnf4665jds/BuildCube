using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BuildCube : MonoBehaviour
{
    public GameObject SetCubePrefab;        // 用於擺設的方塊
    public GameObject MainCubicPrefab;      // 被組合的大方塊
    public GameObject SimulatorRightHand;   // VR Simulator的右手柄
    public GameObject SteamVRRightHand;     // SteamVR的右手柄
    public EditUI EditUI;

    public GameObject SetCube { get; protected set; }
    public GameObject Center { get; protected set; }

    private string deviceName;
    private VRTK_ControllerEvents controllerEvents;
    private Vector3 cameraPosition;

    private void Start()
    {
        GameEventCenter.AddEvent("BuildStart", BuildStart);
    }

    private void Update()
    {
        // 創造被移動的方塊
        if (SetCube == null && EditUI.CurrentMode == EditUI.Mode.Edit)
        {
            SetCube = Instantiate(SetCubePrefab, cameraPosition + new Vector3(0, 0, 0.5f), new Quaternion(0, 0, 0, 0));
            SetCube.GetComponent<Renderer>().material.mainTexture = CubeCreator.instance.Fill(Color.white);
            SetCube.transform.localScale = new Vector3(1, 1, 1) * 0.05f;
        }
        GetCamera();
        if (SetCube != null && SetCube.GetComponentsInChildren<Transform>().Length <= 1)   // 如果還沒拿起setCube(白色方塊)
        {
            SetCube.transform.position = cameraPosition + new Vector3(0, 0, 0.5f);
        }
    }

    /// <summary>
    /// 呼叫此函式來生成Cube在手把上
    /// </summary>
    public void BuildStart()
    {
        Center = Instantiate(MainCubicPrefab);
        StartCoroutine(SetCubeOnController());
    }

    /// <summary>
    /// 取得攝影機位置
    /// </summary>
    private void GetCamera()
    {
        if (deviceName == "VRSimulator")
        {
            cameraPosition = EditUI.SimulatorCamera.gameObject.transform.position;
        }
        else if (deviceName == "SteamVR")
        {
            cameraPosition = EditUI.SteamVRCamera.gameObject.transform.position;
        }
    }

    /// <summary>
    /// 初始時將方塊放置到控制器上
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetCubeOnController()
    {
        yield return new WaitForSeconds(0.1f);

        deviceName = VRTK_SDKManager.instance.loadedSetup.gameObject.name;
        if (deviceName == "VRSimulator")
            Center.transform.parent = SimulatorRightHand.transform;
        if (deviceName == "SteamVR")
            Center.transform.parent = SteamVRRightHand.transform;

        // 產生一個方塊
        GameObject centerCube = Create(new Vector3(0, 0, 0), Quaternion.identity);
        centerCube.GetComponent<Renderer>().material.mainTexture = CubeCreator.instance.Fill(Color.cyan);
        centerCube.name = "Cube0";
        Center.transform.localPosition = new Vector3(0, 0, 0);
        controllerEvents = (controllerEvents ? GameObject.FindObjectOfType<VRTK.VRTK_ControllerEvents>() : controllerEvents);
    }

    /// <summary>
    /// 創造方塊
    /// </summary>
    public GameObject Create(Vector3 pos, Quaternion rotation)
    {
        return CubeCreator.instance.CreateCube(Center, pos, rotation);
    }

    /// <summary>
    /// 編輯模式初始化
    /// </summary>
    public void EditModeInitialize()
    {
        if (controllerEvents != null)
            return;
        foreach (Renderer renderer in Center.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.name != "Cube0")
            {
                renderer.material.mainTexture = CubeCreator.instance.Fill(Color.white);
            }
        }
    }

    /// <summary>
    /// 刪除模式初始化
    /// </summary>
    public void DeleteModeInitialize()
    {
        Destroy(SetCube);
    }
}
