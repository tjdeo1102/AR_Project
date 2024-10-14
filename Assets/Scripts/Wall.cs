using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private GameDataModel gameDataModel;

    private PlayerController playerController;
    void Start()
    {
        gameDataModel = GameManager.Instance.GameDataModel;
        playerController = GameManager.Instance.Player;
    }

    private void Update()
    {
        // ���� y�� ���� �÷��̾��� y��� ���ϼ����� �ǵ��� ����

        if (playerController != null) {
            var pos = transform.position;
            pos.y = playerController.transform.position.y;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            // ���� ��쿡��, �ڽ��� �ǰ� ���̸鼭 ������� �Ҵ´� (���� ���� �ν���)
            if (other.TryGetComponent<MonsterController>(out var controller))
            {
                if (gameDataModel != null)
                {
                    gameDataModel.HP--;
                }
                controller.RemoveMonster();
            }
        }
    }
}
