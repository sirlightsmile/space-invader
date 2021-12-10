using System.Threading.Tasks;
using SmileProject.SpaceInvader.Gameplay;

namespace SmileProject.Generic.GameState
{
    public class PauseState : SpaceInvaderGameState
    {
        public PauseState(int Id, string name, GameController gameController) : base(Id, name, gameController)
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