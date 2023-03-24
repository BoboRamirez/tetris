using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldZoneControl : MonoBehaviour
{
    private GameObject _instance = null;
    [SerializeField]
    private GameObject[] tetriminoList;
    private TetriminoType _instanceType;
    public void Exchange(Tetrimino tetrimino)
    {
        TetriminoType tmp;
        tmp = tetrimino.type;
        if (_instance == null )
        {
            tetrimino.EraseTetrimino();
            _instanceType = tetrimino.type;
            _instance = Instantiate(tetriminoList[(int) _instanceType], transform);
        }
        else
        {
            tetrimino.EraseTetrimino();
            tetrimino.InitializeTetrimino(_instanceType);
            Destroy(_instance);
            _instance = Instantiate(tetriminoList[(int) tmp], transform);
            _instanceType = tmp;
        }
    }
}
