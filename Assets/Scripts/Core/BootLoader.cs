using UnityEngine;
using UnityEngine.SceneManagement;

namespace BusinessLife.Core
{
    /// <summary>
    /// Boot strapper ensuring shared services exist before loading the menu.
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        [SerializeField] private string nextScene = "MainMenu";

        private void Start()
        {
            if (FindObjectOfType<DataLoader>() == null)
            {
                var go = new GameObject("DataLoader");
                go.AddComponent<DataLoader>();
            }

            SceneManager.LoadScene(nextScene);
        }
    }
}
