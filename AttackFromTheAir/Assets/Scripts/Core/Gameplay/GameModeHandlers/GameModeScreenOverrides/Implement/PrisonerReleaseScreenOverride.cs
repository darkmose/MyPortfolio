using Core.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class PrisonerReleaseScreenOverride : AttackModeScreenOverride
    {
        [SerializeField] private TextMeshProUGUI _status;
        [SerializeField] private Image _statusBar;
        public override AttackGameMode AttackGameMode => AttackGameMode.PrisonerRelease;

        public void SetStatus(int currentReleased, int prisonersReleaseTarget)
        {
            _status.text = $"{currentReleased}/{prisonersReleaseTarget}";
            float statusProgress = (float)currentReleased / (float)prisonersReleaseTarget;
            _statusBar.fillAmount = statusProgress;
        }
    }
}