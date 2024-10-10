using Cysharp.Threading.Tasks;

namespace Datasacura.TestTask.ZooWorld.Flow
{
    public interface IGameManagerSceneProvider
    {
        public UniTask<(bool loadedSuccessfully, IGameManager gameManager)> LoadGameManager();

        public UniTask UnloadGameManager();
    }
}
