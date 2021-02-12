using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public int ID;
    public int[] blocksId = new int[7];

    [HideInInspector]
    public float _saveScore = 0;
    [HideInInspector]
    public int _blockHealth = 0;
    [HideInInspector]
    public float _crystalsForGame = 0;
    [HideInInspector]
    public float _maxAmmo = 0;
}
