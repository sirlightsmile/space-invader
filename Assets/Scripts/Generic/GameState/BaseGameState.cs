using System.Threading.Tasks;

namespace SmileProject.Generic.GameState
{
    public abstract class BaseGameState
    {
        public readonly string Name;
        public readonly int ID;

        public BaseGameState(int Id, string name)
        {
            this.ID = Id;
            this.Name = name;
        }

        public abstract Task OnStateEnter();

        public abstract void OnStateUpdate();

        public abstract Task OnStateExit();
    }
}