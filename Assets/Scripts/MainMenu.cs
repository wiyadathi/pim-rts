using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string newScene;
    public void NewGame()
    {
        SceneManager.LoadScene(newScene);

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    // Start i
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
