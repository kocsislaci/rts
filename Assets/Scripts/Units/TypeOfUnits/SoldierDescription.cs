using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierDescription : UnitDescription, IMoveable, IAttacker, IDamagable
{
    private float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _speed = value;
            this.gameObject.GetComponent<NavMeshAgent>().speed = value;
        }
    }
    public void Move()
    {
        throw new NotImplementedException();
    }

    public float Damage { get; set; }
    public float AttackSpeed { get; set; }
    public void Attack(IDamagable target)
    {
        throw new NotImplementedException();
    }

    public float CurrentHealth { get; set; }
    public float MAXHealth { get; set; }
    public float Defense { get; set; }
    public void SufferAttack(IDamagable attacker)
    {
        throw new NotImplementedException();
    }
}
