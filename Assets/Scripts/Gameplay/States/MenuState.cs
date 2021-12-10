using System.Threading.Tasks;
using SmileProject.SpaceInvader.Gameplay;

namespace SmileProject.Generic.GameState
{
    public class MenuState : SpaceInvaderGameState
    {
        public MenuState(int Id, string name, GameController gameController) : base(Id, name, gameController)
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