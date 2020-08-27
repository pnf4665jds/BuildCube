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
    public Text WarningText;

    private GameObject cube;
    private IEnumerator rotate;

    private void Start()
    {
        // 回到UI時開啟鼠標
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartButton.onClick.AddListener(delegate
        {
            if (string.IsNullOrEmpty(IdField.text))
            {
                WarningText.text = "請輸入ID";
                OpenWarning();
            }
            else if (CubeDropdown.captionText.text == "--")
            {
                WarningText.text = "請選擇方塊";
                OpenWarning();
            }
            else
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
        cube.transform.position = new Vector3(388, -77, 514);
        cube.transform.localScale *= 50;
        if (rotate != null)
            StopCoroutine(rotate);
        rotate = Rotate();
        StartCoroutine(rotate);
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            cube.transform.Rotate(new Vector3(0, -1, 0));
        }
    }

    public void OpenWarning()
    {
        WarningText.transform.parent.gameObject.SetActive(true);
    }

    public void CloseWarning()
    {
        WarningText.transform.parent.gameObject.SetActive(false);
    }
}
