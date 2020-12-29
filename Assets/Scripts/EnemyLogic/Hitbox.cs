using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies {
    public class Hitbox : MonoBehaviour
    {
        private EnemyState _eState = null;
        public HITBOXLOCATION _hitboxLocation = HITBOXLOCATION.BODY;

        public EnemyState EState { get => _eState; set => _eState = value; }

        internal void RecieveHit(Vector3 physicsForce, int damage)
        {
            EState.TakeHit(physicsForce, damage, _hitboxLocation);
        }

    }// EOF CLASS
}