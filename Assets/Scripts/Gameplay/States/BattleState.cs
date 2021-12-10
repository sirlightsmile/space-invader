using System.Threading.Tasks;
using SmileProject.SpaceInvader.Gameplay;

namespace SmileProject.Generic.GameState
{
    public class BattleState : SpaceInvaderGameState
    {
        public BattleState(int Id, string name, GameController gameController) : base(Id, name, gameController)
        {
        }

        public override async Task OnStateEnter()
        {
            await _gameController.StartBattle();
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