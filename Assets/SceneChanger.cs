using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void changeScene(string name, LoadSceneMode mode)
    {
        SceneManager.LoadScene(name,mode);
    }
}
