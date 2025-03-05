using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float range;  // ระยะที่กระสุนจะไปได้
    private Vector3 startPosition;

    public float damage = 10f;  // ความเสียหายของกระสุน

    private void Start()
    {
        startPosition = transform.position;  // บันทึกตำแหน่งเริ่มต้นของกระสุน
    }

    public void SetRange(float bulletRange)
    {
        range = bulletRange;
    }

    private void Update()
    {
        // คำนวณระยะทางที่กระสุนเคลื่อนที่ไป
        float distanceTravelled = Vector3.Distance(startPosition, transform.position);

        // ถ้าระยะทางที่กระสุนเคลื่อนที่เกินกว่าระยะที่กำหนด
        if (distanceTravelled >= range)
        {
            Destroy(gameObject);  // ทำลายกระสุน
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ถ้ากระสุนชนกับสิ่งใดๆ
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ทำลายกระสุนและอาจทำให้ศัตรูได้รับความเสียหาย
            Destroy(gameObject);  // ทำลายกระสุน
        }
        else
        {
            // ถ้ากระสุนชนกับวัตถุอื่น ๆ
            Destroy(gameObject);  // ทำลายกระสุน
        }
    }
}
