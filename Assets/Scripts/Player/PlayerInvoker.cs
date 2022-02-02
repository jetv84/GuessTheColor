
using BettingApp.Manager;
using UnityEngine;

namespace BettingApp.Player
{
    public class PlayerInvoker : MonoBehaviour
    {
        public void ExecuteCommand(PlayerCommand command)
        {
            command.Execute();
        }

        public void ExecuteCommand(PlayerCommand command, short enumValue)
        {
            command.Execute(enumValue);
        }
    }
}
