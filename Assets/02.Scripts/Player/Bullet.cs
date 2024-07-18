using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private TrailRenderer trail;

    private float Speed = 1500.0f;
    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 2.0f); //오브젝트 풀링 안썻을 때 사용
    }
    private void BulletDisable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable() //오브젝트가 켜졌을 때 발동 되는 함수
    {
        rb.AddForce(tr.forward * Speed);
        Invoke("BulletDisable", 2.0f); //2초동안 오브젝트가 변화가 없을 경우 강제로 오브젝트를 끈다.
    }
    private void OnDisable()
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep(); //리깃바디 작동 중지
    }
}
