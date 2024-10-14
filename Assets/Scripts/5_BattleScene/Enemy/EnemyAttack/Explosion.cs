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
        // ������Ʈ Ǯ���� ����� ũ�� ������ ó�� ������� �缳��
        min.transform.localScale = addVector3;
    }

    void FixedUpdate()
    {
        // ���� ���� �κ��� ū ���� �κа� ��ġ�ϰų� Ŀ����
        if (min.transform.localScale.magnitude >= max.transform.localScale.magnitude)
        {
            // Ÿ���� ������ �ִٸ�
            if (target != null)
            {
                // Ÿ���� IDamagable�� ApplyDamage�� ���ظ� ��
                target.ApplyDamage(power);
            }
            // ������Ʈ Ǯ�� ���� ������Ʈ �ֱ�
            Managers.Pool.Push(gameObject);
        }
        else
        {
            // ���� ũ�Ⱑ �۴ٸ� ���� �˸��� ũ�⸦ ���� ũ���Ѵ�.
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
