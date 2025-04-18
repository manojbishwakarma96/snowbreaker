using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when Play button is clicked
    public void PlayGame()
    {
        Debug.Log("Loading game scene...");
        try
        {
            SceneManager.LoadScene("Game");  
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load Game scene: " + e.Message);
        }
    }

    // Called when Settings button is clicked
    public void OpenSettings()
    {
        Debug.Log("Opening settings...");
        try
        {
            SceneManager.LoadScene("Settings");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load Settings: " + e.Message);
        }
    }

    // Called when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // For testing - print current scene name
    private void Start()
    {
        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);
    }
}
