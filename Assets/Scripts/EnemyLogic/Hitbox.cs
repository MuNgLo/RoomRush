using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies {
    public class Hitbox : MonoBehaviour
    {
        public EnemyState _eState = null;

        internal void RecieveHit(Vector3 physicsForce)
        {
            _eState.TakeHit(physicsForce);
        }

    }// EOF CLASS
}