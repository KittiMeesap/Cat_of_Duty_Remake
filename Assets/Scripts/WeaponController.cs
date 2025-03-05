using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapons equippedWeapon;
    public Transform firePoint;  // จุดที่ยิงของปืน
    public int currentAmmoInMag;
    public int totalAmmo;

    public float rotationSpeed = 180f; // ความเร็วในการหมุนปืน

    private bool isFlipped = false;  // ตัวแปรบ่งบอกสถานะการ flip ของปืน
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // ตรวจสอบว่าได้กำหนดอาวุธและการตั้งค่าอื่นๆหรือไม่
        if (equippedWeapon != null)
        {
            currentAmmoInMag = equippedWeapon.magazineSize;
            totalAmmo = equippedWeapon.weaponType == WeaponType.Pistol ? int.MaxValue : equippedWeapon.maxAmmo;
        }

        // หาตัว SpriteRenderer ของปืน
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the weapon!");
        }
    }

    // ฟังก์ชันนี้เพื่อให้สามารถตั้งค่า firePoint จากภายนอก
    public void SetFirePoint(Transform newFirePoint)
    {
        firePoint = newFirePoint;
        if (equippedWeapon != null)
        {
            equippedWeapon.firePoint = firePoint;  // มอบค่า firePoint ให้กับ Weapons
        }
    }

    public void RotateWeapon()
    {
        // คำนวณมุมการหมุนจากตำแหน่งเมาส์
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // เช็คทิศทางที่ปืนต้องการหมุน
        bool shouldFlip = (angle > 90f || angle < -90f);  // ถ้าปืนหันไปทางซ้าย (มากกว่า 90 องศาหรือน้อยกว่า -90 องศา)

        if (spriteRenderer != null) // ตรวจสอบว่ามีการสร้าง spriteRenderer แล้วหรือยัง
        {
            if (shouldFlip && !isFlipped)
            {
                // Flip Sprite เมื่อถึงจุดที่ต้องการ
                spriteRenderer.flipX = true;  // ทำการ flip Sprite ของปืน
                isFlipped = true;  // ตั้งค่าให้ flip เสร็จ
            }
            else if (!shouldFlip && isFlipped)
            {
                // กลับสภาพปืนให้เป็นทิศทางเดิมเมื่อมันหันกลับไปขวา
                spriteRenderer.flipX = false;  // กลับด้าน Sprite
                isFlipped = false;  // ตั้งค่าให้กลับสู่สภาพปกติ
            }
        }

        // หมุนปืนไปตามทิศทางเมาส์
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
