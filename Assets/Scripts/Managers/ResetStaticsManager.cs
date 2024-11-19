using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticsManager : MonoBehaviour
{
    private void Awake()
    {
        CuttingCounter.ResetStatics();
        BaseCounter.ResetStatics();
        TrashCan.ResetStatics();
    }
}
