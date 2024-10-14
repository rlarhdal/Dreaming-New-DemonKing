using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Soul : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator animator;
    public Transform target;
    private int speed = 10;
    public float spawnTime = 1.0f;
    public bool check = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        float posX = Random.Range(-1f, 1f);
        float posY = Random.Range(-1f, 1f);
        Vector3 sumPos = new Vector3(posX, posY, 0);

        // 랜덤 방향으로 퍼지게 구현을 위하여 AddForce를 넣었음
        rigid.AddForce(sumPos * 5, ForceMode2D.Impulse);
        // AddForce에서 애니메이터를 넣으면 AddForce가 작동되지 않아서 Invoke를 넣었음
        Invoke("OnAnimator", Random.Range(0.7f, 1.0f));
    }

    // 대기상태에 따른 Soul의 애니메이터 컴포넌트를 활성화
    private void OnAnimator()
    {
        animator.enabled = true;
    }



    void FixedUpdate()
    {
        if(spawnTime > 0)
        {
            spawnTime -= Time.fixedDeltaTime;
            
            return;
        }
        else
        {
            check = true;
        }

        if (target == null)
            target = GameObject.Find("Player").transform;

        // 소울과 플레이어와의 거리가 5이하가 되면 플레이방향으로 이동
        if (Vector3.Distance(target.position, transform.position) < 5)
        {
            rigid.velocity = (target.position - transform.position).normalized * speed;
        }
    }

    public void GetTarget(Transform transform)
    {
        target = transform;
    }

    // 플레이어와 충돌이 되면 소울을 획득
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && check)
        {
            spawnTime = 1.0f;
            check = false;
            Managers.Pool.Push(gameObject);

            if (SceneManager.GetActiveScene().name == System.Enum.GetName(typeof(SceneType), SceneType.Tutorial)) return; // 튜토리얼 예외처리

            MapGenerator.instance?.AddSoul();
        }
    }
}
