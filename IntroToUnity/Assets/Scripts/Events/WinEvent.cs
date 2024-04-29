using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinEvent : MonoBehaviour
{
    int loadNextLevel;

    private void Start()
    {
        loadNextLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void PlayerWins()
    {
        SceneManager.LoadScene(loadNextLevel);
    }

}
