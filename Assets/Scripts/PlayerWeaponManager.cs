using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<Weapons> ownedWeapons = new List<Weapons>();
    public int currentWeaponIndex = 0;

    public WeaponController weaponController;
    public Transform weaponHolder; // จุดที่เก็บปืนในตัวผู้เล่น
    private GameObject currentWeaponObject; // อ็อบเจ็กต์ของปืนที่ถืออยู่

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
        HandleWeaponSwitch();
        if (weaponController != null)
        {
            weaponController.HandleInput();
        }
    }

    public void EquipWeapon(int index)
    {
        if (ownedWeapons.Count == 0) return;

        Weapons selectedWeapon = ownedWeapons[index];

        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        currentWeaponObject = Instantiate(selectedWeapon.weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);

        Weapons weaponInstance = currentWeaponObject.GetComponent<Weapons>();
        weaponController = currentWeaponObject.GetComponent<WeaponController>();
        weaponController.equippedWeapon = weaponInstance; // ใช้ instance ที่สร้างใหม่

        if (weaponInstance.firePoint == null)
        {
            Debug.LogError($"firePoint is not assigned in the prefab for {weaponInstance.weaponName}!");
        }

        Debug.Log("Equipped: " + weaponInstance.weaponName);
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

    public void AddWeapon(Weapons newWeapon)
    {
        if (!ownedWeapons.Exists(weapon => weapon.weaponName == newWeapon.weaponName))
        {
            ownedWeapons.Add(newWeapon);
            Debug.Log("Picked up: " + newWeapon.weaponName);

            EquipWeapon(ownedWeapons.Count - 1);
        }
        else
        {
            Debug.Log("Weapon already in inventory.");
        }
    }
}
