using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    public RectTransform GameOverPanel;
    public Text OverText;


    private void Awake()
    {
        Instance = this;
    }

    public void Goal()
    {
        OverText.text = "Complete Load";
        GameOverPanel.gameObject.SetActive(true);
    }

    public void Fail()
    {
        OverText.text = "Complete Load";
        GameOverPanel.gameObject.SetActive(true);
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
