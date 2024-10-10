using Cysharp.Threading.Tasks;

namespace Datasakura.TestTask.ZooWorld.Flow
{
    public interface IGameManagerSceneProvider
    {
        public UniTask<(bool loadedSuccessfully, IGameManager gameManager)> LoadGameManager();

        public UniTask UnloadGameManager();
    }
}
