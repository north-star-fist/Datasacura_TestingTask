using Cysharp.Threading.Tasks;

namespace Datasacura.TestTask.ZooWorld.Flow
{
    public interface IGameUiSceneProvider
    {
        public UniTask<(bool loadedSuccessfully, IGameUiManager gameManager)> LoadGameUi();

        public UniTask UnloadGameUi();
    }
}
