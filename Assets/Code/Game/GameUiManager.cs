using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Config;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Datasacura.TestTask.ZooWorld
{
    public class GameUiManager : MonoBehaviour, IGameUiManager
    {
        [SerializeField] TMP_Text _deadPreyCountText;
        [SerializeField] TMP_Text _deadPredatorCountText;

        [SerializeField] Button _startButton;
        [SerializeField] Button _exitButton;
        [SerializeField] TMP_Dropdown _levelDropdown;

        public Observable<Unit> OnStart => _startButton.OnClickAsObservable();

        public Observable<Unit> OnExit => _exitButton.OnClickAsObservable();

        public Observable<int> OnLevelChosen => OnValueChangedAsObservable(_levelDropdown);

        public void SetDeadPreyCount(int count) => SetNumberText(_deadPreyCountText, count);

        public void SetDeadPredatorCount(int count) => SetNumberText(_deadPredatorCountText, count);

        public UniTask InitAsync(IReadOnlyList<LevelSO> levels)
        {
            int num = 1;
            List<TMP_Dropdown.OptionData> options = levels.Select(l => new TMP_Dropdown.OptionData($"Level {num++}")).ToList();
            _levelDropdown.options = options;
            return UniTask.CompletedTask;
        }

        private static void SetNumberText(TMP_Text textComponent, int count)
        {
            if (textComponent != null)
            {
                textComponent.text = count.ToString();
            }
        }

        #region R3-TMP compatible extensions
        /// <summary>Observe onValueChanged with current `value` on subscribe.</summary>
        private static Observable<int> OnValueChangedAsObservable(TMP_Dropdown dropdown)
        {
            return Observable.Create<int, TMP_Dropdown>(dropdown, static (observer, d) =>
            {
                observer.OnNext(d.value);
                return d.onValueChanged.AsObservable(GetDestroyCancellationToken(d)).Subscribe(observer);
            });
        }

        private static CancellationToken GetDestroyCancellationToken(TMP_Dropdown value)
        {
            // UNITY_2022_2_OR_NEWER has MonoBehavior.destroyCancellationToken
#if UNITY_2022_2_OR_NEWER
            return value.destroyCancellationToken;
#else
            var component = value.gameObject.GetComponent<R3.Triggers.ObservableDestroyTrigger>();
            if (component == null)
            {
                component = value.gameObject.AddComponent<R3.Triggers.ObservableDestroyTrigger>();
            }
            return component.GetCancellationToken();
#endif
        }
        #endregion
    }
}
