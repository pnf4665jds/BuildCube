using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public AudioClip setCubeSE;     // 設置方塊的音效

    private BuildCube buildCube;
    private Vector3 spawnPosition;
    private Collider targetCollider;

    private void Start()
    {
        buildCube = GameObject.FindWithTag("MainCamera").GetComponent<BuildCube>();
    }

    private void Update()
    {
        // 按下特定按鈕時才擺上方塊
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetCube();
        }
        targetCollider = CollisionEnterCheck();
    }

    /// <summary>
    /// 取得最接近的碰撞方塊
    /// </summary>
    private Collider CollisionEnterCheck()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, 1 << 8);

        Collider nearestCollider = null;
        float minDistance = 1000;
        foreach (Collider c in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, c.gameObject.transform.position);

            if (distance < minDistance)
            {
                nearestCollider = c;
                minDistance = distance;
            }
        }
        return nearestCollider;
    }

    /// <summary>
    /// 檢查方塊的碰撞面
    /// </summary>
    private Vector3 CheckCollisionPoint(GameObject coObject)
    {
        Transform[] transforms = coObject.GetComponentsInChildren<Transform>();
        int targetIndex = 0;
        Vector3 center = transforms[0].position;
        float minDistance = 1000;
        // 計算碰撞點離哪個檢查點最近
        for (int i = 1; i < transforms.Length; i++)
        {
            float distance = Vector3.Distance(transforms[i].position, gameObject.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetIndex = i;
            }
        }

        return transforms[targetIndex].position * 2 - center;
    }

    /// <summary>
    /// 設置方塊
    /// </summary>
    public void SetCube()
    {
        // 取得產生方塊的座標
        if (targetCollider != null)
        {
            spawnPosition = CheckCollisionPoint(targetCollider.gameObject);
        }

        if (targetCollider != null)
        {
            //GameAudioController.Instance.PlayOneShot(setCubeSE);
            buildCube.Create(spawnPosition, targetCollider.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
