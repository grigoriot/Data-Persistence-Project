using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public InputField InputField;

    public Button startButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUsername()
    {
        string text = InputField.text;

        StartManager.Instance.Username = text;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
