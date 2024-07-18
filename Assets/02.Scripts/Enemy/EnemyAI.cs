using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class EnemyAI : MonoBehaviour
{
    private Transform playerTr;
    private Transform enemyTr;
    private Animator animator;
    private EnemyMoveAgent moveAgent;

    private readonly string playStr = "Player";
    private WaitForSeconds ws;

    public float attackDist = 5.0f; //플레이어와 거리가 5 이하이면 총을 쏜다.
    public float traceDist = 10.0f; //플레이어와 거리가 10이하면 추격한다. 그 이상이면 패트롤
    public bool isDie = false; //에너미의 죽음 여부 판단

    private readonly int hashMove = Animator.StringToHash("IsMove");
    //애니메이션 컨트롤러에 정의 한 파라미터의 해시값을 정수로 추출한다. (해시값은 해당 파라미터의 주소값이라고 이해중..)
    private readonly int hashSpeed = Animator.StringToHash("MoveSpeed");
    public enum State //에너미의 상태를 열거형 상수로 선언
    {
        PTROL=0 ,TRACE ,ATTACK ,DIE
    }
    public State state = State.PTROL; //처음 기본 상태는 0번 PTROL

    private EnemyFire enemyFire;

    void Awake()
    { //에너미가 패트롤하는 기능보다 더빠르게 정보를 얻기 위해 Awake사용.
        var player = GameObject.FindGameObjectWithTag(playStr);
        if(player != null)
            playerTr = player.GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        ws = new WaitForSeconds(0.3f); //기다리는 시간 0.3초로 미리 초기화

        moveAgent = GetComponent<EnemyMoveAgent>(); //EnemyMoveAgent스크립트 연결
        enemyFire = GetComponent<EnemyFire>(); //EnemyFire스크립트 연결
    }
    private void OnEnable() //오브젝트가 활성화 될 때마다. 호출
    {
        StartCoroutine(CheckState()); //거리 측정으로 state상태 설정
        StartCoroutine(Action()); // state상태에 따라 애니메이션 출력
    }
    IEnumerator CheckState() //state만 관리하는 함수
    {
        while(!isDie)
        {
            if(state == State.DIE) yield break; //사망 상태면 StartCoroutine바로 종료
            float dist = (playerTr.position - enemyTr.position).magnitude;
            if (dist <= attackDist) //어택 사정거리 안이라면
                state = State.ATTACK; //state를 어택상수로 바꿈
            else if (dist <= traceDist) //트레이스 사정거리 안이라면
                state = State.TRACE; //state를 트레이스 상수로 바꿈
            else //모두 아니라면
                state = State.PTROL; //state를 패트롤 상수로바꿈

            yield return ws;//상태를 체크한 후 0.3초 딜레이를 가짐
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws; //0.3초 후 부터 스위치문 발동

            switch (state)
            {
                case State.PTROL:
                    moveAgent.patrolling = true; //패트롤 프로퍼티에 set에 true를 넣는다, 패트롤 함수 가져옴.
                    animator.SetBool(hashMove, true);
                    enemyFire.isFire = false;
                    break;

                case State.ATTACK:
                    moveAgent.Stop(); //플레이어가 가까이 있을 경우 멈춰서 총을 쏴야하기 때문에 멈춤 함수 불러옴.
                    animator.SetBool(hashMove, false);
                    if(enemyFire.isFire == false) //if문이 있어도 되고 없어도 결과는 같다.
                        enemyFire.isFire = true;
                    break;

                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position; //추격 프로퍼티를 호출하여 player위치를 입력한다.
                    animator.SetBool(hashMove, true);
                    enemyFire.isFire = false;
                    break;

                case State.DIE:
                    moveAgent.Stop(); //에너미가 죽었을 경우 제자리에서 멈춰야함.
                    enemyFire.isFire = false;
                    break;
            }
        }
    }
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
