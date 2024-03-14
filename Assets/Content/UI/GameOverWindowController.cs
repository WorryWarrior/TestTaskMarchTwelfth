using System.Collections;
using Content.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Content.UI
{
    public class GameOverWindowController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private Button restartButton = null;
        [SerializeField] private Button menuButton = null;

        public void Initialize(GameStateMachine gameStateMachine)
        {
            restartButton.onClick.AddListener(gameStateMachine.Enter<LoadLevelState>);
            menuButton.onClick.AddListener(gameStateMachine.Enter<LoadMetaState>);

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        public void Show(float showDuration)
        {
            StartCoroutine(ShowRoutine(showDuration));
        }

        private IEnumerator ShowRoutine(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}