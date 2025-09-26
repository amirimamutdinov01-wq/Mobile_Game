using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessLife.Core
{
    /// <summary>
    /// Lightweight toast queue driving pooled text elements.
    /// </summary>
    public class ToastSystem : MonoBehaviour
    {
        [SerializeField] private Text toastPrefab;
        [SerializeField] private Transform toastRoot;
        [SerializeField] private float lifetime = 2.5f;

        private readonly Queue<Text> _pool = new();

        /// <summary>
        /// Displays a toast if a message exists.
        /// </summary>
        public void Show(string message)
        {
            if (string.IsNullOrEmpty(message) || toastPrefab == null || toastRoot == null)
            {
                return;
            }

            var text = GetText();
            text.text = message;
            text.gameObject.SetActive(true);
            CancelInvoke(nameof(HideOldest));
            Invoke(nameof(HideOldest), lifetime);
        }

        private Text GetText()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }

            return Instantiate(toastPrefab, toastRoot);
        }

        private void HideOldest()
        {
            foreach (Transform child in toastRoot)
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                    _pool.Enqueue(child.GetComponent<Text>());
                    break;
                }
            }
        }
    }
}
