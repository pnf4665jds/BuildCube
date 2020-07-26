using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : Singleton<CubeCreator>
{
    public GameObject NormalCubePrefab;

    /// <summary>
    /// 從Resource資料夾讀取方塊
    /// </summary>
    /// <param name="cubeName"></param>
    /// <returns></returns>
    public GameObject GetCubic(string cubeName)
    {
        Object obj = Resources.Load("Cubes/" + cubeName);
        GameObject cubeObj = Instantiate(obj) as GameObject;

        Renderer[] renderers = cubeObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer ren in renderers)
        {
            ren.material.mainTexture = Fill(Color.white);
        }
        cubeObj.transform.localScale = Vector3.one * 15;
        return cubeObj;
    }

    /// <summary>
    /// 創造方塊
    /// </summary>
    public GameObject CreateCube(GameObject center, Vector3 pos, Quaternion rotation)
    {
        GameObject cube = Instantiate(NormalCubePrefab);
        cube.GetComponent<Renderer>().material.mainTexture = Fill(new Color(1, 1, 1, 1));
        //cube.name = "Cube" + cubeID++;
        cube.transform.position = pos;
        cube.transform.rotation = rotation;
        cube.transform.parent = center.transform;
        cube.transform.localScale = new Vector3(1, 1, 1) * 0.05f;

        return cube;
    }

    /// <summary>
    /// 方塊上色
    /// </summary>
    public Texture2D Fill(Color clr)
    {
        Color color;
        Texture2D texture = new Texture2D(128, 128);
        int y = 0;

        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                if (x <= 9 || y <= 9 || x >= 118 || y >= 118)
                    color = Color.black;
                else
                    color = clr;
                texture.SetPixel(x, y, color);
                ++x;
            }
            ++y;
        }
        texture.Apply();
        return texture;
    }
}
