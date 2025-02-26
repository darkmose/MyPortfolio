using Configuration;
using Core.PlayerModule;
using Core.Tools;
using DG.Tweening;
using System;
using UnityEngine;

namespace Core.LobbyBase
{
    public class PlayerBaseResourceSendingSystem
    {
        private GameConfiguration _gameConfiguration;
        private IWallet _wallet;
        private int _sendingResourceAmountPerUpdate;
        private bool _canSendResource = true;
        private BaseObjectSelector _baseObjectSelector;
        private PlayerBasesManager _playerBasesManager;
        private MonoUpdater _monoUpdater;

        private bool _isSendingResources = false;

        private float _minResourceDelay = 0.1f;
        private float _maxResourceDelay;
        private float _currentDelay;
        private float _delayCounter;

        private float _speedUpDuration = 5f;
        private AnimationCurve _speedUpCurve;
        private float _speedUpCounter = 0f;

        private int _minResourceAmount = 1;
        private int _maxResourceAmount = 5;
        private int _resourceAmount = 1;

        public PlayerBaseResourceSendingSystem(GameConfiguration gameConfiguration, IWallet wallet, BaseObjectSelector baseObjectSelector, MonoUpdater monoUpdater, PlayerBasesManager playerBasesManager)
        {
            _gameConfiguration = gameConfiguration;
            _minResourceAmount = gameConfiguration.LobbyBaseConfiguration.MinResourceAmount;
            _maxResourceAmount = gameConfiguration.LobbyBaseConfiguration.MaxResourceAmount;
            _minResourceDelay = gameConfiguration.LobbyBaseConfiguration.MinResourceDelay;
            _maxResourceDelay = gameConfiguration.LobbyBaseConfiguration.MaxResourceDelay;
            _speedUpDuration = gameConfiguration.LobbyBaseConfiguration.SpeedUpDuration;
            _speedUpCurve = gameConfiguration.LobbyBaseConfiguration.SpeedUpCurve;
            _wallet = wallet;
            _baseObjectSelector = baseObjectSelector;
            _monoUpdater = monoUpdater;
            _monoUpdater.AddMonoUpdateListener(MonoUpdate);
            _playerBasesManager = playerBasesManager;
        }

        private void MonoUpdate()
        {
            if (!_isSendingResources)
            {
                return;
            }

            _delayCounter += Time.deltaTime;
            if (_delayCounter >= _currentDelay)
            {
                _speedUpCounter += _currentDelay;
                _delayCounter = 0f;
                SendResourceToBase();
                UpdateResourceData();
            }
        }

        private void UpdateResourceData()
        {
            var speedUpProgress = _speedUpCounter / _speedUpDuration;
            var animationCurveValue = _speedUpCurve.Evaluate(speedUpProgress);

            _resourceAmount = (int)Mathf.Lerp(_minResourceAmount, _maxResourceAmount, animationCurveValue);
            _currentDelay = Mathf.Lerp(_maxResourceDelay, _minResourceDelay, animationCurveValue);
        }

        public void StartSendingResources()
        {
            _currentDelay = _maxResourceDelay;
            _resourceAmount = _minResourceAmount;
            _speedUpCounter = 0f;
            _delayCounter = 0f;
            var baseObject = _baseObjectSelector.CurrentBaseObject.Value;
            var objectPos = baseObject.View.transform.position;
            var viewportPos = new Vector3(0.5f, 0.1f, 1);
            _playerBasesManager.SetHammersAttractorTo(objectPos);
            _playerBasesManager.SetHammersParticlesToViewportPos(viewportPos);

            UpdateResourceData();

            if (_wallet.GetMoneyCount(MoneyType.Hammers) > 0)
            {
                SendResourceToBase();
                _isSendingResources = true;
            }
        }

        private void SendResourceToBase()
        {
            var hammersSpended = _wallet.SpendAsMuchAsCan(MoneyType.Hammers, _resourceAmount);
            if (hammersSpended == 0)
            {
                StopSendingResources();
            }
            else
            {
                var baseObject = _baseObjectSelector.CurrentBaseObject.Value;
                _playerBasesManager.EmitHammers(1);
                var change = baseObject.AddResource(hammersSpended);
                if (change >= 0)
                {
                    _wallet.AddMoney(MoneyType.Hammers, change);
                    StopSendingResources();
                }
            }
        }

        public void StopSendingResources()
        {
            _isSendingResources = false;
        }
    }
}