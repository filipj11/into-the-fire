using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int progressMax;
}
