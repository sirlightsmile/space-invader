using System.Threading.Tasks;

namespace SmileProject.Generic.GameState
{
    public abstract class BaseGameState
    {
        public readonly string Name;
        public readonly int ID;

        public BaseGameState(string name, int Id)
        {
            this.Name = name;
            this.ID = Id;
        }

        public abstract Task OnStateEnter();

        public abstract void OnStateUpdate();

        public abstract Task OnStateExit();
    }
}