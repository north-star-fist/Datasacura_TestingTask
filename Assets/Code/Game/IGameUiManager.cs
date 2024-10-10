using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datasakura.TestTask.ZooWorld.Config;
using R3;

namespace Datasakura.TestTask.ZooWorld
{
    public interface IGameUiManager
    {
        public Observable<Unit> OnStart { get; }

        public Observable<Unit> OnExit { get; }

        public Observable<int> OnLevelChosen { get; }

        UniTask InitAsync(IReadOnlyList<LevelSO> levels);

        public void SetDeadPreyCount(int count);

        public void SetDeadPredatorCount(int count);
    }
}
