using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private AudioClip fireClip;
    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;
    private Transform firePos;

    private float nextFire = 0.0f; //발사 딜레이 시간 계산용 변수.

    private readonly int hashFire = Animator.StringToHash("FireTrigger");
    private readonly string enemyGunSound= "Sound/enemyGunSound";
    private readonly string playerTag = "Player";
    private readonly float fireRate = 0.2f; // 총알 발사 간격
    private readonly float damping = 10.0f; // 플레이어를 향해 회전할 속도

    public bool isFire = false; //발사 상태를 감지하는 변수.
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
        firePos = transform.GetChild(3).GetChild(0).GetChild(0).transform; //에너미 모델링에 3,0,0 인덱스에 있는 오브젝트
        //find로 찾으면 에너미가 여러명이 되었을 때 같은 오브젝트 이름이 여러개 생겨 오류가 발생.
        fireClip = Resources.Load(enemyGunSound) as AudioClip;
    }
    void Update()
    {
        if(isFire) //발사중이라면
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f); // 현재시간에서 0.2 ~ 0.4초이후 시간이 됨. 
            }
            Vector3 playernormal = playerTr.position - enemyTr.position; //플레이어 - 에너미 => 에너미가 플레이어 보는 방향
            Quaternion rot = Quaternion.LookRotation(playernormal.normalized);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, damping*Time.deltaTime);
            //에너미가 보는 방향이 rot 방향으로 damping속도로 회전한다.
        }
    }

    private void Fire()
    {
        var e_bullet = ObjectPoolingManager.poolingManager.GetE_BulletPool();
        if(e_bullet != null)
        {
            e_bullet.transform.position = firePos.transform.position;
            e_bullet.transform.rotation = firePos.transform.rotation;
            e_bullet.SetActive(true);
        }

        animator.SetTrigger(hashFire);
        SoundManager.S_instance.PlaySound(firePos.position,fireClip);
    }
}
