using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using System;

public class LikeButton : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void LikeScoreSave(int likeScore);
    private int likeScore = 0;  // ���� ����
    public TextMeshProUGUI scoreText;  // UI �ؽ�Ʈ ������Ʈ
    
    
    void Start()
    {
        scoreText.text = "��õ��: " + 0;
        if (scoreText != null)
        {
            UpdateScoreText();  // �ʱ� ���� ����
        }
        else
        {
            Debug.LogError("scoreText has not been assigned in the Inspector.");
        }
    }

    public void OnClick()
    {
        likeScore++;  // ������ 1 ����
        LikeScoreSave(likeScore);
        UpdateScoreText();  // �ؽ�Ʈ�� ������Ʈ
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = likeScore.ToString();  // �ؽ�Ʈ�� ���� ������ ������Ʈ
        }
    }
}
