using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public int difficulty = 1;
    public GameState state = GameState.defaultPhase;
    private float fallCounter = 0;
    [SerializeField]    private Tetrimino tetrimino;
    [SerializeField]    private Spawner spawner;
    [SerializeField]    private HoldZoneControl holder;
    [SerializeField] private Animator _popupAnimator;
    private float FallDelay
    {
        get
        {
            return Data.fallDelay[difficulty];
        }
    }
    private bool canHold, _doPause;
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            //DontDestroyOnLoad(this);
        }
        else if (manager != this)
            Destroy(gameObject);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        MatrixManager.manager.InitializeMatrix();
        _doPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tetrimino.IsActive)
        {
            if (tetrimino.LockTimer <= 0 || tetrimino.OperationCounter <= 0)
            {
                tetrimino.HardDrop();
                if (!tetrimino.Lock())
                {
                    ExceedRootGameOver();
                    return;
                }
            }
            else if (!tetrimino.HasSpaceToFall())
            {
                tetrimino.LockTimerCountDown();
            }

            if (fallCounter >= Data.fallDelay[difficulty])
            {
                fallCounter -= Data.fallDelay[difficulty];
                tetrimino.Fall();
            }
            else
                fallCounter += Time.deltaTime;
        }
        else if (_doPause)
        {
            return;
        }
        else
        {
            spawner.Spawn();
            canHold = true;
        }
    }
    
    public void MoveLeft(InputAction.CallbackContext context)
    {
/*        Debug.Log(tetrimino.IsActive);*/
        if (tetrimino.IsActive)
        {
            if (context.phase == InputActionPhase.Started)
            {
                tetrimino.Move(true);
                //Debug.Log("start");
            }
            else if (context.phase == InputActionPhase.Performed)
            {
                StartCoroutine(RepeatMoving(true, context));
                //Debug.Log("performed");
            }
        }
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
     /*   Debug.Log(tetrimino.IsActive);*/
        if (tetrimino.IsActive)
        {
            if (context.phase == InputActionPhase.Started)
            {
                //Debug.Log("start");
                tetrimino.Move(false);
            }
            else if (context.phase == InputActionPhase.Performed)
            {
               // Debug.Log("performed");
                StartCoroutine(RepeatMoving(false, context));
            }
        }

    }

    private IEnumerator RepeatMoving(bool isLeft, InputAction.CallbackContext context)
    {
        while (tetrimino.IsActive && context.phase == InputActionPhase.Performed)
        {
            tetrimino.Move(isLeft);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    public void SoftDrop(InputAction.CallbackContext context)
    {
        if (tetrimino.IsActive)
        {
            if (context.phase == InputActionPhase.Performed)
                StartCoroutine(RepeatFalling(context));
        }
    }

    private IEnumerator RepeatFalling(InputAction.CallbackContext context)
    {
        while (tetrimino.IsActive && context.phase == InputActionPhase.Performed)
        {
            tetrimino.Fall();
            yield return new WaitForSeconds(Data.fallDelay[difficulty] / 20);
        }
        yield break;
    }

    public void RotateClockwise(InputAction.CallbackContext context)
    {
        if (tetrimino.IsActive && context.phase == InputActionPhase.Started)
        {
            tetrimino.Rotate(true);
        }
    }

    public void RotateCounterClockwise(InputAction.CallbackContext context)
    {
        if (tetrimino.IsActive && context.phase == InputActionPhase.Started)
        {
            tetrimino.Rotate(false);
        }
    }

    public void HardDrop(InputAction.CallbackContext context)
    {
        if (tetrimino.IsActive && context.phase == InputActionPhase.Performed)
        {
            tetrimino.HardDrop();
            if (!tetrimino.Lock())
                ExceedRootGameOver();
        }
    }

    public void SpawnTetrimino(InputAction.CallbackContext context)
    {
        //Debug.Log(tetrimino.IsActive);
        /*
        if (!tetrimino.IsActive && context.phase == InputActionPhase.Performed)
        {
            spawner.Spawn();

        }*/
    }

    public void ExceedRootGameOver()
    {
        //Debug.Log("game over!");
        _doPause = true;
        _popupAnimator.SetBool("isOver", true);
        return;
    }

    public void Hold(InputAction.CallbackContext context)
    {
        if (canHold && context.phase == InputActionPhase.Performed)
        {
            holder.Exchange(tetrimino);
            canHold = false;
        }
    }
}
