using BusinessLife.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessLife.UI
{
    /// <summary>
    /// Displays ending summaries.
    /// </summary>
    public class EndingsView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Text title;
        [SerializeField] private Text description;

        /// <summary>
        /// Shows the ending information.
        /// </summary>
        public void Show(EndingResult result)
        {
            root.SetActive(true);
            title.text = result.id;
            description.text = result.description;
        }
    }
}
