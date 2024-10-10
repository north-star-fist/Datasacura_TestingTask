using System.Threading;
using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Config;
using R3;

namespace Datasacura.TestTask.ZooWorld
{
    public interface IGameManager
    {
        UniTask StartGameAsync(LevelSO level, CancellationToken cancellationToken);

        public GameStats GameStats { get; }

        Observable<IAnimal> OnAnimalDead { get; }
    }
}
