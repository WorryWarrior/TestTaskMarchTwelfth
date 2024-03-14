using TMPro;
using UnityEngine;

namespace Content.UI
{
    public class ProgressWindowEntryController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText = null;
        [SerializeField] private TextMeshProUGUI playerScoreText = null;

        public void SetData(string playerName, int playerScore)
        {
            playerNameText.text = playerName;
            playerScoreText.text = $"{playerScore}";
        }
    }
}