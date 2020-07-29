using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class BuildTask : MonoBehaviour
{
    public EditUI EditUI;

    private GameObject TargetCube;     // 要蓋的目標方塊
    private GameObject BuildingCube;   // 手上的方塊
    private List<float> TargetDistanceList;  // 紀錄目標方塊中兩兩小方塊的彼此距離，並排序
    private List<float> BuildingDistanceList;  // 紀錄目標方塊中兩兩小方塊的彼此距離，並排序
    private bool IsFinshed = false;
    private Vector3 cameraPosition;
    private IEnumerator timer;

    private void Start()
    {
        SetTargetCube();
        timer = EditUI.Timer();
        StartCoroutine(timer);
        StartCoroutine(Process());
    }

    private void Update()
    {
        if (TargetCube)
        {
            GetCamera();
            TargetCube.transform.position = cameraPosition + new Vector3(0, -0.5f, 1f);
        }
    }

    private void GetCamera()
    {
        string deviceName = VRTK_SDKManager.instance.loadedSetup?.gameObject.name;
        if (deviceName == "VRSimulator")
        {
            cameraPosition = EditUI.SimulatorCamera.gameObject.transform.position;
        }
        if (deviceName == "SteamVR")
        {
            cameraPosition = EditUI.SteamVRCamera.gameObject.transform.position;
        }
    }

    /// <summary>
    /// 設置TargetCube及數據
    /// </summary>
    private void SetTargetCube()
    {
        TargetCube = CubeCreator.instance.GetCubic(GameData.instance.Data.TargetCube);
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
    /// 遊戲運行
    /// </summary>
    /// <returns></returns>
    private IEnumerator Process()
    {
        yield return null;
        BuildingCube = GameObject.FindWithTag("MainCubic");
        while (!IsFinshed)
        {
            // 下面這行是避免CheckIfSame回傳False時進入無限迴圈
            yield return new WaitUntil(() => TargetCube.transform.childCount != BuildingCube.transform.childCount);
            // 等到玩家拼出同樣數量的小方塊時才判斷
            yield return new WaitUntil(() => TargetCube.transform.childCount == BuildingCube.transform.childCount);
            IsFinshed = CheckIfSame();
        }
        Debug.Log("win");
        StopCoroutine(timer);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainUI");
    }

    /// <summary>
    /// 設置TargetCube及數據
    /// </summary>
    public bool CheckIfSame()
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
                return false;
        }

        return true;
    }
}
