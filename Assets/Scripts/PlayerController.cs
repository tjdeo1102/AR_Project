using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Game Manager")]
    GameManager gameManager;
    [Header("Bullet Pool Setting")]
    [SerializeField] List<GameObject> bulletPrefabs;
    [SerializeField] GameObjectPool bulletPool;
    [SerializeField] int bulletPoolIndex;
    [SerializeField] int maxPoolCount;

    [Header("Shoot Setting")]
    [SerializeField] float shootPower;
    [SerializeField] float bulletReloadTime;

    private GameDataModel model;
    private Coroutine bulletReloadCoroutine;

    void Start()
    {
        gameManager = GameManager.Instance;
        model = gameManager.GameDataModel; 
        if (bulletPool != null)
        {
            bulletPool.Initialize(bulletPrefabs, maxPoolCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isPlaying && bulletReloadCoroutine == null)
        {
            bulletReloadCoroutine = StartCoroutine(BulletReloadRoutine());
        }

        if (gameManager.isPlaying == false &&  bulletReloadCoroutine != null)
        {
            StopCoroutine(bulletReloadCoroutine);
        }
    }

    public void Shoot()
    {
        if (gameManager.isPlaying == false) return;
        if (bulletPool != null && bulletPool.canSpawnObject && model != null && model.BulletCount > 0)
        {
            var obj = bulletPool.GetObject(bulletPoolIndex);
            obj.transform.position = transform.position;
            if (obj.TryGetComponent<Bullet>(out var bullet))
            {
                if (bullet.rb != null)
                {
                    bullet.rb.AddForce(shootPower * transform.forward);
                }
                bullet.returnPool = bulletPool;
                bullet.returnPoolIndex = bulletPoolIndex;
            }
            model.BulletCount--;
        }
    }

    IEnumerator BulletReloadRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(bulletReloadTime);

        while (model != null)
        {
            yield return delay;
            model.BulletCount++;
        }
    }

}
