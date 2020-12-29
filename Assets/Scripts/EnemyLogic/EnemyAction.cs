using UnityEngine;
using UnityEngine.AI;
using RoomLogic;

namespace Enemies
{
    class EnemyAction : RoomObjectBehaviour
    {
        public Transform _meleePoint1;
        public Transform _meleePoint2;
        private float _tsLastMelee = 100.0f;

        private EnemyState _eState = null;
        private EnemyAI _ai = null;
        private NavMeshAgent _navAgent = null;

        public override void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript)
        {
            _eState = GetComponent<EnemyState>();
            _ai = GetComponent<EnemyAI>();
            _navAgent = GetComponent<NavMeshAgent>();
            roomDriver.OnRoomUpdate.AddListener(RoomUpdate);
            _tsLastMelee = Core.Instance.Runs.CurrentTotalTime + 10.0f;
        }

        public override void RoomUpdate(float roomDeltaTime)
        {
            switch (_eState.State)
            {
                case ENEMYSTATE.INACTIVE:
                    break;
                case ENEMYSTATE.IDLE:
                    break;
                case ENEMYSTATE.MOVING:
                    MoveToPoint(_ai.Target.transform.position);
                    break;
                case ENEMYSTATE.MELEE:
                    DoMeleeAttack();
                    break;
                case ENEMYSTATE.RUSHING:
                    MoveToPoint(_ai.Target.transform.position);
                    break;
                case ENEMYSTATE.STALKING:
                    _navAgent.isStopped = true;
                    //MoveToPoint(_ai.Target.transform.position);
                    break;
                case ENEMYSTATE.STUNNED:
                    break;
                case ENEMYSTATE.DEAD:
                    break;
                default:
                    break;
            }
        }


        internal void MoveToPoint(Vector3 point)
        {
            _navAgent.isStopped = false;
            _navAgent.destination = point;
        }

        internal void DoMeleeAttack()
        {
            if (Core.Instance.Runs.CurrentTotalTime > _tsLastMelee - Core.Instance.Settings.Enemies.RavMeleeCooldown)
            {
                return;
            }

            RaycastHit[] hits = Physics.CapsuleCastAll(_meleePoint1.position, _meleePoint2.position, Core.Instance.Settings.Enemies.RavMeleeRadius, transform.forward, 0.1f);
            //Debug.Log($"MeleeAttack! {hits.Length} colliders hit");
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.name == name) { return; }
                if (hit.collider.GetComponent<CharacterController>())
                {
                    Debug.Log($"MeleeAttack! Hit Player!");
                    _tsLastMelee = Core.Instance.Runs.CurrentTotalTime - Core.Instance.Settings.Enemies.RavMeleePenalty;
                    Core.Instance.Runs.ForcedPenaltyTime(Core.Instance.Settings.Enemies.RavMeleePenalty, true);
                }
            }
        }
    }
}
