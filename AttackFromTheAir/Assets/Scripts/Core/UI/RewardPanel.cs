using Core.Resourses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class RewardPanel : MonoBehaviour
    {
        [SerializeField] private Image _iconReward;
        [SerializeField] private TextMeshProUGUI _countRewardText;

        public void SetIconReward(Sprite unity)
        {
            _iconReward.sprite = unity;
        }

        public void SetCountReward(int count)
        {
            _countRewardText.text = count.ToString();
        }

        public void StatusActivPanel(bool onOffStatus)
        {
            gameObject.SetActive(onOffStatus);
        }

    }

   
}
