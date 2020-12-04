using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DecaMovement.Base;

namespace DecaMovement.Events
{
    /// <summary>
    /// All movement related events goes here including their raisers
    /// </summary>
    public static class DecaMovementEvents
    {
        // EVENTS
        static public PlayerJumpEvent OnJump = new PlayerJumpEvent();
        static public PlayerLandEvent OnLand = new PlayerLandEvent();
        static public PlayerMovementEvent OnPlayerMovementEvent = new PlayerMovementEvent();
        static public PlayerMovementResetEvent OnPlayerMovementResetEvent =  new PlayerMovementResetEvent();
        static public void RaiseOnJumpEvent(GroundTest groundData)
        {
            OnJump?.Invoke(groundData);
        }
        static internal void RaiseOnLandEvent(GroundTest groundData)
        {
            OnLand?.Invoke(groundData);
        }
        static internal void RaiseOnPlayerMovementEvent(PlayerMovementEventArguments args)
        {
            OnPlayerMovementEvent?.Invoke(args);
        }
        static internal void RaiseOnPlayerMovementResetEvent(PlayerMovementResetArguments args)
        {
            OnPlayerMovementResetEvent?.Invoke(args);
        }
    }// EOC
}
