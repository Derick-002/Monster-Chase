using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    public void RestartGame()
    {
        // SceneManager.LoadScene("GamePlay");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
