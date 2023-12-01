using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private int _score = 0;
    private void Start()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
    }
    public void UpdateScore(int clearLineCount)
    {
        if (clearLineCount <= 0)
            return;
        _score += 1 << (clearLineCount - 1);
        if (_score > 9999)
        {
            _score = 9999;
            _scoreText.color = Color.red;
        }
        Debug.Log($"{clearLineCount}: {_score}");
        _scoreText.text = _score.ToString();
    }
}
