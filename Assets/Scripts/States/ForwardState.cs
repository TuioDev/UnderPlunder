using System.Collections;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class ForwardState : VisualState
    {
        public ForwardState(Player player) : base(player)
        {
        }

        protected override void Enter(Player player)
        {
            //Set sprite to forward
        }
        protected override void Execute()
        {

        }
        protected override void Exit(Player player)
        {
            //Get back to iddle?
        }
    }
}