using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyCtrl : MonoBehaviour
{
    bool isDie = false;
    Animator animator;
    public float hp = 15f;
    public float maxHp = 15f;
    public float damage = 3f;
    public float attackDelay = 3f;
    private float attackTimer = 0f;
    Rigidbody2D rigid;
    public GameObject WayPoint0;
    public GameObject WayPoint1;
    public float MoveSpeed = 3.0f;
    public float RotationSpeed = 2.0f;
    public bool isWayPoint = false;
    public bool isSearch = false;
    public GameObject Target;
    public GameObject attackColider;
    private SpriteRenderer sprite;
    public Slider hpBar;
    public float way0x,way1x;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        sprite = this.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        way0x = WayPoint0.transform.position.x;
        way1x = WayPoint1.transform.position.x;
        attackTimer = attackDelay;
        attackColider.GetComponent<enemyAttack>().damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            if (isSearch == true)
            { //기존과 탐색 조건을 추가함
                Attack(); //공격기능		
                attackTimer += Time.deltaTime;
                if (attackTimer >= 100f) attackTimer = 10f;
                way0x = WayPoint0.transform.position.x;
                way1x = WayPoint1.transform.position.x;
            }
            else
            {
                animator.SetBool("run", true);
                WayPointMove(); //패트롤 기능
            }
            if (sprite.sprite.name == "Attack1_2")
            {
                attackColider.SetActive(true);

            }
            else
            {
                attackColider.SetActive(false);
            }
        }
        else if (sprite.sprite.name == "Death_8")
        {
            Destroy(this.gameObject);
        }
    }
    public void hit(float damage)
    {
        
        hp -= damage;
        if (hp <= 0) { 
            hp = 0;
            hpBar.gameObject.SetActive(false); 
            animator.SetTrigger("die");
            animator.SetBool("idle", false);
            isDie = true;
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                return; }
        if (maxHp != hp) hpBar.gameObject.SetActive(true);
        hpBar.value = hp / maxHp;
    }
    void WayPointMove()
    {
        if (isWayPoint == false)
        {
            //회전
            //transform.rotation = 
            //Quaternion.Slerp(transform.rotation,
            //Quaternion.LookRotation(WayPoint1.transform.position - transform.position),1);			
            //이동
            transform.localPosition = Vector2.MoveTowards(transform.position, new Vector2(way1x, this.transform.position.y), MoveSpeed * Time.deltaTime);
            //반전
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            if (Mathf.Abs(transform.position.x - way1x) <= 0.5f)
                isWayPoint = true;
        }
        else
        {
            //회전
            //transform.rotation = 
            //Quaternion.Slerp(transform.rotation,
            //Quaternion.LookRotation(WayPoint0.transform.position - transform.position),1);			
            //이동
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition = Vector2.MoveTowards(transform.position, new Vector2(way0x, this.transform.position.y), MoveSpeed * Time.deltaTime);
            //반전
            if (Mathf.Abs(transform.position.x - way0x) <= 0.5f)
                isWayPoint = false;
        }
        Search();//탐색기능 <-New Challenger!!
    }
    void Search()
    {
        float distance = Vector2.Distance(Target.transform.position, transform.position);
        //거리가 가까워지면 탐색에 걸림
        if (distance <= 6)
            isSearch = true;

    }
    void Attack()
    {
        if (transform.position.x < Target.transform.position.x)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else
            this.transform.rotation = Quaternion.Euler(0, 180, 0);

        //
        //돌아보는 방향을 플레이어 쪽으로

        float distance = Vector2.Distance(Target.transform.position, transform.position);
        if (distance <= 3)
        {
            if (attackTimer >= attackDelay)
            {
                Debug.Log("enemyAtk");
                attackTimer = 0;
                animator.SetTrigger("attack");

            }
            else if(distance>=1f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.position, new Vector2(Target.transform.position.x, this.transform.position.y), MoveSpeed * Time.deltaTime);

                animator.SetBool("run", true);

                animator.SetBool("idle", false);
            }
            else
            {
                animator.SetBool("run", false);

                animator.SetBool("idle", true);
            }
        }
        else if (distance > 10)
        {
            isSearch = false;
        }
        else
        {
            transform.localPosition = Vector2.MoveTowards(transform.position, new Vector2(Target.transform.position.x, this.transform.position.y), MoveSpeed * Time.deltaTime);
        }
    }
}
