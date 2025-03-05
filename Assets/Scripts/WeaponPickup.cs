using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapons weapon; // ข้อมูลปืนที่เก็บ

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerWeaponManager player = other.GetComponent<PlayerWeaponManager>();
        if (player != null)
        {
            bool weaponExists = false;

            // ตรวจสอบว่า weapon นี้อยู่ใน inventory ของผู้เล่นหรือไม่
            foreach (Weapons ownedWeapon in player.ownedWeapons)
            {
                if (ownedWeapon == weapon)
                {
                    weaponExists = true;
                    AddAmmoToWeapon(ownedWeapon, player); // เพิ่มกระสุนให้ปืนที่มีอยู่แล้ว
                    break;
                }
            }

            if (!weaponExists)
            {
                player.AddWeapon(weapon); // เพิ่มปืนใหม่ใน inventory และเปลี่ยนปืนที่ถืออยู่
                player.EquipWeapon(player.ownedWeapons.Count - 1); // เลือกปืนใหม่ทันที
            }

            Destroy(gameObject); // ลบปืนจากฉากหลังจากที่เก็บ
        }
    }

    private void AddAmmoToWeapon(Weapons ownedWeapon, PlayerWeaponManager player)
    {
        if (ownedWeapon.weaponType != WeaponType.Pistol)
        {
            // ปืนที่ไม่ใช่พิสตอล
            WeaponController weaponController = player.weaponController;

            int ammoNeeded = ownedWeapon.magazineSize - weaponController.currentAmmoInMag;
            if (ammoNeeded > 0)
            {
                int ammoToReload = Mathf.Min(ammoNeeded, player.weaponController.totalAmmo);
                weaponController.currentAmmoInMag += ammoToReload;
                player.weaponController.totalAmmo -= ammoToReload;
            }
        }
    }
}