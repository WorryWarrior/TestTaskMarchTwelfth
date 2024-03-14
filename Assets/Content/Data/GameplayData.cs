using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Content.Data
{
    public class GameplayData
    {
        public ReactiveProperty<List<ProgressEntryData>> LeaderboardData { get; set; }
        public int LeaderboardSize { get; set; }
        public string[] LeaderboardNames { get; set; }

        public Vector2 GameplayAreaSize { get; set; }
        public float StartPlayerScale { get; set; }
        public float StartPlayerSpeed { get; set; }
        public float RadiusIncreaseRate { get; set; }
        public float SpeedDecreaseRate { get; set; }
        public float FoodScale { get; set; }
        public float StartEnemySpeed { get; set; }
        public int EnemyCount { get; set; }
        public float EnemyAwarenessRadius { get; set; }
        public float AiUpdateFrequency { get; set; }
        public int FoodCount { get; set; }
        public float FoodRespawnFrequency { get; set; }
        public float EnemyRespawnFrequency { get; set; }
    }
}