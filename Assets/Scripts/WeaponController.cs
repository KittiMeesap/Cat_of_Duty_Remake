using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapons equippedWeapon;
    public int currentAmmoInMag;
    public int totalAmmo;

    public float rotationSpeed = 180f;
    private SpriteRenderer spriteRenderer;

    private float nextFireTime;

    private void Start()
    {
        if (equippedWeapon != null)
        {
            currentAmmoInMag = equippedWeapon.magazineSize;
            totalAmmo = equippedWeapon.weaponType == WeaponType.Pistol ? int.MaxValue : equippedWeapon.maxAmmo;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the weapon!");
        }
    }

    public void HandleInput()
    {
        RotateWeapon();

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void RotateWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Shoot()
    {
        if (currentAmmoInMag > 0 || equippedWeapon.weaponType == WeaponType.Pistol)
        {
            if (equippedWeapon.firePoint != null && equippedWeapon.bulletPrefab != null)
            {
                GameObject bullet = Instantiate(equippedWeapon.bulletPrefab, equippedWeapon.firePoint.position, equippedWeapon.firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 shootDirection = equippedWeapon.firePoint.right;
                float spreadAngle = Random.Range(-equippedWeapon.spread, equippedWeapon.spread);
                shootDirection = Quaternion.Euler(0, 0, spreadAngle) * shootDirection;
                rb.linearVelocity = shootDirection * 20f;

                if (equippedWeapon.weaponType != WeaponType.Pistol)
                {
                    currentAmmoInMag--;
                }

                nextFireTime = Time.time + 1f / equippedWeapon.fireRate;
            }
            else
            {
                Debug.LogError("firePoint or bulletPrefab is not assigned in the equipped weapon!");
            }
        }
    }

    public void Reload()
    {
        int ammoNeeded = equippedWeapon.magazineSize - currentAmmoInMag;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);
        currentAmmoInMag += ammoToReload;
        totalAmmo -= ammoToReload;
    }
}