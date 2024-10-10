using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Config;
using R3;

namespace Datasacura.TestTask.ZooWorld
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
