using CucuTools.PlayerSystem;
using CucuTools.Terminal;

namespace Examples.Playground.Scripts
{
    public class PlaygroundCommands : TerminalCommandRegistrator
    {
        public FirstPersonRigidController player;

        [TerminalCommand("player.set.speed")]
        private void SetMoveSpeedMax(float speedMax)
        {
            player.settings.moveSpeedMax = speedMax;
        }
        
        [TerminalCommand("player.set.jump")]
        private void SetJump(float jumpHeight)
        {
            player.settings.jumpHeight = jumpHeight;
        }
        
        [TerminalCommand("player.set.gravity")]
        private void SetGravity(float gravity)
        {
            player.settings.gravityMax = gravity;
        }
    }
}