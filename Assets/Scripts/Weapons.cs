using UnityEngine;

public enum WeaponType { Pistol, AR, SMG, Shotgun }

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapons : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public WeaponType weaponType;
    public float damage;
    public float spread;
    public float fireRate;
    public int magazineSize;
    public int maxAmmo;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public WeaponController weaponController;
}
