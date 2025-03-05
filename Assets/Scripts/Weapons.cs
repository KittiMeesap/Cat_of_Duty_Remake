using UnityEngine;

public enum WeaponType { Pistol, Handcannon, AR, SMG, Shotgun }
public class Weapons : MonoBehaviour
{
    public string weaponName;
    public GameObject weaponPrefab;
    public WeaponType weaponType;
    public float spread;
    public float bulletRange = 50f;
    public float fireRate;
    public int magazineSize;
    public int maxAmmo;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public WeaponController weaponController;
}
