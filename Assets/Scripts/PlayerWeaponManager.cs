using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<Weapons> ownedWeapons = new List<Weapons>();
    public int currentWeaponIndex = 0;

    public WeaponController weaponController;
    public Transform weaponHolder; // จุดที่เก็บปืนในตัวผู้เล่น
    private GameObject currentWeaponObject; // อ็อบเจ็กต์ของปืนที่ถืออยู่
    private float nextFireTime;  // เวลาถัดไปในการยิง

    private void Start()
    {
        if (ownedWeapons.Count > 0)
        {
            EquipWeapon(0);  // เริ่มต้นด้วยการถือปืนแรกใน inventory
        }
        else
        {
            Debug.LogError("No weapons in inventory.");
        }
    }

    private void Update()
    {
        HandleWeaponSwitch();  // การสลับปืน
        HandleShooting();      // การยิง
        HandleReloading();     // การรีโหลด
        HandleWeaponRotation(); // การหมุนปืน
    }

    public void EquipWeapon(int index)
    {
        if (ownedWeapons.Count == 0) return;

        Weapons selectedWeapon = ownedWeapons[index];

        // ลบปืนเก่าในฉากก่อน (ถ้ามี)
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        // สร้างปืนจากพรีแฟ็บ
        currentWeaponObject = Instantiate(selectedWeapon.weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);

        // ตั้งค่า WeaponController ให้กับปืนที่สร้างใหม่
        weaponController = currentWeaponObject.GetComponent<WeaponController>();
        weaponController.equippedWeapon = selectedWeapon;

        // ตั้งค่า FirePoint จาก WeaponController
        Transform firePoint = currentWeaponObject.transform.Find(selectedWeapon.weaponName + "FirePoint");
        weaponController.SetFirePoint(firePoint);

        Debug.Log("Equipped: " + selectedWeapon.weaponName);
    }

    private void HandleWeaponSwitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % ownedWeapons.Count;
        }
        else if (scroll < 0f)
        {
            currentWeaponIndex = (currentWeaponIndex - 1 + ownedWeapons.Count) % ownedWeapons.Count;
        }

        EquipWeapon(currentWeaponIndex);
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && weaponController != null)
        {
            Shoot();
            nextFireTime = Time.time + 1f / weaponController.equippedWeapon.fireRate;
        }
    }

    private void HandleReloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && weaponController != null)
        {
            Reload();
        }
    }

    private void HandleWeaponRotation()
    {
        if (weaponController != null)
        {
            weaponController.RotateWeapon(); // หมุนปืนตามทิศทางเมาส์
        }
    }

    private void Shoot()
    {
        if (weaponController.currentAmmoInMag > 0 || weaponController.equippedWeapon.weaponType == WeaponType.Pistol)
        {
            if (weaponController.equippedWeapon.bulletPrefab != null && weaponController.firePoint != null)
            {
                GameObject bullet = Instantiate(weaponController.equippedWeapon.bulletPrefab, weaponController.firePoint.position, weaponController.firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 shootDirection = weaponController.firePoint.right;

                // การกระจายของกระสุน
                float spreadAngle = Random.Range(-weaponController.equippedWeapon.spread, weaponController.equippedWeapon.spread);
                shootDirection = Quaternion.Euler(0, 0, spreadAngle) * shootDirection;

                rb.linearVelocity = shootDirection * 20f; // ความเร็วของกระสุน
            }

            // ลดจำนวนกระสุนในแม็กกาซีน
            if (weaponController.equippedWeapon.weaponType != WeaponType.Pistol)
            {
                weaponController.currentAmmoInMag--;
            }
        }
    }

    private void Reload()
    {
        int ammoNeeded = weaponController.equippedWeapon.magazineSize - weaponController.currentAmmoInMag;
        int ammoToReload = Mathf.Min(ammoNeeded, weaponController.totalAmmo);
        weaponController.currentAmmoInMag += ammoToReload;
        weaponController.totalAmmo -= ammoToReload;
    }

    public void AddWeapon(Weapons newWeapon)
    {
        if (!ownedWeapons.Contains(newWeapon))
        {
            ownedWeapons.Add(newWeapon);  // เพิ่มปืนเข้า inventory
            Debug.Log("Picked up: " + newWeapon.weaponName);

            // เมื่อมีการเพิ่มปืนใหม่ จะทำการเลือกปืนใหม่ทันที
            EquipWeapon(ownedWeapons.Count - 1);
        }
        else
        {
            Debug.Log("Weapon already in inventory.");
        }
    }
}
