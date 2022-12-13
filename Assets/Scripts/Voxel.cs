using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    // 1. 복셀이 날아갈 속도 속성
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
        //2. 랜덤한 방향을 갖는다.
        Vector3 direction = Random.insideUnitSphere;
        //3. 랜덤한 방향으로 날아가는 속도를 준다.
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    

        
    }
    //복셀을 제거할 시간
    public float destoryTime = 3.0f;
    // 경과시간
    float currentTime;

    void Update()
    {

        //일정시간이 지나면 복셀을 제거하고싶다.

        //1.시간이 흘러야 한다.
        currentTime += Time.deltaTime;
        //2. 제거 시간이 됐으니까.
        //만약 경과 시간이 제거 시간을 초과했다면
        if(currentTime > destoryTime)
        {
           
            gameObject.SetActive(false);
            Voxel voxel = gameObject.GetComponent<Voxel>();
            VoxelMaker.voxelPool.Enqueue(voxel);
        }
    }
    }

