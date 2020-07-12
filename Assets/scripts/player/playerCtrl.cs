using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerCtrl : MonoBehaviour
{
    Animator animator;
    public float hp;
    public float maxHp = 30f;
    public float damage = 5f;
    public float movePower = 3f;
    public float jumpPower = 3f;
    public float attackDelay = 1f;
    private float attackTimer = 0f;
    public float dashDelay = 1f;
    private float dashTimer = 0f;
    private float invisibleTimer = 0f;
    private bool isInvisible = false;
    public Slider hpSlider;
    Rigidbody2D rigid;
    public bool isGround = false;
    Vector3 movement;
    bool isJumping = false;
    public GameObject attackColider;
    private SpriteRenderer sprite;

    //---------------------------------------------------[Override Function]
    //Initialization
    void Start()
    {
        attackColider.GetComponent<playerAttack>().damage = damage;
        sprite = this.GetComponent<SpriteRenderer>();
        hp = maxHp;
        animator = gameObject.GetComponentInChildren<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    //Graphic & Input Updates	
    void Update()
    {
        if (sprite.sprite.name == "Attack_4")
        {
            attackColider.SetActive(true);

        }
        else
        {
            attackColider.SetActive(false);
        }
        if (isInvisible)
        {
            invisibleTimer += Time.deltaTime;
            if (invisibleTimer >= 0.5f)
            {
                isInvisible = false;
                invisibleTimer = 0.0f;
                this.gameObject.layer = 0;
            }
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= 100000) attackTimer = 0f;
        dashTimer += Time.deltaTime;
        if (dashTimer >= 100000) dashTimer = 0f;
        if (Input.GetButtonDown("Jump")&&isGround&& attackTimer > 0.5f)
        {
            isJumping = true;
        }
        if (Input.GetMouseButtonDown(0)&&isGround&&attackTimer>attackDelay)
        {
            attackTimer = 0f;
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Space)&&isGround&& attackTimer >0.5f&&dashTimer>=dashDelay)
        {
            dashTimer = 0f;
            Dash();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform")
        {
            isGround=true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform")
        {
            isGround = false;
        }
    }
    //Physics engine Updates
    void FixedUpdate()
    {
        
        Move();
        Jump();

        
        
    }

    //---------------------------------------------------[Movement Function]
    void Attack()
    {
        Debug.Log("Attack");
        animator.SetTrigger("Attack");


    }
    public void hit(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            Debug.Log(hp);
            if (hp < 0) hp = 0;
            if (hp == 0) hpSlider.gameObject.SetActive(false);
            float CurHP = (float)hp / (float)maxHp;
            hpSlider.value= CurHP;
        }
        
    }
    void Dash()
    {
        this.gameObject.layer = 9;
        isInvisible = true;

        if (this.transform.rotation == Quaternion.Euler(0, 0, 0))
            this.rigid.AddForce(new Vector2(3f, 0f),ForceMode2D.Impulse);
        else
            this.rigid.AddForce(new Vector2(-3f, 0f), ForceMode2D.Impulse);

        Debug.Log("Dash");
        animator.SetTrigger("Dash");
    }
    void Move()
    {
        animator.SetBool("Run", true);
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0&&attackTimer>=0.7f)
        {
            moveVelocity = Vector3.left;
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else if (Input.GetAxisRaw("Horizontal") > 0 && attackTimer >= 0.7f)
        {
            moveVelocity = Vector3.right;
            this.transform.rotation=Quaternion.Euler(0, 0, 0);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping)
            return;
        animator.SetTrigger("Jump");
        //Prevent Velocity amplification.
        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }
}