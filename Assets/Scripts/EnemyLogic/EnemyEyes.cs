using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [System.Serializable]
    public class EnemyEyes
    {
        [SerializeField]
        public LayerMask _blocksSight;
        public float _maxDistance = 100.0f;
        public float _coneAngle = 90.0f;
        public Transform _eyes;

        private Bounds _bBox;

        /*public EnemyEyes(Transform eyeBone)
        {
            _eyes = eyeBone;
        }*/

        public bool CanWeSee(Bounds bBox)
        {
            _bBox = bBox;
            if(Vector3.Distance(_eyes.position, bBox.center) > _maxDistance) { return false; }
            Vector3 bbDirection = bBox.center - _eyes.position;
            if(Vector3.Angle(_eyes.forward, bbDirection) <= _coneAngle * 0.5f) {
                // Hit anything between eyes and center of bBox and we say we can't see player
                if(Physics.Raycast(_eyes.position,bbDirection,Vector3.Distance(_eyes.position, bBox.center), _blocksSight))
                {
                    Debug.DrawLine(_eyes.position, _eyes.position + bbDirection.normalized * _maxDistance, Color.cyan, Time.deltaTime * 10.0f);
                    return false;
                }
                return true;
            }
            return false;
        }

        

        internal void OnDrawGizmos()
        {
            if (_eyes)
            {
                GizmoExtension.DrawGizmosCone(_eyes.position, _coneAngle, _maxDistance, _eyes.forward, Color.red, true);
            }
        }

        /*private void DebugView(Vector3 Headposition, Vector3 bbBoxPosition, Transform head, Vector3 bbDirection )
        {

            Debug.DrawLine(Headposition, Headposition + head.forward * 4.0f, Color.red); // Works
            Debug.DrawLine(Headposition, Headposition + bbDirection.normalized * 4.0f, Color.magenta); // works

            Vector3 rightSide = (head.rotation * Quaternion.AngleAxis(_coneAngle, head.right)).eulerAngles;
            Vector3 leftSide = (head.rotation * Quaternion.AngleAxis(-_coneAngle, head.right)).eulerAngles;

            Debug.DrawLine(Headposition, Headposition + rightSide.normalized * 10.0f, Color.green);
            Debug.DrawLine(Headposition, Headposition + leftSide.normalized * 10.0f, Color.blue);


            Debug.DrawLine(Headposition, bbBoxPosition, Color.yellow);
        }*/


    }
}
