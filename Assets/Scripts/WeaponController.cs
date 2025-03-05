using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapons equippedWeapon;  // อ้างอิงถึงอาวุธที่ถืออยู่
    public int currentAmmoInMag;  // จำนวนกระสุนในแม็กกาซีน
    public int totalAmmo;  // จำนวนกระสุนทั้งหมด

    public float rotationSpeed = 180f;  // ความเร็วในการหมุนปืน

    private bool isFlipped = false;  // ตัวแปรบ่งบอกสถานะการ flip ของปืน
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // ตรวจสอบว่าได้กำหนดอาวุธและการตั้งค่าอื่นๆ หรือไม่
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

    public void RotateWeapon()
    {
        // คำนวณมุมการหมุนจากตำแหน่งเมาส์
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // เช็คทิศทางที่ปืนต้องการหมุน
        bool shouldFlip = (angle > 90f || angle < -90f);  // ถ้าปืนหันไปทางซ้าย

        if (spriteRenderer != null)
        {
            if (shouldFlip && !isFlipped)
            {
                spriteRenderer.flipX = true;  // ทำการ flip Sprite ของปืน
                isFlipped = true;
            }
            else if (!shouldFlip && isFlipped)
            {
                spriteRenderer.flipX = false;  // กลับสภาพปืนให้เป็นทิศทางเดิม
                isFlipped = false;
            }
        }

        // หมุนปืนไปตามทิศทางเมาส์
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Shoot()
    {
        // ตรวจสอบว่า equippedWeapon มีค่าและมี firePoint หรือไม่
        if (equippedWeapon != null && equippedWeapon.firePoint != null)
        {
            GameObject bullet = Instantiate(equippedWeapon.bulletPrefab, equippedWeapon.firePoint.position, equippedWeapon.firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 shootDirection = equippedWeapon.firePoint.right;

            // การกระจายของกระสุน
            float spreadAngle = Random.Range(-equippedWeapon.spread, equippedWeapon.spread);
            shootDirection = Quaternion.Euler(0, 0, spreadAngle) * shootDirection;

            rb.linearVelocity = shootDirection * 20f; // ความเร็วของกระสุน
        }

        // ลดจำนวนกระสุนในแม็กกาซีน
        if (equippedWeapon.weaponType != WeaponType.Pistol)
        {
            currentAmmoInMag--; // ลดกระสุน
        }
    }
}
