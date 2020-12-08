using UnityEngine;
namespace RoomLogic
{
    abstract public class RoomObjectBehaviour : MonoBehaviour
    {
        abstract public void RoomUpdate(float roomDeltaTime);
        abstract public void RoomObjectInit(RoomDriver roomDriver, ConditionBehaviour conditionScript);
    }
}
