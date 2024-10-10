using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Datasakura.TestTask.ZooWorld.Config;
using Datasakura.TestTask.ZooWorld.StateMachine;
using R3;
using UnityEngine;

namespace Datasakura.TestTask.ZooWorld.Flow
{
    public class AppStateGame : IState
    {
        private readonly IList<LevelSO> _levels;
        private readonly IGameManagerSceneProvider _gameManagerSceneProvider;
        private readonly IGameLevelSceneProvider _levelsProvider;
        private readonly IGameUiSceneProvider _uiSceneProvider;

        private IGameManager _gameManager;
        private IGameUiManager _uiManager;
        private int _currentLevel;
        private int _loadedLevel = -1;

        CancellationTokenSource _gameCancellationTokenSource;
        private DisposableBag _disposables = new DisposableBag();

        public AppStateGame(
            IList<LevelSO> levels,
            IGameManagerSceneProvider gameSceneProvider,
            IGameLevelSceneProvider levelsProvider,
            IGameUiSceneProvider uiSceneProvider)
        {
            _levels = levels;
            _gameManagerSceneProvider = gameSceneProvider;
            _levelsProvider = levelsProvider;
            _uiSceneProvider = uiSceneProvider;
        }

        public async UniTask OnEnterAsync()
        {
            _disposables.Dispose();
            _disposables = new();

            (var gmLoadResult, var uiLoadResult) = await UniTask.WhenAll(
                _gameManagerSceneProvider.LoadGameManager(),
                _uiSceneProvider.LoadGameUi()
            );

            if (!gmLoadResult.loadedSuccessfully)
            {
                throw new InvalidOperationException("Game manager scene was not loaded!");
            }
            _gameManager = gmLoadResult.gameManager;

            if (!uiLoadResult.loadedSuccessfully)
            {
                throw new InvalidOperationException("Levels Provider was not loaded");
            }
            _uiManager = uiLoadResult.gameManager;
            _uiManager.OnStart.Subscribe(_ => StartGameAsync().Forget()).AddTo(ref _disposables);
            _uiManager.OnExit.Subscribe(_ => Application.Quit()).AddTo(ref _disposables);
            _uiManager.OnLevelChosen.Subscribe(lvl => _currentLevel = lvl).AddTo(ref _disposables);
            _gameManager.OnAnimalDead.Subscribe(HandleAnimalDeath).AddTo(ref _disposables);
            _uiManager.SetDeadPredatorCount(0);
            _uiManager.SetDeadPreyCount(0);
        }

        public async UniTask OnExitAsync()
        {
            await UniTask.WhenAll(
                _gameManagerSceneProvider.UnloadGameManager(),
                _levelsProvider.UnloadGameEnvironment(),
                _uiSceneProvider.UnloadGameUi()
            );

            _disposables.Dispose();
        }

        public UniTask StartAsync()
        {
            // Do nothing because the game is launched via UI button
            return UniTask.CompletedTask;
        }

        private async UniTask StartGameAsync()
        {
            if (_loadedLevel != _currentLevel && !await _levelsProvider.LoadGameEnvironment(_currentLevel))
            {
                throw new InvalidOperationException("No such level");
            }

            if (_gameCancellationTokenSource != null)
            {
                _gameCancellationTokenSource.Cancel();
                _gameCancellationTokenSource.Dispose();
            }
            _gameCancellationTokenSource = new CancellationTokenSource();

            _loadedLevel = _currentLevel;
            _uiManager.SetDeadPredatorCount(0);
            _uiManager.SetDeadPreyCount(0);
            await _gameManager.StartGameAsync(_levels[_currentLevel], _gameCancellationTokenSource.Token);
        }

        private void HandleAnimalDeath(IAnimal animal)
        {
            if (animal.IsPredator)
            {
                _uiManager.SetDeadPredatorCount(_gameManager.GameStats.DeadPredators);
            }
            else
            {
                _uiManager.SetDeadPreyCount(_gameManager.GameStats.DeadPrey);
            }
        }
    }
}
