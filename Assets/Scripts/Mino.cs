using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    private BlockState _state;
    public BlockState State { get { return _state; } }
    public MatCoor coordinate;
    //public Material material;
    //public GameObject minoGO;
    private SpriteRenderer spriteRenderer;
    public void Initialized(int x, int y)
    {
        coordinate = new MatCoor(x, y);
        _state = BlockState.available;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    public void SetState(BlockState s)
    {
        _state = s;
        //will have different sprite colors for them later.
        switch (s)
        {
            case BlockState.available:
                spriteRenderer.enabled = false;
                break;
            case BlockState.active0:
            case BlockState.active1:
            case BlockState.active2:
            case BlockState.active3:
                spriteRenderer.enabled = true;
                break;
            case BlockState.locked:
                spriteRenderer.enabled = true;
                break;
        }
    }
}
