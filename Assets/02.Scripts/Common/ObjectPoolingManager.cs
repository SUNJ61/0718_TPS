using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;
    [SerializeField] private GameObject Bullet;
    private int MaxPool = 10; //오브젝트 풀링으로 생성할 갯수
    public List<GameObject> bulletpoolList; //using System.Collections.Generic; 활성화 됨

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

        CreatBulletPool(); //오브젝트 풀링 생성 함수
        CreatE_BulletPool();
    }
    void CreatBulletPool()
    {
        GameObject PlayerBulletGroup = new GameObject("PlayerBulletGroup "); //게임 오브젝트 한개 생성
        for (int i = 0; i < MaxPool; i++)
        {
            var _bullet = Instantiate(Bullet, PlayerBulletGroup.transform); //총알 오브젝트를 10개를 PlayerBulletGroup 안에 생성
            _bullet.name = $"{(i+1).ToString()}발"; //오브젝트 이름을 1발 부터 10발 까지
            _bullet.SetActive(false); //생성 된것 비활성화
            bulletpoolList.Add(_bullet); // 생성된 총알 리스트에 넣었다.
        }
    }
    public GameObject GetBulletPool()
    {
        for(int i = 0; i < bulletpoolList.Count; i++)
        {
            if(bulletpoolList[i].activeSelf == false) //해당 번째의 총알이 비활성화 되어있다면
            {
                return bulletpoolList[i]; //비활성화 되어있다면 총알 반환
            }
        }
        return null; //활성화 되어있으면 null을 반환
    }

    void CreatE_BulletPool()
    {
        GameObject EnemyBulletGroup = new GameObject("EnemyBulletGroup "); //게임 오브젝트 한개 생성
        for (int i = 0; i < E_MaxPool; i++)
        {
            var _bullet = Instantiate(E_Bullet, EnemyBulletGroup.transform); //총알 오브젝트를 10개를 PlayerBulletGroup 안에 생성
            _bullet.name = $"{(i + 1).ToString()}발"; //오브젝트 이름을 1발 부터 10발 까지
            _bullet.SetActive(false); //생성 된것 비활성화
            E_bulletpoolList.Add(_bullet); // 생성된 총알 리스트에 넣었다.
        }
    }
    public GameObject GetE_BulletPool()
    {
        for (int i = 0; i < E_bulletpoolList.Count; i++)
        {
            if (E_bulletpoolList[i].activeSelf == false) //해당 번째의 총알이 비활성화 되어있다면
            {
                return E_bulletpoolList[i]; //비활성화 되어있다면 총알 반환
            }
        }
        return null; //활성화 되어있으면 null을 반환
    }
}
