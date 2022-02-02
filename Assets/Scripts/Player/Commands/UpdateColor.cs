using Photon.Pun;

namespace BettingApp.Player
{
    public class UpdateColor : PlayerCommand
    {
        private PlayerController _controller;

        public UpdateColor(PlayerController controller)
        {
            _controller = controller;
        }

        public override void Execute(short betColorID)
        {
            _controller.GetView().RPC("UpdateColor", RpcTarget.AllBuffered, betColorID as object);
        }
    }
}
