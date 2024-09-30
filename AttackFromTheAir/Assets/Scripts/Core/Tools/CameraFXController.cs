using DG.Tweening;
using UnityEngine;

namespace Core.Tools
{
    public class CameraFXController : MonoBehaviour 
    {
        [Header("Mozaic Glitch")]
        [SerializeField] private CameraFilterPack_Glitch_Mozaic _mozaic;
        [SerializeField] private float _mozaicGlitchDuration;
        [SerializeField] private float _mozaicGlitchEndValue;
        [Header("Night Vision")]
        [SerializeField] private CameraFilterPack_Color_GrayScale _grayScale;
        [SerializeField] private CameraFilterPack_Color_Adjust_Levels _adjustLevels;
        [SerializeField] private CameraFilterPack_Film_Grain _filmGrain;
        [SerializeField] private CameraFilterPack_Noise_TV _noiseTV;
        [SerializeField] private float _nightVisionTransitionDuration;
        [SerializeField] private float _nightVisionTransitionEndValue;

        private Sequence _mozaicGlitchSequence;
        private Sequence _nightVisionSequence;
        public bool IsNightVisionEnabled { get; private set; }

        private void Awake()
        {
            EnableNightVision(true, false);
        }

        public void AnimateMozaicGlitch()
        {
            _mozaicGlitchSequence?.Kill();
            _mozaic.enabled = true;
            var value = 0.001f;
            var duration = _mozaicGlitchDuration / 2f;
            var mozaicIn = DOTween.To(() => value, newValue => value = newValue, _mozaicGlitchEndValue, duration).SetEase(Ease.Linear);
            var mozaicOut = DOTween.To(() => value, newValue => value = newValue, 0.001f, duration).SetEase(Ease.Linear);
            _mozaicGlitchSequence = DOTween.Sequence().Append(mozaicIn).Append(mozaicOut)
            .OnUpdate(() =>
            {
                _mozaic.Intensity = value;
            })
            .OnComplete(() =>
            {
                _mozaic.enabled = false;
            });
        }

        public void EnableNightVision(bool enabled, bool withFX = true)
        {
            _nightVisionSequence?.Kill();
            _grayScale.enabled = enabled;
            _adjustLevels.enabled = enabled;
            _filmGrain.enabled = enabled;
            IsNightVisionEnabled = enabled;
            if (enabled && withFX)
            {
                AnimateNightVisionOn();
            }
        }

        private void AnimateNightVisionOn()
        {
            var value = 0f;
            _nightVisionSequence?.Kill();
            var duration = _nightVisionTransitionDuration / 2f;
            var nightVisionIn = DOTween.To(() => value, newValue => value = newValue, _nightVisionTransitionEndValue, duration).SetEase(Ease.Linear);
            var nightVisionOut = DOTween.To(() => value, newValue => value = newValue, 0f, duration).SetEase(Ease.Linear);
            _noiseTV.enabled = true;
            _nightVisionSequence = DOTween.Sequence().Append(nightVisionIn).Append(nightVisionOut)
            .OnUpdate(() =>
            {
                _noiseTV.Fade = value;
            })
            .OnComplete(() =>
            {
                _noiseTV.enabled = false;
            });
        }
    }
}