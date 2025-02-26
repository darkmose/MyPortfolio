using Core.Buildings;
using Core.Utilities;
using System;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectView : MonoBehaviour, IDisposable
    {
        [SerializeField] private BaseObjectDescriptor _objectDescriptor;
        [SerializeField] private FillShaderGraphProgressBar _fillShaderGraphProgressBar;
        [SerializeField] private BaseUpgradableBuildingView _upgradableBuildingView;
        [SerializeField] private ParticleSystem _levelUpParticles;
        [SerializeField] private Animator _objectAnimator;
        private int _bounceAnimationHash = Animator.StringToHash("Bounce");
        private BaseObject _model;
        public BaseObjectDescriptor ObjectDescriptor => _objectDescriptor;
        public BaseUpgradableBuildingView UpgradableBuildingView => _upgradableBuildingView;
        public BaseObject Model => _model;

        public void LinkModel(BaseObject model)
        {
            _model = model;
            _fillShaderGraphProgressBar.Init();
        }

        public Bounds GetObjectBounds()
        {
            return _fillShaderGraphProgressBar.GetCombinedBoundsOfObject();
        }

        public void SetObjectState(FillShaderGraphProgressBar.FillShaderProgressBarStates state)
        {
            _fillShaderGraphProgressBar.SetState(state);
        }

        public void SetProgress(float unlockProgress)
        {
            _fillShaderGraphProgressBar.SetProgressValue(unlockProgress);
        }

        public void AnimateGotResource()
        {
            //_objectAnimator.SetTrigger(_bounceAnimationHash);
        }

        public void AnimateObjectUnlocked()
        {
            _levelUpParticles.Play();
        }

        public void AnimateObjectLevelUp()
        {
            _levelUpParticles.Play();
        }

        public void Dispose()
        {
            _fillShaderGraphProgressBar.Dispose();
        }
    }
}