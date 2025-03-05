using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class CharacterController : Character
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    public PlayerWeaponManager playerWeaponManager; // ใช้ PlayerWeaponManager

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // รับค่าการเคลื่อนไหว
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // ตรวจสอบว่าตัวละครกำลังกระทำการเคลื่อนไหวหรือไม่
        bool isMoving = movement.sqrMagnitude > 0;

        // ตั้งค่าตัวแปรอนิเมชั่น
        animator.SetBool("IsIdle", !isMoving);
        animator.SetBool("IsWalkSide", movement.x != 0);
        animator.SetBool("IsWalkBack", movement.y > 0);
        animator.SetBool("IsWalkForward", movement.y < 0);

        // Flip sprite ถ้าเดินไปทางซ้าย
        if (movement.x < 0)
            spriteRenderer.flipX = true; // เดินซ้าย
        else if (movement.x > 0)
            spriteRenderer.flipX = false; // เดินขวา

        // Update movement ของตัวละคร
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    void FixedUpdate()
    {
        // ควบคุมการเคลื่อนที่ของตัวละคร
        rb.linearVelocity = movement.normalized * moveSpeed;
    }
}
