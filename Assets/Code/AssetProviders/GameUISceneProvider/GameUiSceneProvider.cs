﻿using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Datasakura.TestTask.ZooWorld.Config;
using Datasakura.TestTask.ZooWorld.Util;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer;

namespace Datasakura.TestTask.ZooWorld.Flow
{

    public class GameUiSceneProvider : IGameUiSceneProvider
    {
        private AsyncOperationHandle<SceneInstance> _loadedSceneOperation;
        private readonly List<LevelSO> _levels;
        private readonly AssetReference _mainGameModeScene;

        [Inject]
        public GameUiSceneProvider(AssetReference gameUiScene, LevelsSO levels)
        {
            _mainGameModeScene = gameUiScene;
            _levels = levels.Levels.Where(l => l != null).ToList();
        }

        public async UniTask<(bool loadedSuccessfully, IGameUiManager gameManager)> LoadGameUi()
        {
            await UnloadGameUi();

            _loadedSceneOperation = Addressables.LoadSceneAsync(_mainGameModeScene, LoadSceneMode.Additive);
            SceneInstance sceneInstance = await _loadedSceneOperation;
            if (sceneInstance.Scene.TryGetRootGameObject<IGameUiManager>(out var result))
            {
                await result.InitAsync(_levels);
                return (true, result);
            }
            else
            {
                await UnloadGameUi();
            }
            return (false, default);
        }

        public async UniTask UnloadGameUi()
        {
            if (_loadedSceneOperation.IsValid())
            {
                await Addressables.UnloadSceneAsync(_loadedSceneOperation);
            }
        }
    }
}
