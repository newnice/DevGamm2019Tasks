using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player :  IComparable<Player>
{
    private Vector2 _position;

    public int Hp { get; private set; }
    public Vector2 Position => _position;

    public void ChangeHp()
    {
        Hp += Random.Range(-100, 100);
    }

    public void ChangePosition()
    {
        _position.x+= Random.Range(-100f, 100f);
        _position.y+= Random.Range(-100f, 100f);
    }

    public int CompareTo(Player other)
    {
        if (other == null) return 1;
        return Hp.CompareTo(other.Hp);
    }
}