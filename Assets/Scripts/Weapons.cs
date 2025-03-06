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
    public float bulletSpeed = 20f; // ความเร็วของกระสุน
    public bool isAutomatic; // true = กดยิงค้างได้ (AR, SMG), false = ต้องกดทีละนัด (Pistol, Shotgun)
}
