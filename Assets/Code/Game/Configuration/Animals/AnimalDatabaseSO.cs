using UnityEngine;

namespace Datasacura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(fileName = "Animal Database", menuName = "Zoo World/Animals/Animal Database")]
    public class AnimalDatabaseSO : ScriptableObject
    {
        public AAnimalFactorySO[] Animals;
    }
}
