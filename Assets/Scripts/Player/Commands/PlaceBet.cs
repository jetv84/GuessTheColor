using Photon.Pun;

namespace BettingApp.Player
{
    public class PlaceBet : PlayerCommand
    {
        private PlayerController _controller;

        public PlaceBet(PlayerController controller)
        {
            _controller = controller;
        }

        public override void Execute()
        {
            _controller.GetView().RPC("PlaceBet", RpcTarget.AllBuffered);
        }
    }
}
