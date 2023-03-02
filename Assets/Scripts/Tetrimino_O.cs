using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino_O : Tetrimino
{
    private void Start()
    {
        InitializeTetrimino(TetriminoType.O);
    }
    
    public override void Rotate(bool isClockwise)
    {
        RotationState oldOrientation = orientation;
        Dictionary<RotationState, MatCoor[]> offsetData = Data.GetOffsetData(type);
        MatCoor offset;
        
        MatrixManager.manager.ClearBlocks(minos);

        orientation += isClockwise ? 1 : -1;
        if (orientation > RotationState.west)
            orientation = RotationState.north;
        else if (orientation < RotationState.north)
            orientation = RotationState.west;
        offset = offsetData[oldOrientation][0] - offsetData[orientation][0];
        //to keep rotation center from shifting an offset during rotation, have to use 2 for-loops...
        for (int i = 0; i < 4; i++)
            minos[i] = (minos[i] - minos[rotationCenter]).Rotate90(isClockwise) + minos[rotationCenter];
        for (int i = 0; i < 4; i++)
            minos[i] += offset;

        MatrixManager.manager.UpdateTetrimino(minos);
        tetriminoPiece.transform.SetPositionAndRotation(Data.GetUnityPos(orientation, minos[0]),
                                                        isClockwise ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90));
        return;
    }
    public override MatCoor[] GetMinoPos(RotationState r, MatCoor p)
    {
        MatCoor[] c = new MatCoor[4];
        c[0] = p;
        switch (r)
        {
            case RotationState.north:
                c[1] = new MatCoor(p, 1, 0);
                c[2] = new MatCoor(p, 0, -1);
                c[3] = new MatCoor(p, 1, -1);
                break;
            case RotationState.east:
                c[1] = new MatCoor(p, 0, -1);
                c[2] = new MatCoor(p, -1, 0);
                c[3] = new MatCoor(p, -1, -1);
                break;
            case RotationState.south:
                c[1] = new MatCoor(p, -1, 0);
                c[2] = new MatCoor(p, 0, 1);
                c[3] = new MatCoor(p, -1, 1);
                break;
            case RotationState.west:
                c[1] = new MatCoor(p, 0, 1);
                c[2] = new MatCoor(p, 1, 0);
                c[3] = new MatCoor(p, 1, 1);
                break;
            default:
                throw (new System.Exception("Wrong Tetrimino Orientation in GetMinoPos!"));
        }
        return c;
    }
}
