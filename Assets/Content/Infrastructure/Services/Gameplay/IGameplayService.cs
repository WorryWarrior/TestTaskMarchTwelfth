using System.Threading.Tasks;
using Content.Gameplay;

namespace Content.Infrastructure.Services.Gameplay
{
    public interface IGameplayService
    {
        void Initialize();
        FoodController GetFoodObject(out int objectPoolId);
        void ReleaseFoodObject(int objectPoolId);
        Task CreateFoodPool();
        void ClearFoodPool();

        EnemyController GetEnemyObject(out int objectPoolId);
        void ReleaseEnemyObject(int objectPoolId);
        Task CreateEnemyPool();
        void ClearEnemyPool();

        void ClearLeaderboardEntries();
        void SubmitLeaderboardEntry(int id, int score);
        void UpdateLeaderboardState();
    }
}