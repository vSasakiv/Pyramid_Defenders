public interface IEquipableWeapon
{
    string WeaponName { get; } 
    int Damage { get; }

    public void Attack(); 
}