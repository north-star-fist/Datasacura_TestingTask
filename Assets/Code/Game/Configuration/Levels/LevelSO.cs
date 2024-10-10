using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Datasacura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(fileName = "level", menuName = "Zoo World/Levels/Level")]
    public class LevelSO : ScriptableObject
    {
        [field: SerializeField, AssetReferenceUILabelRestriction(AddressablesTags.GameLevelScene)]
        public AssetReference LevelEnvironmentScene { get; private set; }

        [field: SerializeField, Tooltip("Minimum delay before next animal spawning in seconds")]
        public float AnimalSpawnMinDelay { get; private set; } = 1f;

        [field: SerializeField, Tooltip("Maximum delay before next animal spawning in seconds")]
        public float AnimalSpawnMaxDelay { get; private set; } = 2f;

        [field: SerializeField]
        public AnimalDatabaseSO Animals { get; private set; }
    }
}
