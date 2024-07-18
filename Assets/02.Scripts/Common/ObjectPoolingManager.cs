using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;
    [SerializeField] private GameObject Bullet;
    private int MaxPool = 10; //������Ʈ Ǯ������ ������ ����
    public List<GameObject> bulletpoolList; //using System.Collections.Generic; Ȱ��ȭ ��

    private GameObject E_Bullet;
    private int E_MaxPool = 20;
    public List<GameObject> E_bulletpoolList;

    private string bullet = "Bullet";
    private string E_bullet = "E_Bullet";
    void Awake()
    {
        if (poolingManager == null)
            poolingManager = this;
        else if(poolingManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Bullet = Resources.Load(bullet) as GameObject;
        E_Bullet = Resources.Load(E_bullet) as GameObject;

        CreatBulletPool(); //������Ʈ Ǯ�� ���� �Լ�
        CreatE_BulletPool();
    }
    void CreatBulletPool()
    {
        GameObject PlayerBulletGroup = new GameObject("PlayerBulletGroup "); //���� ������Ʈ �Ѱ� ����
        for (int i = 0; i < MaxPool; i++)
        {
            var _bullet = Instantiate(Bullet, PlayerBulletGroup.transform); //�Ѿ� ������Ʈ�� 10���� PlayerBulletGroup �ȿ� ����
            _bullet.name = $"{(i+1).ToString()}��"; //������Ʈ �̸��� 1�� ���� 10�� ����
            _bullet.SetActive(false); //���� �Ȱ� ��Ȱ��ȭ
            bulletpoolList.Add(_bullet); // ������ �Ѿ� ����Ʈ�� �־���.
        }
    }
    public GameObject GetBulletPool()
    {
        for(int i = 0; i < bulletpoolList.Count; i++)
        {
            if(bulletpoolList[i].activeSelf == false) //�ش� ��°�� �Ѿ��� ��Ȱ��ȭ �Ǿ��ִٸ�
            {
                return bulletpoolList[i]; //��Ȱ��ȭ �Ǿ��ִٸ� �Ѿ� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ������� null�� ��ȯ
    }

    void CreatE_BulletPool()
    {
        GameObject EnemyBulletGroup = new GameObject("EnemyBulletGroup "); //���� ������Ʈ �Ѱ� ����
        for (int i = 0; i < E_MaxPool; i++)
        {
            var _bullet = Instantiate(E_Bullet, EnemyBulletGroup.transform); //�Ѿ� ������Ʈ�� 10���� PlayerBulletGroup �ȿ� ����
            _bullet.name = $"{(i + 1).ToString()}��"; //������Ʈ �̸��� 1�� ���� 10�� ����
            _bullet.SetActive(false); //���� �Ȱ� ��Ȱ��ȭ
            E_bulletpoolList.Add(_bullet); // ������ �Ѿ� ����Ʈ�� �־���.
        }
    }
    public GameObject GetE_BulletPool()
    {
        for (int i = 0; i < E_bulletpoolList.Count; i++)
        {
            if (E_bulletpoolList[i].activeSelf == false) //�ش� ��°�� �Ѿ��� ��Ȱ��ȭ �Ǿ��ִٸ�
            {
                return E_bulletpoolList[i]; //��Ȱ��ȭ �Ǿ��ִٸ� �Ѿ� ��ȯ
            }
        }
        return null; //Ȱ��ȭ �Ǿ������� null�� ��ȯ
    }
}
