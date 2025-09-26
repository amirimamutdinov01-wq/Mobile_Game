using System.Collections.Generic;
using System.Linq;
using BusinessLife.Models;
using UnityEngine;

namespace BusinessLife.Core
{
    /// <summary>
    /// Loads all JSON data files under Resources/data.
    /// </summary>
    public class DataLoader : MonoBehaviour
    {
        public static DataLoader Instance { get; private set; }

        public ConfigData Config { get; private set; }
        public Dictionary<string, Industry> Industries { get; private set; } = new();
        public Dictionary<string, GameEvent> Events { get; private set; } = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAll();
        }

        /// <summary>
        /// Loads all configured JSON resources into memory.
        /// </summary>
        public void LoadAll()
        {
            Config = LoadJson<ConfigData>("data/config");
            LoadIndustries();
            LoadEvents();
        }

        private void LoadIndustries()
        {
            var text = Resources.Load<TextAsset>("data/industries");
            if (text == null)
            {
                Debug.LogError("Missing industries.json in Resources/data");
                return;
            }

            var wrapper = JsonUtility.FromJson<IndustryListWrapper>(text.text);
            Industries = wrapper.industries.ToDictionary(i => i.id, i => i);
        }

        private void LoadEvents()
        {
            Events.Clear();
            foreach (var industry in Industries.Values)
            {
                var asset = Resources.Load<TextAsset>($"data/events_{industry.id}");
                if (asset == null)
                {
                    Debug.LogWarning($"Missing events file for industry {industry.id}");
                    continue;
                }

                var wrapper = JsonUtility.FromJson<EventListWrapper>(asset.text);
                foreach (var evt in wrapper.events)
                {
                    Events[evt.id] = evt;
                }
            }
        }

        private static T LoadJson<T>(string path)
        {
            var asset = Resources.Load<TextAsset>(path);
            if (asset == null)
            {
                Debug.LogError($"Missing JSON resource at {path}");
                return default;
            }

            return JsonUtility.FromJson<T>(asset.text);
        }

        [System.Serializable]
        private class IndustryListWrapper
        {
            public List<Industry> industries;
        }

        [System.Serializable]
        private class EventListWrapper
        {
            public List<GameEvent> events;
        }
    }
}
