using UnityEngine;

namespace Datasacura.TestTask.ZooWorld
{
    public interface IGameSceneContext
    {
        /// <summary>
        /// Parent transform for all animal game objects
        /// </summary>
        public Transform AnimalParent { get; }
    }
}
