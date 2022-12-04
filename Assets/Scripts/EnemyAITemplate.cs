using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyAITemplate : ScriptableObject
{
    public List<MainState> mainStates = new()
    {
        new MainState(),
        new MainState(),
        new MainState(),
        new MainState(),
        new MainState(),
        new MainState(),
    };

    public void Awake()
    {
        for (var i = 0; i < mainStates.Count; i++)
        {
            mainStates[i].primaryState = i switch
            {
                0 => PrimaryState.OnSummon,
                1 => PrimaryState.OffTurn,
                2 => PrimaryState.OnAttack,
                3 => PrimaryState.OnDeath,
                4 => PrimaryState.OnMovement,
                5 => PrimaryState.OnStunned,
                _ => mainStates[i].primaryState
            };
        }
    }
}
