using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldZoneControl : MonoBehaviour
{
    private GameObject _instance = null;
    private Vector3 _holdPos = new Vector3(-3, 16, 0);
    [SerializeField] private GameObject[] tetriminoList;
    private TetriminoType _instanceType;
    public void Exchange(Tetrimino tetrimino)
    {
        TetriminoType tmp;
        tmp = tetrimino.type;
        if (_instance == null )
        {
            tetrimino.EraseTetrimino();
            _instanceType = tetrimino.type;
            _instance = Instantiate(tetriminoList[(int) _instanceType], _holdPos, Quaternion.identity);
        }
        else
        {
            tetrimino.EraseTetrimino();
            tetrimino.InitializeTetrimino(_instanceType);
            Destroy(_instance);
            _instance = Instantiate(tetriminoList[(int) tmp], _holdPos, Quaternion.identity);
            _instanceType = tmp;
        }
    }
}
