using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Mino _m;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("meow");
        _m.Initialized(10, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
