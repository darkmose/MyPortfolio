using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class PlayerExperienceAnimationHelperView : MonoBehaviour, IDisposable
    {
        private const float LEVEL_ANIMATION_DURATION = 0.5f;
        private const float LEVEL_PERCENTAGE_ANIMATION_DURATION = 0.4f;
        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _experienceToAdd;
        [SerializeField] private Image _experiencePercentageProgressBar;
        private PlayerExperienceAnimationHelper _model;
        private Tweener _experienceValueTweener; 
        private Tweener _experiencePercentageValueTweener;
        private Tweener _currentLevelTweener;
        
        public void LinkModel(PlayerExperienceAnimationHelper model)
        {
            _model = model;
        }

        public void SetActiveExperience(bool isActive)
        {
            _experienceToAdd.gameObject.SetActive(isActive);
        }

        public Tweener AnimateCurrentLevel(int startLevel, int endLevel, float duration, Action onComplete = null)
        {
            var format = "0 LVL";
            _currentLevelTweener?.Kill();
            _currentLevel.text = startLevel.ToString(format);

            _currentLevelTweener = DOTween.To(() => startLevel, newValue =>
            {
                startLevel = newValue;
                _currentLevel.text = startLevel.ToString(format);
            }, 
            endLevel, duration).OnComplete(()=> onComplete?.Invoke());
            return _currentLevelTweener;
        }

        public Tweener AnimateExperienceValueToAdd(int startValue, int endExperienceValue, float duration, Action onComplete = null)
        {
            _experienceValueTweener?.Kill();
            _experienceToAdd.text = startValue.ToString("+0 XP");

            _experienceValueTweener = DOTween.To(() => startValue, newValue =>
            {
                startValue = newValue;
                _experienceToAdd.text = startValue.ToString("+0 XP");
            },
            endExperienceValue, duration).OnComplete(() => onComplete?.Invoke());
            return _experienceValueTweener;
        }

        public Tweener AnimateCurrentExperiencePercentage(float startValue, float endExperiencePercentage, float duration, Action onComplete = null) 
        {
            _experiencePercentageValueTweener?.Kill();
            _experiencePercentageProgressBar.fillAmount = startValue;

            _experiencePercentageValueTweener = DOTween.To(()=>startValue, newValue =>
            {
                startValue = newValue;
                _experiencePercentageProgressBar.fillAmount = startValue;
            },
            endExperiencePercentage, duration).OnComplete(() => onComplete?.Invoke());
            return _experiencePercentageValueTweener;
        }

        public void AnimateLevelUp(int startLevel, int endLevel, float startExperiencePercentage, float endExperiencePercentage, Action onComplete = null)
        {
            var sequence = DOTween.Sequence();
            var levels = endLevel - startLevel;
            var startTweener = AnimateCurrentExperiencePercentage(startExperiencePercentage, 1f, (1f - startExperiencePercentage) * LEVEL_PERCENTAGE_ANIMATION_DURATION);
            sequence.Append(startTweener);

            for (int i = 1; i < levels; i++)
            {
                var tweener = AnimateCurrentExperiencePercentage(0f, 1f, LEVEL_PERCENTAGE_ANIMATION_DURATION);
                var changeLevelTweener = AnimateCurrentLevel(startLevel + (i-1), startLevel + i, LEVEL_ANIMATION_DURATION);
                sequence.Append(tweener);
                sequence.Insert(LEVEL_PERCENTAGE_ANIMATION_DURATION * i, changeLevelTweener);
            }

            var endExperienceTweener = AnimateCurrentExperiencePercentage(0f, endExperiencePercentage, LEVEL_PERCENTAGE_ANIMATION_DURATION);
            sequence.Insert(LEVEL_PERCENTAGE_ANIMATION_DURATION * levels, endExperienceTweener);

            sequence.OnComplete(()=> onComplete?.Invoke());
            sequence.Play();
        }

        public void SetInitialData(int currentLevel, int experienceToAdd, float currentExperiencePercentage)
        {
            DisposeTweeners();
            _currentLevel.text = currentLevel.ToString("0 LVL");
            _experienceToAdd.text = experienceToAdd.ToString("+0 XP");
            _experiencePercentageProgressBar.fillAmount = currentExperiencePercentage;
        }

        private void DisposeTweeners()
        {
            _currentLevelTweener?.Kill();
            _experiencePercentageValueTweener?.Kill();
            _experienceValueTweener?.Kill();
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            DisposeTweeners();
        }
    }
}