using System;

public class PeasantDescription : UnitDescription, IMoveable, IDamagable, IGatherer, IBuilder
{
    private float _speed;
    public float Speed { get; set; }
    public void Move()
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

    public float GatherSpeed { get; set; }
    public float CurrentLoad { get; set; }
    public float MAXLoad { get; set; }
    public string TypeOfResource { get; set; }

    public void Gather(ICollectable collectable)
    {
        throw new NotImplementedException();
    }

    public void Unload()
    {
        throw new NotImplementedException();
    }

    public void DropLoad()
    {
        throw new NotImplementedException();
    }

    public int BuildingSpeed { get; set; }
    public void Build()
    {
        throw new NotImplementedException();
    }
}