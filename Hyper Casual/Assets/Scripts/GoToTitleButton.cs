using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTitleButton : MonoBehaviour
{
    [SerializeField] private Button TitleButton;

    private void Awake()
    {
        TitleButton.onClick.RemoveListener(GoToTitle);
        TitleButton.onClick.AddListener(GoToTitle);
    }

    private void GoToTitle()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}
