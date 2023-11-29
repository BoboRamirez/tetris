using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class Mino : MonoBehaviour
{
    private BlockState _state;
    public BlockState State { get { return _state; } }
    public MatCoor coordinate;
    //public Material material;
    //public GameObject minoGO;
    [SerializeField]private MeshRenderer _render;
    private Material _mat;
    [SerializeField] private TetriminoType _type;
    public TetriminoType Type { get {  return _type; } }
    public void Initialized(int x, int y)
    {
        coordinate = new MatCoor(x, y);
        _mat = _render.sharedMaterial;
        //Debug.Log(_mat.GetColor("_color"));
        SetState(BlockState.available);
        //Debug.Log("init done!");
    }

    /*public void SetState(BlockState s)
    {
        _state = s;
        switch (s)
        {
            case BlockState.available:
                SetMinoMat(1, 1, Color.white);
                break;
            case BlockState.ghost:
                SetMinoMat(1.1f, 0.88f, Color.white);
                break;
            default:
                Debug.Log("wrong state perhaps?");
                break;
        }
    }*/

    public void SetState(BlockState s, TetriminoType t = TetriminoType.O)
    {
        _state = s;
        _type = t;
        switch (s)
        {
            case BlockState.available:
                SetMinoMat(1, 1, Color.white);
                return;
            case BlockState.ghost:
                SetMinoMat(1.1f, 0.88f, Color.white);
                return;
            default:
                SetMinoMat(Data.MinoIntensity, Data.MinoThreshold, Data.TetriminoColor[t]);
                return;
        }
        
        /*switch (t)
        {
            case TetriminoType.O:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0.996f, 0.772f, 0.043f));
                break;
            case TetriminoType.I:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0.129f, 0.631f, 0.867f));
                break;
            case TetriminoType.J:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0f, 0.416f, 0.643f));
                break;
            case TetriminoType.L:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0.820f, 0.506f, 0.165f));
                break;
            case TetriminoType.S:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0.424f, 0.663f, 0.235f));
                break;
            case TetriminoType.T:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0.478f, 0.125f, 0.482f));
                break;
            case TetriminoType.Z:
                SetMinoMat(_minoIntensity, minoThresh, new Color(0.765f, 0.145f, 0.125f));
                break;
            default:
                Debug.Log("wrong type perhaps?");
                break;
        }*/
    }

    private void SetMinoMat(float intensity = 1, float threshold = 0, Color color = default)
    {
        if (_mat.enableInstancing)
        {
            MaterialPropertyBlock props = new();
            props.SetColor("_color", color);
            props.SetFloat("_fresnelIntensity", intensity);
            props.SetFloat("_thresh", threshold);
            _render.SetPropertyBlock(props);
        }
        else
        {
            _mat.SetColor("_color", color);
            _mat.SetFloat("_fresnelIntensity", intensity);
            _mat.SetFloat("_thresh", threshold);
        }
    }
}

/*public struct MinoShape
{
    public float Intensity;
    public float Threshhold;
    public Vector3 Color;
    public MinoShape(float Intensity, float Threshold, Vector3 Color)
    {
        this.Intensity = Intensity;
        this.Threshhold = Threshold;
        this.Color = Color;
    }
}*/
