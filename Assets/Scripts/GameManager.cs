using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public int difficulty = 1;
    public GameState state = GameState.defaultPhase;
    private float fallCounter = 0;
    private float lockTimer = 0.5f;
    [SerializeField]
    private GameObject tetriminoGO;
    private Tetrimino tetrimino;
    [SerializeField]
    private GameObject spawnerGO;
    private Spawner spawner;
    private float FallDelay
    {
        get
        {
            return Data.fallDelay[difficulty];
        }
    }
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
        tetrimino = tetriminoGO.GetComponent<Tetrimino>();
        spawner = spawnerGO.GetComponent<Spawner>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
