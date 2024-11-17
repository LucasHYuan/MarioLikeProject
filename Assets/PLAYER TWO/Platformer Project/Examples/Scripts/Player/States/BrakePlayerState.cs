using UnityEngine;

public class BrakePlayerState: PlayerState
{
    protected override void OnEnter(Player player)
    {
    }

    protected override void OnExit(Player player)
    {
    }

    protected override void OnStep(Player player)
    {
        player.Jump();
        player.Fall();
        player.Decelerate();
        
        var inputDirection = player.inputs.GetMovementCameraDirection();

        //if(player.stats.current.canBackflip && 
        //    Vector3.Dot(inputDirection, player.transform.forward) < 0)
        //{
        //    player.Backflip(player.stats.current.backflipBackwardTurnForce);
        //}
        
        if (player.lateralVelocity.sqrMagnitude == 0)
        {
            player.states.Change<IdlePlayerState>();
        }
        
    }


}