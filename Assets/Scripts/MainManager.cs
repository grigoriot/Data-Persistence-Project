using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private static string path;

    private SaveData highscore;


    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/savefile.json";
        ScoreText.text = StartManager.Instance.Username + " : Score : 0";
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        
        LoadHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        if (m_Points > highscore.highscore)
        {
            highscore.username = StartManager.Instance.Username;
            highscore.highscore = m_Points;
        }

        HighScoreText.text = "Best Score : " + highscore.username + " : " + highscore.highscore;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        //ScoreText.text = $"Score : {m_Points}";
        ScoreText.text = ScoreText.text.Replace((m_Points-point).ToString(), m_Points.ToString());
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScore(highscore.username, highscore.highscore);
    }

    [System.Serializable]
    public class SaveData
    {
        public string username;
        public int highscore;
    }

    public void SaveHighScore(string username, int newScore)
    {
        SaveData data = new SaveData();
        data.username = username;
        data.highscore = newScore;

        string json = JsonUtility.ToJson(data);
        
        File.WriteAllText(path, json);
    }

    public void LoadHighScore()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highscore = data;
        }
        else
        {
            highscore = new SaveData();
            highscore.username = StartManager.Instance.Username;
            highscore.highscore = 0;
        }
    }
}
