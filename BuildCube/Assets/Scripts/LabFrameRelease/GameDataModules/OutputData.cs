using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSync;

public class OutputData : LabDataBase
{
    public float TotalTime;

    public OutputData(float time)
    {
        TotalTime = time;
    }
}
