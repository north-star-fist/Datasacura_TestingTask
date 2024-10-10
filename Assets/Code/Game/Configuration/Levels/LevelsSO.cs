using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Datasacura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(fileName = "levels", menuName = "Zoo World/Levels/Level Set")]
    public class LevelsSO : ScriptableObject
    {
        public List<LevelSO> Levels => _levels != null ? _levels.Where(l => l != null).ToList() : new List<LevelSO>();

        [SerializeField]
        private List<LevelSO> _levels;
    }
}
