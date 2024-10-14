using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    GameObject min;
    [SerializeField]
    GameObject max;

    public int power;
    public int speed = 5;
    private Vector3 addVector3 = new Vector3(0.1f, 0.1f, 0.1f);

    private IDamagable target;

    private void OnEnable()
    {
        // 오브젝트 풀에서 재사용시 크기 문제로 처음 사이즈로 재설정
        min.transform.localScale = addVector3;
    }

    void FixedUpdate()
    {
        // 작은 폭발 부분이 큰 폭발 부분과 일치하거나 커지면
        if (min.transform.localScale.magnitude >= max.transform.localScale.magnitude)
        {
            // 타겟의 정보가 있다면
            if (target != null)
            {
                // 타겟의 IDamagable의 ApplyDamage로 피해를 줌
                target.ApplyDamage(power);
            }
            // 오브젝트 풀에 현재 오브젝트 넣기
            Managers.Pool.Push(gameObject);
        }
        else
        {
            // 아직 크기가 작다면 폭발 알림의 크기를 점점 크게한다.
            min.transform.localScale += addVector3 * speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable) && collision.gameObject.CompareTag("Player"))
        {
            target = damagable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable) && collision.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }
}
