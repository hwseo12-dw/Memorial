using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;

public class LikeButton : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void LikeScoreSave(string userName, int likeScore);
    private int likeScore = 0;  // ���� ����
    public TextMeshProUGUI scoreText;  // UI �ؽ�Ʈ ������Ʈ
    public string Player;
    
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").name;
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
        LikeScoreSave(Player, likeScore);
        UpdateScoreText();  // �ؽ�Ʈ�� ������Ʈ
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "��õ��: " + likeScore.ToString();  // �ؽ�Ʈ�� ���� ������ ������Ʈ
        }
    }
}
