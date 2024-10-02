using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private Shooter shooter;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float circleRadius;
    private bool isGrounded;

    [SerializeField] private SpriteRenderer equipment;
    [SerializeField] private Animator anim;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float forceKnockBack;

    private ItemDisplay currentSlot;
    private Rigidbody2D rb;
    private float moveInput;
    private int direction = 1;
    private bool isJumping = false;
    private bool alive = true;
    private bool isRun;
    public bool canAttack = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetBool("isJump", false);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, circleRadius, groundLayer);

        if (!alive)
            return;

        Run();
        Attack();

        if (isGrounded)
            Jump();
    }

    public void SetSpriteEquipment(Sprite icon, ItemDisplay slot)
    {
        if (currentSlot != null)
            currentSlot.RemoveEquipitem();

        currentSlot = slot;
        equipment.sprite = icon;
        equipment.gameObject.SetActive(true);
        ShowEquippedItem();
    }

    private void ShowEquippedItem()
    {
        if (equipment != null)
        {
            GameObject newSlot = new GameObject("Equipment Slot");
            UnityEngine.UI.Image slotImage = newSlot.AddComponent<UnityEngine.UI.Image>();
            slotImage.sprite = equipment.sprite;
            slotImage.rectTransform.sizeDelta = new Vector2(100, 100);
            newSlot.transform.SetParent(transform, false);
        }
    }

    public void RemoveEquipItem()
    {
        equipment.sprite = null;
        equipment.gameObject.SetActive(false);
    }

    void Run()
    {
        Vector3 moveVelocity = Vector3.zero;
        anim.SetBool("isRun", false);
        isRun = false;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {

            direction = -1;
            moveVelocity = Vector3.left;

            transform.localScale = new Vector3(direction, 1, 1);
            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);

            isRun = true;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {

            direction = 1;
            moveVelocity = Vector3.right;

            transform.localScale = new Vector3(direction, 1, 1);
            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);

            isRun = true;
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0)
            && !anim.GetBool("isJump"))
        {
            isGrounded = false;
            anim.SetBool("isJump", true);
        }
        if (isGrounded)
        {
            return;
        }

        rb.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpForce);
        rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
        isGrounded = true;
    }

    void Attack()
    {
        if (isRun)
            return;

        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            anim.SetTrigger("attack");
            shooter.Shoot(direction);
            StartCoroutine(Cooldown());
        }
    }
    void Hurt()
    {
        anim.SetTrigger("hurt");
        if (direction == 1)
            rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
    }
    void Die()
    {
        anim.SetTrigger("die");
        alive = false;

    }
    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetTrigger("idle");
            alive = true;
        }
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void GetHit()
    {
        if (direction == 1)
            rb.AddForce(new Vector2(-forceKnockBack, 2f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(forceKnockBack, 2f), ForceMode2D.Impulse);
    }
}
