using TMPro;
using UnityEngine;

namespace Content.UI
{
    public class LeaderboardWindowEntryController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI orderText = null;
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private TextMeshProUGUI scoreText = null;

        public void SetEntryData(int order, string playerName, int playerScore)
        {
            orderText.text = $"{order + 1}.";
            nameText.text = playerName;
            scoreText.text = $"{playerScore}";
        }
    }
}