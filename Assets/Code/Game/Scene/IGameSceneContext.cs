using UnityEngine;

namespace Datasakura.TestTask.ZooWorld
{
    public interface IGameSceneContext
    {
        /// <summary>
        /// Parent transform for all animal game objects
        /// </summary>
        public Transform AnimalParent { get; }
    }
}
