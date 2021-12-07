using System.Threading.Tasks;

namespace SmileProject.Generic.GameState
{
    public class MenuState : BaseGameState
    {
        public MenuState(string name, int Id) : base(name, Id)
        {
        }

        public override Task OnStateEnter()
        {
            return null;
        }

        public override Task OnStateExit()
        {
            return null;
        }

        public override void OnStateUpdate()
        {
        }
    }
}