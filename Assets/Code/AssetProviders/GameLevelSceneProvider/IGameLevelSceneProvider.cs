using Cysharp.Threading.Tasks;

namespace Datasakura.TestTask.ZooWorld.Flow
{
    public interface IGameLevelSceneProvider
    {
        /// <summary>
        /// Loads level scene by it's index.
        /// </summary>
        /// <returns> <see langword="true"/>if such level was found and loaded and false otherwise</returns>
        public UniTask<bool> LoadGameEnvironment(int levelIndex);

        /// <summary>
        /// Unloads level scene previously loaded.
        /// </summary>
        public UniTask UnloadGameEnvironment();
    }
}
