using UnityEngine;
public interface IDamageSource
{
    DamageSourceType DamageSourceType { get; }
    float Damage { get; }
    string Name { get; }

    Transform Transform { get; }
}