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

        if (equippedWeapon.isAutomatic)
        {
            // สำหรับปืนอัตโนมัติ กดค้างยิงรัว
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                Shoot();
            }
        }
        else
        {
            // สำหรับปืนกึ่งอัตโนมัติ กดยิงได้ทีละนัด
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                Shoot();
            }
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

        // หมุนปืน
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ดึงสเกลปัจจุบัน แล้วแค่สลับ Y
        Vector3 currentScale = transform.localScale;
        if (Mathf.Abs(angle) > 90)
        {
            transform.localScale = new Vector3(currentScale.x, -Mathf.Abs(currentScale.y), currentScale.z); // พลิก Y โดยรักษา X/Z
        }
        else
        {
            transform.localScale = new Vector3(currentScale.x, Mathf.Abs(currentScale.y), currentScale.z); // คง Y ปกติ
        }
    }


    public void Shoot()
    {
        if (currentAmmoInMag <= 0 && equippedWeapon.weaponType != WeaponType.Pistol)
        {
            Debug.Log("Out of ammo! Reload your weapon.");
            return; // หยุดการยิงถ้ากระสุนหมด (ยกเว้นปืนพก)
        }

        if (equippedWeapon.firePoint != null && equippedWeapon.bulletPrefab != null)
        {
            int bulletCount = equippedWeapon.weaponType == WeaponType.Shotgun ? 8 : 1; // ยิง 1 นัดสำหรับ AR และปืนอื่นๆ

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = Instantiate(equippedWeapon.bulletPrefab, equippedWeapon.firePoint.position, equippedWeapon.firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                Vector2 shootDirection = equippedWeapon.firePoint.right;

                // ใส่ spreadAngle แค่สำหรับลูกซองเท่านั้น
                if (equippedWeapon.weaponType == WeaponType.Shotgun)
                {
                    float spreadAngle = Random.Range(-equippedWeapon.spread, equippedWeapon.spread);
                    shootDirection = Quaternion.Euler(0, 0, spreadAngle) * shootDirection;
                }

                rb.linearVelocity = shootDirection * equippedWeapon.bulletSpeed;
            }

            if (equippedWeapon.weaponType != WeaponType.Pistol)
            {
                currentAmmoInMag--;
            }

            nextFireTime = Time.time + 1f / equippedWeapon.fireRate;
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