using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic.RoomObjects
{
    public class HazardMoving : RoomObjectBehaviour
    {
        public bool _resetPlayerWhenHit = false;
        public float _penaltyWhenPlayerHit = 0.0f;
        public float _moveSpeed = 5.0f;
        public float _moveDelay = 5.0f;
        [Range(0.0f, 1.0f)]
        public float _startCyclePos = 0.0f;
        public bool _startCycle = true;

        private Vector3 _startPos = Vector3.zero;
        private Vector3 _endPos = Vector3.zero;
        private float _travelDistance = 1.0f;
        private bool _onStartCycle = true;
        private float _cyclePos = 0.0f;
        private float _delayTimer = 0.0f;
        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            roomDriver.OnRoomReset.AddListener(ResetToStartPosition);
            _startPos = transform.Find("Start").position;
            _endPos = transform.Find("End").position;
            _travelDistance = Vector3.Distance(_startPos, _endPos);

            ResetToStartPosition();
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            if (_delayTimer < 0.0f)
            {
                float frameMovement = _moveSpeed * roomDeltaTime;
                if (_onStartCycle)
                {
                    _cyclePos += frameMovement / _travelDistance;
                }
                else
                {
                    _cyclePos -= frameMovement / _travelDistance;
                }
                _cyclePos = Mathf.Clamp01(_cyclePos);
                transform.position = Vector3.Lerp(_startPos, _endPos, _cyclePos);
                if(_cyclePos <= 0.0f || _cyclePos >= 1.0f)
                {
                    _delayTimer = _moveDelay;
                    _onStartCycle = !_onStartCycle;
                }
            }
            else
            {
                _delayTimer -= roomDeltaTime;
            }
        }

        private void ResetToStartPosition()
        {
            Debug.Log("Moving Hazard Reset called!!");
            _onStartCycle = _startCycle;
            _cyclePos = _startCyclePos;
            _delayTimer = 0.0f;
            transform.position = Vector3.Lerp(_startPos, _endPos, _cyclePos);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<MotorAlt>())
            {
                Core.Instance.Runs.ForcedPenaltyTime(_penaltyWhenPlayerHit);
                if (_resetPlayerWhenHit)
                {
                    Core.Instance.Player.ResetToSpawnPoint();
                }
            }
        }
    }// EOF CLASS
}