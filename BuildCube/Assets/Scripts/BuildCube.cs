using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BuildCube : MonoBehaviour
{
    public GameObject SetCubePrefab;        // 用於擺設的方塊
    public GameObject LinePrefab;           // 鍵盤模式下的射線
    public GameObject MainCubicPrefab;      // 被組合的大方塊
    public GameObject SimulatorRightHand;   // VR Simulator的右手柄
    public GameObject SteamVRRightHand;     // SteamVR的右手柄
    public CubeCreator Creator;
    public EditUI EditUI;
    public GameObject SimulatorCamera, SteamVRCamera;

    public GameObject setCube { get; protected set; }
    public GameObject center { get; protected set; }
    public GameObject lineEndObject { get; protected set; }

    private string deviceName;
    private LineRenderer lineRenderer;
    private GameObject previousCube;
    private GameObject currentCube;
    private VRTK_ControllerEvents controllerEvents;
    private Vector3 cameraPosition;

    void Start()
    {
        StartCoroutine(SetCubeOnController());
    }

    void Update()
    {
        // 創造被移動的方塊
        if (setCube == null && EditUI.CurrentMode == EditUI.Mode.Edit)
        {
            setCube = Instantiate(SetCubePrefab, cameraPosition + new Vector3(0, 0, 0.5f), new Quaternion(0, 0, 0, 0));
            setCube.GetComponent<Renderer>().material.mainTexture = Creator.Fill(Color.white);
            setCube.transform.localScale = new Vector3(1, 1, 1) * 0.05f;
        }
        GetCamera();
        if (setCube != null && setCube.GetComponentsInChildren<Transform>().Length <= 1)   // 如果還沒拿起setCube(白色方塊)
        {
            setCube.transform.position = cameraPosition + new Vector3(0, 0, 0.5f);
        }
        //InputControl();
        DrawLine();
    }

    /// <summary>
    /// 取得攝影機位置
    /// </summary>
    private void GetCamera()
    {
        try
        {
            string deviceName = VRTK_SDKManager.instance.loadedSetup.gameObject.name;
            if (deviceName == "VRSimulator")
            {
                cameraPosition = SimulatorCamera.gameObject.transform.position;
            }
            if (deviceName == "SteamVR")
            {
                cameraPosition = SteamVRCamera.gameObject.transform.position;
            }
        }
        catch
        {
            Debug.Log("No SDK Found!");
        }
    }

    /// <summary>
    /// 初始時將方塊放置到控制器上
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetCubeOnController()
    {
        yield return new WaitForSeconds(0.1f);

        center = Instantiate(MainCubicPrefab);
        try
        {
            deviceName = VRTK_SDKManager.instance.loadedSetup.gameObject.name;
            if (deviceName == "VRSimulator")
                center.transform.parent = SimulatorRightHand.transform;
            if (deviceName == "SteamVR")
                center.transform.parent = SteamVRRightHand.transform;
        }
        catch
        {
            Debug.Log("No SDK Found!");
        }

        GameObject centerCube = Create(new Vector3(0, 0, 0), Quaternion.identity);
        centerCube.GetComponent<Renderer>().material.mainTexture = Creator.Fill(Color.cyan);
        centerCube.name = "Cube0";
        center.transform.localPosition = new Vector3(0, 0, 0);
        controllerEvents = (controllerEvents == null ? GameObject.FindObjectOfType<VRTK.VRTK_ControllerEvents>() : controllerEvents);

        GetCamera();
    }

    /// <summary>
    /// 創造方塊
    /// </summary>
    public GameObject Create(Vector3 pos, Quaternion rotation)
    {
        return Creator.CreateCube(center, pos, rotation);
    }

    /// <summary>
    /// 刪除方塊
    /// </summary>
    public void Delete()
    {
        if (currentCube != null && currentCube.name == "Cube0")
        {
            return;
        }
        Destroy(currentCube);
    }

    /// <summary>
    /// 編輯模式初始化
    /// </summary>
    public void EditModeInitialize()
    {
        if (controllerEvents != null)
            return;
        Destroy(lineEndObject);
        if (currentCube != null && currentCube.name != "Cube0")
        {
            currentCube.GetComponent<Renderer>().material.mainTexture = Creator.Fill(Color.white);
        }
    }

    /// <summary>
    /// 刪除模式初始化
    /// </summary>
    public void DeleteModeInitialize()
    {
        Destroy(setCube);
        if (controllerEvents != null)
        {
            return;
        }
        lineEndObject = Instantiate(LinePrefab, new Vector3(0, 0, 20), new Quaternion(0, 0, 0, 1));
        lineRenderer = lineEndObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    /// <summary>
    /// 刪除模式中畫出提示線
    /// </summary>
    public void DrawLine()
    {
        if (EditUI.CurrentMode == EditUI.Mode.Edit || controllerEvents != null)
            return;

        RaycastHit hit;
        // 產生一個指向lineEndObject的射線
        Ray ray = new Ray(new Vector3(0, 0, -10), lineEndObject.transform.position - new Vector3(0, 0, -10));
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * 20);
        // 射線碰撞偵測
        if (previousCube != null && previousCube.name != "Cube0")
            previousCube.GetComponent<Renderer>().material.mainTexture = Creator.Fill(Color.white);
        if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
        {
            previousCube = currentCube;
            currentCube = hit.transform.gameObject;

            float rate = (hit.transform.position.z - ray.origin.z) / (ray.direction.z * 20);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * (20 * rate));
            if (currentCube.name != "0")
                currentCube.GetComponent<Renderer>().material.mainTexture = Creator.Fill(Color.red);
        }
    }

}
