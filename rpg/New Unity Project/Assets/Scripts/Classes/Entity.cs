using UnityEngine;
public class Entity : ScriptableObject
{
    public string Name;
    public int Age;
    public string Faction;
    public string Occupation;
    public int Level;
    public int Health;
    public int Strength;
    public int Magic;
    public int Defense;
    public int Speed;
    public int Damage;
    public int Armor;
    public int NoOfAttacks;
    public string Weapon;
    public Vector2 Position;

    public void TakeDamage(int Amount) { Health -= (Amount - Armor); }
    public void Attack(Entity Entity) { Entity.TakeDamage(Strength); }
}
