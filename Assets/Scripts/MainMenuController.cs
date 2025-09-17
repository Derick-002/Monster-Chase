using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public void PlayerGame()
    {
        //ndx is for the selected char 0 for player1 and 1 for player2;
        int ndx = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log("Index: " + ndx);

        GameManager.instance.CharIndex = ndx;

        SceneManager.LoadScene("GamePlay");
    }
}
