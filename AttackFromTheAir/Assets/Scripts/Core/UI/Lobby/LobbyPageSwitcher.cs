using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class LobbyPageSwitcher : MonoBehaviour 
    {
        [SerializeField] private LobbyTabToggles _lobbyTabToggles;
        [SerializeField] private List<LobbyPageDescriptor> _pages;

        private void OnEnable()
        {
            _lobbyTabToggles.OnLobbyTabChangedEvent += LobbyTabChangedHandler;
        }

        private void LobbyTabChangedHandler(LobbyTabs tab)
        {
            foreach (LobbyPageDescriptor page in _pages) 
            {
                page.Page.SetActive(page.Tab == tab);
            }
        }

        private void OnDisable()
        {   
            _lobbyTabToggles.OnLobbyTabChangedEvent -= LobbyTabChangedHandler;
        }
    }

    [System.Serializable]
    public class LobbyPageDescriptor
    {
        public LobbyTabs Tab;
        public GameObject Page;
    }
}