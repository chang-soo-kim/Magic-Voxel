using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMaker : MonoBehaviour
{
    //ũ�ν���� ����
    public Transform crossHair;
    public Voxel voxelFactory;

    public int voxelPoolSize = 20;
    public static Queue<Voxel> voxelPool = new Queue<Voxel>();
    void Start()
    {
        for (int i = 0; i < voxelPoolSize; i++)
        {
            Voxel voxel = Instantiate(voxelFactory);
            voxel.gameObject.SetActive(false);
            voxelPool.Enqueue(voxel);
        }
    }

    public float createTime = 0.1f;
    float currentTime = 0;
    void Update()
    {
        // ũ�ν���� �׸���
        ARAVRInput.DrawCrosshair(crossHair);

        if (ARAVRInput.Get(ARAVRInput.Button.One) == false) return;

        currentTime += Time.deltaTime;
        if (currentTime > createTime)
        {
            //2. ���콺�� ��ġ�� �ٴ� ���� ��ġ�� �ִٸ�
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (voxelPool.Count > 0)
                {
                    currentTime = 0;
                    Voxel voxel0 = voxelPool.Dequeue();
                    voxel0.gameObject.SetActive(true);
                    voxel0.transform.position = hitInfo.point;
                }
            }
        }
      
        
       
        

    }
}
