using System.Collections.Generic;
using Core.MVP;
using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ChestsRewardPanel : MonoBehaviour
    {
        [SerializeField] private Button _okButton;
        [SerializeField] private RewardPanel _pewardPanel;
        [SerializeField] private RectTransform _parentRectTransformRewadPanel;
        public SimpleEvent OnOkButtonClickEvent { get; private set; } = new SimpleEvent();
        private List<RewardPanel> _listRewardPanels = new List<RewardPanel>(); //список для отображения количиства нужных мне елеентов реварда 

        public void StatusActivChestsRewardPanel(bool actuvStatus) // при нажати актевируем окно нужного мне выграша 
        {
            gameObject.SetActive(actuvStatus);
        }

        public void SetRewardPanel(List<PanelRewardData> rewardPanels)
        {

            for (int i = 0; i < _listRewardPanels.Count; i++) //удалить все обекти в из сцени 
            {
                DestroyImmediate(_listRewardPanels[i].gameObject);
            }

            _listRewardPanels.Clear();
          

            Debug.LogError("Creat Reward Object");
            for (int i = 0; i < rewardPanels.Count; i++)
            {
                GameObject game = Instantiate(_pewardPanel.gameObject,_parentRectTransformRewadPanel); // создаем новый тип виграша
                RewardPanel panel = game.GetComponent<RewardPanel>(); // получаем компонет для заполнения данными 
                panel.SetIconReward(rewardPanels[i].IconReward);// задали тконку реварда  
                panel.SetCountReward(rewardPanels[i].CountReward); // задали количесва получиного реварда 
                game.SetActive(true);

                _listRewardPanels.Add(panel);
            }
        }

        private void Start() => _okButton.onClick.AddListener(OnOkButtonPressed);

        private void OnOkButtonPressed()
        {
            if (OnOkButtonClickEvent != null)
            {
                OnOkButtonClickEvent.Notify();
            }
        }
    }

}