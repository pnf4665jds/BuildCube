using System.Collections;
using System.Collections.Generic;
using GameData;
using LabData;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainUI : MonoBehaviour
{
    public InputField IdField;
    public Button StartButton, QuitButton;
    public Dropdown CubeDropdown;
    public GameObject CubeParent;

    private GameObject cube;
    private IEnumerator rotate;

    private void Start()
    {
        // 回到UI時開啟鼠標
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartButton.onClick.AddListener(delegate
        {
            if (CubeDropdown.captionText.text != "--")
            {
                // 生成一個GameFlowData來儲存所需資料
                GameFlowData flowData = new GameFlowData();
                flowData.UserId = IdField.text;
                flowData.TargetCubeName = CubeDropdown.captionText.text;
                // 存放在GameDataManager讓其他場景存取
                GameDataManager.FlowData = flowData;
                GameDataManager.LabDataManager.LabDataCollectInit(() => GameDataManager.FlowData.UserId);
                GameSceneManager.Instance.Change2MainScene();
            }
        });

        QuitButton.onClick.AddListener(delegate
        {
            Application.Quit();
        });

        CubeDropdown.onValueChanged.AddListener(delegate
        {
            if (CubeDropdown.captionText.text != "--")
            {
                LoadCube(CubeDropdown.captionText.text);
            }
            else
            {
                if (cube)
                    Destroy(cube);
                if (rotate != null)
                    StopCoroutine(rotate);
            }
        });
    }

    /// <summary>
    /// 讀取方塊
    /// </summary>
    /// <param name="name"></param>
    private void LoadCube(string name)
    {
        if (cube)
            Destroy(cube);
        cube = CubeCreator.instance.GetCubic(name);
        cube.transform.position = new Vector3(6, 0, 0);
        if (rotate != null)
            StopCoroutine(rotate);
        rotate = Rotate();
        StartCoroutine(rotate);
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            yield return null;
            cube.transform.Rotate(new Vector3(0, -1, 0));
        }
    }
}
