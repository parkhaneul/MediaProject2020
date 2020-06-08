using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public RectTransform GameOver_GoalPanel;
    public RectTransform GameOver_FailPanel;
    void Start()
    {
        
    }

    public void Goal()
    {
        GameOver_GoalPanel.gameObject.SetActive(true);
    }

    public void Fail()
    {
        GameOver_FailPanel.gameObject.SetActive(true);
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
