using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPiece : MonoBehaviour
{
    public MatCoor[] minoCoordinates = new MatCoor[4];
    private MatrixManager matrix;
    private void Start()
    {
        matrix = MatrixManager.manager;
    }
    public void ConjureGhostPiece(MatCoor[] m)
    {
        minoCoordinates = m;
        matrix.ChannelGhostPiece(m);
    }
    public void UpdateGhostPiece(MatCoor[] m)
    {
        matrix.ClearBlocks(minoCoordinates);
        minoCoordinates = m;
        matrix.ChannelGhostPiece(minoCoordinates);
    }
    public void PurgeGhostPiece()
    {
        matrix.ClearBlocks(minoCoordinates);
    }
    public void PrintMinoCoordinates()
    {
        foreach (MatCoor c in minoCoordinates)
            Debug.Log(c);
    }
}
