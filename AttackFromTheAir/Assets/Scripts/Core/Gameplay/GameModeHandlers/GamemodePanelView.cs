using Core.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class GamemodePanelView : MonoBehaviour
    {
        [SerializeField] private GameMode _gamemode;
        [SerializeField] private Image _background;
        [SerializeField] private Sprite _activeBG;
        [SerializeField] private Sprite _inactiveBG;
        public GameMode GameMode => _gamemode;

        public void Select()
        {
            _background.sprite = _activeBG;
        }

        public void Unselect()
        {
            _background.sprite = _inactiveBG;
        }
    }
}