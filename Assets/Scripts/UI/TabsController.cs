using UnityEngine;

namespace BusinessLife.UI
{
    /// <summary>
    /// Basic tab switching logic for dashboard placeholders.
    /// </summary>
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private GameObject[] tabs;

        private int _activeIndex;

        /// <summary>
        /// Selects the tab by index.
        /// </summary>
        public void Select(int index)
        {
            _activeIndex = index;
            for (var i = 0; i < tabs.Length; i++)
            {
                if (tabs[i] != null)
                {
                    tabs[i].SetActive(i == _activeIndex);
                }
            }
        }
    }
}
