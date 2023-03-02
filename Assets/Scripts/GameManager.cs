using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public int difficulty = 1;
    public GameState state = GameState.defaultPhase;
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else if (manager != this)
            Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        MatrixManager.manager.InitializeMatrix();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
