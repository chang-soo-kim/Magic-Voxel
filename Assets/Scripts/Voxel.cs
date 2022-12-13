using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    // 1. ������ ���ư� �ӵ� �Ӽ�
    public float speed = 5;

    private void OnEnable()
    {
        currentTime = 0;
        Vector3 direction = Random.insideUnitSphere;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }
    void Start()
    {
        //2. ������ ������ ���´�.
        Vector3 direction = Random.insideUnitSphere;
        //3. ������ �������� ���ư��� �ӵ��� �ش�.
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    

        
    }
    //������ ������ �ð�
    public float destoryTime = 3.0f;
    // ����ð�
    float currentTime;

    void Update()
    {

        //�����ð��� ������ ������ �����ϰ�ʹ�.

        //1.�ð��� �귯�� �Ѵ�.
        currentTime += Time.deltaTime;
        //2. ���� �ð��� �����ϱ�.
        //���� ��� �ð��� ���� �ð��� �ʰ��ߴٸ�
        if(currentTime > destoryTime)
        {
           
            gameObject.SetActive(false);
            Voxel voxel = gameObject.GetComponent<Voxel>();
            VoxelMaker.voxelPool.Enqueue(voxel);
        }
    }
    }

