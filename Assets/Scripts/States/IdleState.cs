using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class IdleState : VisualState
    {
        public IdleState(Player player) : base(player)
        {
        }

        protected override void Enter(Player player)
        {
            //Change sprite to left
        }
        protected override void Execute()
        {

        }
        protected override void Exit(Player player)
        {
            //Change back?
        }
    }
}