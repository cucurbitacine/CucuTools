using CucuTools.PlayerSystem;
using CucuTools.Terminal;

namespace Examples.Playground.Scripts
{
    public class PlaygroundCommands : TerminalCommandRegistrator
    {
        public PlaygroundController playground;

        private PlayerManager player => playground.player;
        private PlayerController person => player.person;
        
        [TerminalCommand("player.spawn")]
        private void SpawnPlayer()
        {
            playground.SpawnPlayer();
        }
        
        [TerminalCommand("player.set.speed")]
        private void SetMoveSpeedMax(float speedMax)
        {
            person.settings.moveSpeedMax = speedMax;
        }
        
        [TerminalCommand("player.set.jump")]
        private void SetJump(float jumpHeight)
        {
            person.settings.jumpHeight = jumpHeight;
        }
        
        [TerminalCommand("player.set.gravity")]
        private void SetGravity(float gravity)
        {
            person.settings.gravityMax = person.normal * gravity;
        }
    }
}