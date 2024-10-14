using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartPointTracking : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager ImageManager;

    [SerializeField] GameObject[] pillarPrefabs;

    [SerializeField] GameObject wallPrefab;

    public bool isPlayInit;

    private bool[] isCheckBlock;
    private List<Transform> blocks;
    private void OnEnable()
    {
        ImageManager.trackedImagesChanged += OnImageChange;
        isCheckBlock = new bool[pillarPrefabs.Length];
        blocks = new List<Transform>(pillarPrefabs.Length);
    }

    private void OnDisable()
    {
        ImageManager.trackedImagesChanged -= OnImageChange;
    }

    void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        if (pillarPrefabs.Length < 5 || isPlayInit == false) return;

        foreach (ARTrackedImage image in args.added)
        {
            string imageName = image.referenceImage.name;

            switch (imageName)
            {
                case "Block1":
                    Instantiate(pillarPrefabs[0], image.transform.position, image.transform.rotation, image.transform);
                    isCheckBlock[0] = true;
                    break;
                case "Block2":
                    Instantiate(pillarPrefabs[1], image.transform.position, image.transform.rotation, image.transform);
                    isCheckBlock[1] = true;
                    break;
                case "Block3":
                    Instantiate(pillarPrefabs[2], image.transform.position, image.transform.rotation, image.transform);
                    isCheckBlock[2] = true;
                    break;
                case "Block4":
                    Instantiate(pillarPrefabs[3], image.transform.position, image.transform.rotation, image.transform);
                    isCheckBlock[3] = true;
                    break;
                case "Block5":
                    Instantiate(pillarPrefabs[4], image.transform.position, image.transform.rotation,image.transform);
                    isCheckBlock[4] = true;
                    break;
                default:
                    break;
            }
            if (blocks.Contains(image.transform.GetChild(0)) == false)
            {
                blocks.Add(image.transform.GetChild(0));
            }

            // ��� ��Ұ� true�� ��쿡 ��� ��ġ ��, ��輳ġ�� �ǰ� �������� ����
            if (Array.IndexOf(isCheckBlock,false) < 0)
            {
                var sortedBlocks = SortPillars(blocks);
                for (int i = 0; i < blocks.Count; i++)
                {
                    var curBlockPos = sortedBlocks[i].position;
                    var nextBlockPos = sortedBlocks[(i + 1) % sortedBlocks.Count].position;
                    // y���� ����
                    nextBlockPos.y = curBlockPos.y;
                    // �߽����� �� �� ����, ������ �� ���� ���⺤���� ������ ���ϵ���
                    var dir = nextBlockPos - curBlockPos;
                    var wallPos = (curBlockPos + nextBlockPos) / 2;
                    wallPos.y = curBlockPos.y;
                    var wall = Instantiate(wallPrefab, wallPos, Quaternion.LookRotation(new Vector3(-dir.z, 0, dir.x).normalized));
                    // ���� ���� ���� (x�� ���̰� ���� ����, y���� ������ ���ǵ� ����, z���� ������ ���ǵ� ���� �β�)
                    wall.transform.localScale = new Vector3(Vector3.Distance(curBlockPos, nextBlockPos), wall.transform.localScale.y, wall.transform.localScale.z);
                }
                // ���� ���۰� �Բ� isPlayInit ���� ����
                isPlayInit = false;
                var center = new Vector3(transform.position.x, sortedBlocks[0].position.y, transform.position.z);
                GameManager.Instance.Play(center);
            }
        }

        // ����� ������ ������ ������� ����
        List<Transform> SortPillars(List<Transform> pillars)
        {
            if (pillars.Count != 5) return pillars;

            // �߽��� ���
            Vector3 center = Vector3.zero;
            foreach (var pillar in pillars)
            {
                center += pillar.position;
            }
            center /= pillars.Count;

            // �߽������� ���� �� �������� ������ ����ؼ� ����
            return pillars.OrderBy(p => Mathf.Atan2(p.position.z - center.z, p.position.x - center.x)).ToList();
        }

        //foreach (ARTrackedImage image in args.updated)
        //{
        //    var obj = image.transform.GetChild(0).transform;
        //    obj.position = image.transform.position;
        //    obj.rotation = image.transform.rotation;
        //}
    }


}
