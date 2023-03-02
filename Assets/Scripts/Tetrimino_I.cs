using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino_I : Tetrimino
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Rotate(bool isCLockwise)
    {
        return;
    }

    public override MatCoor[] GetMinoPos(RotationState r, MatCoor p)
    {
        throw new System.NotImplementedException();
    }


}
