using System;
using System.Collections.Generic;

[Serializable]
public class MainState
{
    public PrimaryState primaryState;
    public SecondaryState secondaryState;
}

[Serializable]
public class SecondaryState
{
    public SubState subState;
    public List<TertiaryState> tertiaryStates;
}

[Serializable]
public class TertiaryState
{
    public FinalAction finalActions;
    public float finalActionProbability;
    public PriorityLevel priorityLevel;
}

[Serializable]
public enum PrimaryState
{
    OnSummon,
    OffTurn,
    OnMovement,
    OnAttack,
    OnStunned,
    OnDeath
}

[Serializable]
public enum SubState
{
    EnemyInAttackRange,
    AllyInHealRange,
    AllyGettingHurt,
    SelfGettingHurt,
    SelfOnLowHealth,
    SkipToAction
}

[Serializable]
public enum FinalAction
{
    DoNothing,
    MoveAwayFromEnemy,
    MoveAwayFromAlly,
    MoveTowardsEnemy,
    MoveTowardsAlly,
    AttackEnemy,
    HealAlly,
}

[Serializable]
public enum PriorityLevel
{
    LowPriority,
    MediumPriority,
    HighPriority,
    MaxPriority,
}