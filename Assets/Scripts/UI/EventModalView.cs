using System;
using System.Collections.Generic;
using BusinessLife.Models;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessLife.UI
{
    /// <summary>
    /// Handles binding event data to the modal UI.
    /// </summary>
    public class EventModalView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Text title;
        [SerializeField] private Text description;
        [SerializeField] private Button[] choiceButtons;

        public event Action<Choice> OnChoiceSelected;

        private List<Choice> _currentChoices = new();

        /// <summary>
        /// Presents the given event to the user.
        /// </summary>
        public void Show(GameEvent gameEvent)
        {
            root.SetActive(true);
            title.text = gameEvent.title;
            description.text = gameEvent.description;
            _currentChoices = gameEvent.choices;

            for (var i = 0; i < choiceButtons.Length; i++)
            {
                var button = choiceButtons[i];
                if (i < _currentChoices.Count)
                {
                    var choice = _currentChoices[i];
                    button.gameObject.SetActive(true);
                    button.GetComponentInChildren<Text>().text = choice.label;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => OnChoice(choice));
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Hides the modal.
        /// </summary>
        public void Hide()
        {
            root.SetActive(false);
        }

        private void OnChoice(Choice choice)
        {
            OnChoiceSelected?.Invoke(choice);
        }
    }
}
