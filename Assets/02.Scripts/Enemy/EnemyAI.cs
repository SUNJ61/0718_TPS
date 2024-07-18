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

    public float attackDist = 5.0f; //�÷��̾�� �Ÿ��� 5 �����̸� ���� ���.
    public float traceDist = 10.0f; //�÷��̾�� �Ÿ��� 10���ϸ� �߰��Ѵ�. �� �̻��̸� ��Ʈ��
    public bool isDie = false; //���ʹ��� ���� ���� �Ǵ�

    private readonly int hashMove = Animator.StringToHash("IsMove");
    //�ִϸ��̼� ��Ʈ�ѷ��� ���� �� �Ķ������ �ؽð��� ������ �����Ѵ�. (�ؽð��� �ش� �Ķ������ �ּҰ��̶�� ������..)
    private readonly int hashSpeed = Animator.StringToHash("MoveSpeed");
    public enum State //���ʹ��� ���¸� ������ ����� ����
    {
        PTROL=0 ,TRACE ,ATTACK ,DIE
    }
    public State state = State.PTROL; //ó�� �⺻ ���´� 0�� PTROL

    private EnemyFire enemyFire;

    void Awake()
    { //���ʹ̰� ��Ʈ���ϴ� ��ɺ��� �������� ������ ��� ���� Awake���.
        var player = GameObject.FindGameObjectWithTag(playStr);
        if(player != null)
            playerTr = player.GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        ws = new WaitForSeconds(0.3f); //��ٸ��� �ð� 0.3�ʷ� �̸� �ʱ�ȭ

        moveAgent = GetComponent<EnemyMoveAgent>(); //EnemyMoveAgent��ũ��Ʈ ����
        enemyFire = GetComponent<EnemyFire>(); //EnemyFire��ũ��Ʈ ����
    }
    private void OnEnable() //������Ʈ�� Ȱ��ȭ �� ������. ȣ��
    {
        StartCoroutine(CheckState()); //�Ÿ� �������� state���� ����
        StartCoroutine(Action()); // state���¿� ���� �ִϸ��̼� ���
    }
    IEnumerator CheckState() //state�� �����ϴ� �Լ�
    {
        while(!isDie)
        {
            if(state == State.DIE) yield break; //��� ���¸� StartCoroutine�ٷ� ����
            float dist = (playerTr.position - enemyTr.position).magnitude;
            if (dist <= attackDist) //���� �����Ÿ� ���̶��
                state = State.ATTACK; //state�� ���û���� �ٲ�
            else if (dist <= traceDist) //Ʈ���̽� �����Ÿ� ���̶��
                state = State.TRACE; //state�� Ʈ���̽� ����� �ٲ�
            else //��� �ƴ϶��
                state = State.PTROL; //state�� ��Ʈ�� ����ιٲ�

            yield return ws;//���¸� üũ�� �� 0.3�� �����̸� ����
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws; //0.3�� �� ���� ����ġ�� �ߵ�

            switch (state)
            {
                case State.PTROL:
                    moveAgent.patrolling = true; //��Ʈ�� ������Ƽ�� set�� true�� �ִ´�, ��Ʈ�� �Լ� ������.
                    animator.SetBool(hashMove, true);
                    enemyFire.isFire = false;
                    break;

                case State.ATTACK:
                    moveAgent.Stop(); //�÷��̾ ������ ���� ��� ���缭 ���� �����ϱ� ������ ���� �Լ� �ҷ���.
                    animator.SetBool(hashMove, false);
                    if(enemyFire.isFire == false) //if���� �־ �ǰ� ��� ����� ����.
                        enemyFire.isFire = true;
                    break;

                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position; //�߰� ������Ƽ�� ȣ���Ͽ� player��ġ�� �Է��Ѵ�.
                    animator.SetBool(hashMove, true);
                    enemyFire.isFire = false;
                    break;

                case State.DIE:
                    moveAgent.Stop(); //���ʹ̰� �׾��� ��� ���ڸ����� �������.
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
