using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;

public class NextInLineControl : MonoBehaviour
{
    private GameObject _instance = null;
    [SerializeField]
    private GameObject[] tetriminoList;
    private Vector3 _nextPos = new Vector3(12.5f, 16, 0);
    public void UpdateNextZone(TetriminoType t)
    {
        Destroy( _instance );
        _instance = Instantiate(tetriminoList[(int)t], _nextPos, Quaternion.identity);
        Enlight(_instance, t);
        /*Debug.Log($"{transform.position}");*/
    }
    public void Enlight(GameObject i, TetriminoType t)
    {
        foreach (MeshRenderer renderer in i.GetComponentsInChildren<MeshRenderer>())
        {
            //Debug.Log(renderer.gameObject);
            Material mat = renderer.sharedMaterial;
            if (mat.enableInstancing)
            {
                MaterialPropertyBlock props = new();
                //Debug.Log($"{Data.TetriminoColor[t]}");
                props.SetColor("_color", Data.TetriminoColor[t]);
                props.SetFloat("_fresnelIntensity", Data.MinoIntensity);
                props.SetFloat("_thresh", Data.MinoThreshold);
                renderer.SetPropertyBlock(props);
            }
            else
            {
                //Debug.Log("zako zako~");
                mat.SetColor("_color", Data.TetriminoColor[t]);
                mat.SetFloat("_fresnelIntensity", Data.MinoIntensity);
                mat.SetFloat("_thresh", Data.MinoThreshold);
            }
        }
    }
}
