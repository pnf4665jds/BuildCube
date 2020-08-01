using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TargetCubeEntity : GameEntityBase
{
    private GameObject cameraObject;
    private GameSceneRes sceneRes;

    public override void EntityInit()
    {
        base.EntityInit();
        sceneRes = GameEntityManager.Instance.GetCurrentSceneRes<GameSceneRes>();

        string deviceName = VRTK_SDKManager.instance.loadedSetup?.gameObject.name;
        if (deviceName == "VRSimulator")
        {
            cameraObject = sceneRes.SimulatorCamera.gameObject;
        }
        if (deviceName == "SteamVR")
        {
            cameraObject = sceneRes.SteamVRCamera.gameObject;
        }
    }

    private void Update()
    {
        if(cameraObject)
            transform.position = cameraObject.transform.position + new Vector3(0, -0.5f, 1f);
    }

    public override void EntityDispose()
    {
        
    }
}
