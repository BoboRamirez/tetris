using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countDownText;
    private int n = 11;
    private IEnumerator _countDownCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _countDownCoroutine = CountDown();
    }

    public void StartCountDown()
    {
        //Debug.Log("Coroutine start!");
        StartCoroutine(_countDownCoroutine);
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            n--;
            if (n < 0)
            {
                OnClickNo();
                yield break;
            }
            //Debug.Log($"Set to {n}");
            _countDownText.text = n.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    public void OnClickYes()
    {
        //Debug.Log("YESYESYES");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene("Stage");
    }

    public void OnClickNo()
    {
        SceneManager.LoadScene("Welcome");
    }
}
