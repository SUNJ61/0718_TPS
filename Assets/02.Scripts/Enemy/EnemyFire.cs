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

    private float nextFire = 0.0f; //�߻� ������ �ð� ���� ����.

    private readonly int hashFire = Animator.StringToHash("FireTrigger");
    private readonly string enemyGunSound= "Sound/enemyGunSound";
    private readonly string playerTag = "Player";
    private readonly float fireRate = 0.2f; // �Ѿ� �߻� ����
    private readonly float damping = 10.0f; // �÷��̾ ���� ȸ���� �ӵ�

    public bool isFire = false; //�߻� ���¸� �����ϴ� ����.
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
        firePos = transform.GetChild(3).GetChild(0).GetChild(0).transform; //���ʹ� �𵨸��� 3,0,0 �ε����� �ִ� ������Ʈ
        //find�� ã���� ���ʹ̰� �������� �Ǿ��� �� ���� ������Ʈ �̸��� ������ ���� ������ �߻�.
        fireClip = Resources.Load(enemyGunSound) as AudioClip;
    }
    void Update()
    {
        if(isFire) //�߻����̶��
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f); // ����ð����� 0.2 ~ 0.4������ �ð��� ��. 
            }
            Vector3 playernormal = playerTr.position - enemyTr.position; //�÷��̾� - ���ʹ� => ���ʹ̰� �÷��̾� ���� ����
            Quaternion rot = Quaternion.LookRotation(playernormal.normalized);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, damping*Time.deltaTime);
            //���ʹ̰� ���� ������ rot �������� damping�ӵ��� ȸ���Ѵ�.
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
