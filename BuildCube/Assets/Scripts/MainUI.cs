using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public InputField IdField;
    public Button StartButton;
    public Dropdown CubeDropdown;
    public CubeCreator Creator;
    public GameObject CubeParent;

    private GameObject cube;

    private void Start()
    {
        StartButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("GameScene");
        });

        CubeDropdown.onValueChanged.AddListener(delegate
        {
            if (CubeDropdown.captionText.text != "--")
                LoadCube(CubeDropdown.captionText.text);
        });
    }

    private void LoadCube(string name)
    {
        if (cube)
            Destroy(cube);
        cube = Creator.GetCubic(name);
        cube.transform.position = new Vector3(6, 0, 0);
    }
}
