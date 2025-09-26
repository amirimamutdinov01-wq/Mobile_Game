using UnityEngine;
using UnityEngine.SceneManagement;

namespace BusinessLife.UI
{
    /// <summary>
    /// Handles main menu buttons.
    /// </summary>
    public class MainMenuView : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene("NewGame");
        }

        public void ContinueGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void OpenSettings()
        {
            // placeholder for settings panel toggle
            Debug.Log("Settings tapped");
        }
    }
}
