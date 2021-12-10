using System.Threading.Tasks;
using SmileProject.SpaceInvader.Gameplay;

namespace SmileProject.Generic.GameState
{
    public class SpaceInvaderGameState : BaseGameState
    {
        protected GameController _gameController;

        public SpaceInvaderGameState(int Id, string name, GameController gameController) : base(Id, name)
        {
            _gameController = gameController;
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