using Unity.Behavior;

namespace Monsters.BT
{
    [BlackboardEnum]
    public enum PatrolMonsterEnum
    {
        IDLE, PATROL, REACT, CHASE, ATTACK, STUN, DEATH
    }
}