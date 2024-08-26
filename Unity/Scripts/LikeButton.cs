using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using System;

public class LikeButton : MonoBehaviour
{
    [Serializable]
    public class JsonList
    {
        public string name;
        public string wall;
        public string floor;
        public string furniture;
    }

    [DllImport("__Internal")]
    private static extern void LikeScoreSave(int likeScore);
    [DllImport("__Internal")]
    public static extern void DelilveryJson(string json);
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
        JsonList jsonList = new JsonList(); // ������ Json���� ��ȯ
        jsonList.name = "Player";
        jsonList.wall = CafeDecorator.saveWall;
        jsonList.floor = CafeDecorator.saveFloor;
        jsonList.furniture = FurniturePlacer.saveFurniture;
        string json = JsonUtility.ToJson(jsonList);
        DelilveryJson(json);
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = likeScore.ToString();  // �ؽ�Ʈ�� ���� ������ ������Ʈ
        }
    }
}
