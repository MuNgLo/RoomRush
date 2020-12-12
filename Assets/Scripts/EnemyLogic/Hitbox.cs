using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies {
    public class Hitbox : MonoBehaviour
    {
        public EnemyState _eState = null;
        public float _damageMultiplier = 1.0f;

        internal void RecieveHit(Vector3 physicsForce, int damage)
        {
            _eState.TakeHit(physicsForce, damage * _damageMultiplier);
        }

    }// EOF CLASS
}