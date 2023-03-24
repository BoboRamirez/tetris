using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextInLineControl : MonoBehaviour
{
    private GameObject _instance = null;
    [SerializeField]
    private GameObject[] tetriminoList;
    public void UpdateNextZone(TetriminoType t)
    {
        Destroy( _instance );
        _instance = Instantiate(tetriminoList[(int)t], transform);
    }
}
