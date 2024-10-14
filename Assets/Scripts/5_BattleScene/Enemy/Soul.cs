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

        // ���� �������� ������ ������ ���Ͽ� AddForce�� �־���
        rigid.AddForce(sumPos * 5, ForceMode2D.Impulse);
        // AddForce���� �ִϸ����͸� ������ AddForce�� �۵����� �ʾƼ� Invoke�� �־���
        Invoke("OnAnimator", Random.Range(0.7f, 1.0f));
    }

    // �����¿� ���� Soul�� �ִϸ����� ������Ʈ�� Ȱ��ȭ
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

        // �ҿ�� �÷��̾���� �Ÿ��� 5���ϰ� �Ǹ� �÷��̹������� �̵�
        if (Vector3.Distance(target.position, transform.position) < 5)
        {
            rigid.velocity = (target.position - transform.position).normalized * speed;
        }
    }

    public void GetTarget(Transform transform)
    {
        target = transform;
    }

    // �÷��̾�� �浹�� �Ǹ� �ҿ��� ȹ��
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && check)
        {
            spawnTime = 1.0f;
            check = false;
            Managers.Pool.Push(gameObject);

            if (SceneManager.GetActiveScene().name == System.Enum.GetName(typeof(SceneType), SceneType.Tutorial)) return; // Ʃ�丮�� ����ó��

            MapGenerator.instance?.AddSoul();
        }
    }
}
