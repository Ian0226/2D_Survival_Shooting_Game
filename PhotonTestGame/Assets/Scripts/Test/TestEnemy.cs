using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private EnemyFollowPlayerSystem enemyFollowPlayerSystem;

    public Transform Target;
    void Start()
    {
        enemyFollowPlayerSystem = GetComponent<EnemyFollowPlayerSystem>();
    }

    void Update()
    {
        enemyFollowPlayerSystem.MoveToTarget(Target);
    }
}
