using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    private Vector3[] position = { new Vector3(0.025f, 0, 0),
                                      new Vector3(-0.025f, 0, 0),
                                      new Vector3(0,  0.025f, 0),
                                      new Vector3(0, -0.025f, 0),
                                      new Vector3(0, 0,  0.025f),
                                      new Vector3(0, 0,  -0.025f)};

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
        // 產生檢查點用於判斷碰撞面
        for (int i = 0; i < 6; i++)
        {
            GameObject checkPoint = new GameObject("checkPoint" + i);
            checkPoint.transform.position = this.transform.position + this.transform.parent.rotation * position[i];
            checkPoint.transform.parent = this.transform;
            checkPoints.Add(checkPoint);
        }
    }

    // 關閉檢查點
    public void Connect(int index)
    {
        checkPoints[index].SetActive(false);
    }

    // 開啟檢查點
    public void Disconnect(int index)
    {
        checkPoints[index].SetActive(true);
    }

    /// <summary>
    /// 控制方塊的檢查點
    /// </summary>
    /// <param name="connect"></param>
    public void CheckPointControl(bool connect)
    {
        RaycastHit hit;

        for (int i = 0; i < 6; i++)
        {
            if (Physics.Raycast(transform.position, checkPoints[i].transform.position - this.transform.position, out hit, 1.5f, LayerMask.NameToLayer("Cube")))
            {
                if (connect)
                {
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
        this.gameObject.layer = LayerMask.NameToLayer("Cube");
    }

    private void OnDestroy()
    {
        CheckPointControl(false);
    }
}
