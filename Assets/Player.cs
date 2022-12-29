using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 3f; //player �����϶� �̵��ӵ� 
    public float jumpPower = 8f;
    float hAxis; //player�� horizontalAxis���� ���� ����
    float vAxis; // verticalAxis ���� ���� ����
    bool isJumping; //Jump() ȣ������ ������ �����ϴ� ��
    bool walkDown; //�÷��̾ �Ȱ��ִ��� �ƴ���

    Vector3 moveVec = new Vector3(0,0,0); // �� ���� ���ڷι޴� ���Ͱ�ü

    Rigidbody rigid;

    GameObject floor; //�ٴ� ������Ʈ (�����Ҷ� �ٴڿ� �����������, ������ �� �ְԸ��������)
    bool onFloor; // �ٴڿ� ����ִ��� �ƴ���

    Animator ani;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //GetComponent<T> >> ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� �ٸ� ������Ʈ �����ö� ����ϴ� �Լ�
        floor = GameObject.FindGameObjectWithTag("floor"); // floor�±׸� �߰��ؼ� floor������Ʈ�� �߰�������

        ani = GetComponentInChildren<Animator>(); //animator �ʱ�ȭ
    }

 void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk"); // ����Ʈ �����µ��� walkDown = True; walk��ư�� input�Ŵ������� ���� left shift�� ���� �߰���(���������� ���ӿ��� ����Ʈ������ �ȱ��)


        if (Input.GetButtonDown("Jump") && onFloor)
            isJumping = true;

        ani.SetBool("isRun", moveVec != Vector3.zero);
        ani.SetBool("isWalk", walkDown); //
    }
    void FixedUpdate()
    {
        Move(hAxis, vAxis, walkDown);
        Turn();
        Jump();
    }

    // �����϶� ĳ���Ͱ� ���� ���� ���� rotation ����
    void Turn()
    {
        if (hAxis == 0 && vAxis == 0) return;
        //Quaternion ȸ������ �ٷ�� Ŭ���� ȸ������ ���� �� �ִ�
        //Transform ���� X, Y, Z ���� ���� �����Ͽ� ����x �������ִ� �Լ��� ����ؾߵ�

        Quaternion rotation = Quaternion.LookRotation(moveVec);

        rigid.rotation = Quaternion.Slerp(rigid.rotation, rotation, 0.01f * Time.deltaTime);
        //Slerp�� �ȳ����� ���İ��� ������ �� ���ư��� ȸ���ϴ� ������ �ȵ�

        rigid.MoveRotation(rotation);
    }

    // Update is called once per frame
    
    
    // �÷��̾� ������ ������ ����
    void Move(float h, float v, bool isWalk)
    {
        moveVec.Set(h, 0, v);

        rigid.MovePosition(transform.position + moveVec * (isWalk ? speed * 0.7f : speed) * Time.deltaTime);
        //���� �÷��̾� ��ġ�� transform.position ���ٰ� �������� ���� 
        //iswalk�� ���̸� ���� speed�� 7/10�� �ӵ��� ��
    }
    void Jump()
        {
            if (!isJumping || !onFloor) return;

        
            //rigid.MovePosition(transform.position + Vector3.up); //�̷����ϸ� �׳� ������ �ܼ��� ���̸�ŭ �̵���Ű�� ������ ���ڿ�������

            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); //addForce(�����ִ� ����,�������)�� ������ �������� ���� �ִ� �Լ�
                                                            //ForceMode�� �������� ���� �ʿ��ϹǷ� impulse
        
            isJumping = false;
        }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == floor)//�ٴڿ� ����ִ�
        {
            onFloor = true;
        }
    }
    //�浹�� ������ �̺�Ʈ
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == floor)//�ٴڿ��� ��������
        {
            onFloor = false;
        }
    }

   
}
