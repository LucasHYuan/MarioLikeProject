using UnityEngine;

public class PoleClimbingPlayerState : PlayerState
{
    protected override void OnEnter(Player player) { }
    protected override void OnExit(Player player) { }
    protected override void OnStep(Player player) { }
    public override void OnContact(Player player, Collider other) { }
}