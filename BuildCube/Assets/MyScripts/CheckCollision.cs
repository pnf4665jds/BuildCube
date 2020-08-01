using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public AudioClip SetCubeSound;  // 設置方塊音效

    // 小方塊的六個方位
    private Vector3[] position = { new Vector3(0.5f, 0, 0),
                                      new Vector3(-0.5f, 0, 0),
                                      new Vector3(0,  0.5f, 0),
                                      new Vector3(0, -0.5f, 0),
                                      new Vector3(0, 0,  0.5f),
                                      new Vector3(0, 0,  -0.5f)};

    private List<GameObject> checkPoints = new List<GameObject>();

    void Start()
    {
        CreateCheckPoint();
        CheckPointControl(true);
    }

    /// <summary>
    /// 建立六個檢查點
    /// </summary>
    protected void CreateCheckPoint()
    {
        // 產生六個檢查點用於判斷連接面
        for (int i = 0; i < 6; i++)
        {
            GameObject checkPoint = new GameObject("checkPoint" + i);
            checkPoint.transform.parent = transform;
            checkPoint.transform.localPosition = position[i];
            checkPoints.Add(checkPoint);
        }
    }

    /// <summary>
    /// 關閉連接面的檢查點
    /// </summary>
    /// <param name="index"></param>
    public void Connect(int index)
    {
        checkPoints[index].SetActive(false);
    }

    /// <summary>
    /// 開啟連接面的檢查點
    /// </summary>
    /// <param name="index"></param>
    public void Disconnect(int index)
    {
        checkPoints[index].SetActive(true);
    }

    /// <summary>
    /// 檢查這個方塊的六個面是否有相鄰的方塊
    /// </summary>
    /// <param name="connect"></param>
    public void CheckPointControl(bool connect)
    {
        for (int i = 0; i < 6; i++)
        {
            if (Physics.Raycast(transform.position, checkPoints[i].transform.position - transform.position, out RaycastHit hit, 1.5f, 1 << 8))
            {
                if (connect)
                {
                    GameAudioController.Instance.PlayOneShot(SetCubeSound);
                    Connect(i);
                    hit.collider.gameObject.GetComponent<CheckCollision>().Connect((i % 2 == 0) ? i + 1 : i - 1);
                }
                else
                {
                    Disconnect(i);
                    hit.collider.gameObject.GetComponent<CheckCollision>().Disconnect((i % 2 == 0) ? i + 1 : i - 1);
                }
            }
            //Debug.DrawRay(this.transform.position, checkPoints[i].transform.position - this.transform.position, Color.red, 3);
        }
        // 將此物件Layer設為Cube，讓接下來的方塊都能偵測到此方塊
        gameObject.layer = LayerMask.NameToLayer("Cube");
    }

    private void OnDestroy()
    {
        CheckPointControl(false);
    }
}
