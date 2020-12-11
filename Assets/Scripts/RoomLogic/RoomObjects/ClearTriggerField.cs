using RoomLogic.Conditionscripts;
using UnityEngine;

namespace RoomLogic.RoomObjects
{
    [RequireComponent(typeof(BoxCollider))]
    class ClearTriggerField : RoomObjectBehaviour
    {
        public LayerMask _LayerToLookFor;
        public int _CountNeeded = 1;
        public int _canClearCount = 1;
        public float _triggerCooldown = 1.0f;

        private int _haveClearedCount = 0;
        private float _tsLastTriggered = 0.0f;
        private ConditionBehaviour _conditionScript;
        private bool _FireClearInLateUpdate = false;
        private void Awake()
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            _conditionScript = conditionScript;
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            if(Core.Instance.Rooms.CurrentRoomState != ROOMSTATE.ACTIVE) { return; }
            BoxCollider box = GetComponent<BoxCollider>();
            if(Time.time < _tsLastTriggered + _triggerCooldown) { return; }
            RaycastHit[] hits = Physics.BoxCastAll(
                transform.position + box.center + Vector3.up * box.size.y, 
                box.size * 0.5f, 
                Vector3.down, 
                box.transform.rotation, 
                box.size.y, 
                _LayerToLookFor);
            if (hits.Length > 0)
            {
                if (hits.Length >= _CountNeeded) {
                    _tsLastTriggered = Time.time;
                    _haveClearedCount++;
                    _FireClearInLateUpdate = true;
                }
            }
        }

        private void LateUpdate()
        {
            if (_FireClearInLateUpdate)
            {
                _FireClearInLateUpdate = false;
                Core.Instance.Rooms.CurrentRoom.ForceRoomClear();
            }
        }
    }// EOF CLASS
}
