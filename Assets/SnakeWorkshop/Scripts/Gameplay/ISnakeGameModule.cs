using SnakeWorkshop.Scripts.Data;

namespace SnakeWorkshop.Scripts.Gameplay
{
    public interface ISnakeGameModule
    {
        void Initialize(SnakeGameData gameData);
        void OnUpdate();
    }
}