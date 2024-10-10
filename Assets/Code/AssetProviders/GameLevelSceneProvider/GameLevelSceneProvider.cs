using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Config;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer;

namespace Datasacura.TestTask.ZooWorld.Flow
{

    public class GameLevelSceneProvider : IGameLevelSceneProvider
    {
        private AsyncOperationHandle<SceneInstance> _loadedSceneOperation;
        private List<LevelSO> _levels;

        [Inject]
        public void Init(LevelsSO levels)
        {
            _levels = levels.Levels.Where(l => l != null).ToList();
        }

        public async UniTask<bool> LoadGameEnvironment(int levelIndex)
        {
            await UnloadGameEnvironment();
            AssetReference sceneRef = GetSceneRef(levelIndex);
            if (sceneRef == null)
            {
                return false;
            }
            _loadedSceneOperation = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Additive);
            SceneInstance sceneInstance = await _loadedSceneOperation;
            return sceneInstance.Scene != null;
        }

        public async UniTask UnloadGameEnvironment()
        {
            if (_loadedSceneOperation.IsValid())
            {
                await Addressables.UnloadSceneAsync(_loadedSceneOperation);
            }
        }

        private AssetReference GetSceneRef(int levelInd)
        {
            return _levels.Count > levelInd ? _levels[levelInd].LevelEnvironmentScene : null;
        }
    }
}
