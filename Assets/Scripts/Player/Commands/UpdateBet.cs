using Photon.Pun;

namespace BettingApp.Player
{
    public class UpdateBet : PlayerCommand
    {
        private PlayerController _controller;

        public UpdateBet(PlayerController controller)
        {
            _controller = controller;
        }

        public override void Execute(short betAmmount)
        {
            _controller.GetView().RPC("UpdateBet", RpcTarget.AllBuffered, betAmmount as object);
        }
    }
}
