using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextInLineControl : MonoBehaviour
{
    private GameObject _instance = null;
    [SerializeField]
    private GameObject[] tetriminoList;
    private Vector3 _nextPos = new Vector3(13, 16, 0);
    public void UpdateNextZone(TetriminoType t)
    {
        Destroy( _instance );
        _instance = Instantiate(tetriminoList[(int)t], _nextPos, Quaternion.identity);
        /*Debug.Log($"{transform.position}");*/
    }
}
