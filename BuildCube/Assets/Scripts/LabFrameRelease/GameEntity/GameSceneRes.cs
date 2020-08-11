using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GameSceneRes : GameSceneEntityRes
{
    // 場景中存取SceneRes的方法：
    // GameEntityManager.Instance.GetCurrentSceneRes<MatchSceneRes>();

    public EditUI EditUI;   // 遊戲場景UI
    public VRTK_Pointer Pointer;    // 刪除模式射線
    public GameObject SimulatorCamera, SteamVRCamera;   // 攝影機
    public VRTK_ControllerEvents LeftEvent, RightEvent;     // 手把控制事件腳本
    public GameObject SetCubePrefab;        // 用於擺設的方塊
    public GameObject MainCubicPrefab;      // 被組合的大方塊
    public GameObject SimulatorRightHand;   // VR Simulator的右手柄
    public GameObject SteamVRRightHand;     // SteamVR的右手柄
    public BuildTask MainTask;      // 管理遊戲流程的腳本
    public BuildCubeTask BuildCubeTask; // 用於管理方塊的組裝

    public AudioClip GameInstructionVoice;  // 遊戲說明語音
    public AudioClip FinishSound;   // 完成音效

}
