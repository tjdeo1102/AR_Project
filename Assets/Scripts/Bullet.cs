using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float removeTime;
    [HideInInspector] public GameObjectPool returnPool;
    [HideInInspector] public int returnPoolIndex;
    [HideInInspector] public Rigidbody rb;


    private Coroutine bulletCoroutine;
    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);
    }
    private void OnEnable()
    {
        bulletCoroutine = StartCoroutine(BulletRoutine());
    }

    void RemoveBullet()
    {
        if (bulletCoroutine != null) 
        {
            StopCoroutine(bulletCoroutine);
        }
        if (returnPool != null)
        {
            if (rb != null) rb.velocity = Vector3.zero;
            returnPool.ReturnObject(returnPoolIndex, gameObject);
        }
    }

    IEnumerator BulletRoutine()
    {
        yield return new WaitForSeconds(removeTime);
        RemoveBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            if (collision.transform.TryGetComponent<MonsterController>(out var controller))
            {
                controller.RemoveMonster();
                RemoveBullet();
            }
        }
    }
}
