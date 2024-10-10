using Cysharp.Threading.Tasks;

namespace Datasakura.TestTask.ZooWorld.Flow
{
    public interface IGameUiSceneProvider
    {
        public UniTask<(bool loadedSuccessfully, IGameUiManager gameManager)> LoadGameUi();

        public UniTask UnloadGameUi();
    }
}
