using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnitDescription : MonoBehaviour
{
    private string _unitName;
    private Color _color;
    private int _team;
    public string UnitName { get; set; }
    public Color Color { get; set; }
    public int Team { get; set; }
}
public interface IMoveable
{
    public float Speed
    {
        get;
        set;
    }
    public void Move();
}
public interface IAttacker
{
    public float Damage
    {
        get;
        set;
    }
    public float AttackSpeed
    {
        get;
        set;
    }

    public void Attack(IDamagable target);
}
public interface IDamagable
{
    public float CurrentHealth
    {
        get;
        set;
    }
    public float MAXHealth
    {
        get;
        set;
    }
    public float Defense
    {
        get;
        set;
    }
    public void SufferAttack(IDamagable attacker);
}
public interface IGatherer
{
    public float GatherSpeed
    {
        get;
        set;
    }
    public float CurrentLoad
    {
        get;
        set;
    }
    public float MAXLoad
    {
        get;
        set;
    }
    public String TypeOfResource
    {
        get;
        set;
    }
    public void Gather(ICollectable collectable);
    public void Unload();
    public void DropLoad();
}
public interface ICollectable
{
    public string TypeOfResource { get; set; }
    public int LeftAmount { get; set; }
    public int MAXAmount { get; set; }
    public void BeingCollected(IGatherer gatherer);
}
public interface IBuilder
{
    public int BuildingSpeed { get; set; }
    public void Build();
}
