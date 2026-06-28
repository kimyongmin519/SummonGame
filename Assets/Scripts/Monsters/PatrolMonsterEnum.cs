using Unity.Behavior;

namespace Monsters
{
    [BlackboardEnum]
    public enum PatrolMonsterEnum
    {
        IDLE, PATROL, REACT, CHASE, ATTACK, STUN, DEATH
    }
}