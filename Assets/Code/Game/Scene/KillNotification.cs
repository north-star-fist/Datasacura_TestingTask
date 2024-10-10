using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Datasacura.TestTask.ZooWorld.Visuals
{
    public class KillNotification : MonoBehaviour
    {
        [field: SerializeField, Tooltip("Rotation speed in degrees per second")]
        public TMP_Text NotificationText { get; private set; }

        private void Awake()
        {
            if (NotificationText != null)
            {
                NotificationText.enabled = false;
            }
        }

        /// <summary>
        /// Shows the notifications for specified time.
        /// </summary>
        public async UniTask Show(float time)
        {
            if (NotificationText == null)
            {
                return;
            }

            NotificationText.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(time));
            NotificationText.enabled = false;
        }
    }
}
