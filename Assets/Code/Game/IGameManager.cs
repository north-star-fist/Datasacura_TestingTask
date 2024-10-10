using System.Threading;
using Cysharp.Threading.Tasks;
using Datasakura.TestTask.ZooWorld.Config;
using R3;

namespace Datasakura.TestTask.ZooWorld
{
    public interface IGameManager
    {
        UniTask StartGameAsync(LevelSO level, CancellationToken cancellationToken);

        public GameStats GameStats { get; }

        Observable<IAnimal> OnAnimalDead { get; }
    }
}
