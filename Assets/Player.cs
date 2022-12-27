using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 1f; //player �����϶� �̵��ӵ� 
    float hAxis; //player�� horizontalAxis���� ���� ����
    float vAxis; // verticalAxis ���� ���� ����
    bool isJumping; //Jump() ȣ������ ������ �����ϴ� ��

    Vector3 moveVec = new Vector3(0,0,0); // �� ���� ���ڷι޴� ���Ͱ�ü

    Rigidbody rigid;

    GameObject floor; //�ٴ� ������Ʈ (�����Ҷ� �ٴڿ� �����������, ������ �� �ְԸ��������)
    bool onFloor; // �ٴڿ� ����ִ��� �ƴ���


    void Start()
    {
        
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //GetComponent<T> >> ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� �ٸ� ������Ʈ �����ö� ����ϴ� �Լ�
        floor = GameObject.FindGameObjectWithTag("floor"); // floor�±׸� �߰��ؼ� floor������Ʈ�� �߰�������
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
    void FixedUpdate()
    {
        Move(hAxis, vAxis);
        Turn();
        Jump();
    }
    
    // �÷��̾� ������ ������ ����
    void Move(float h, float v)
    {
        moveVec.Set(h, 0, v);
        rigid.MovePosition ( transform.position + moveVec * speed * Time.deltaTime );
        //���� �÷��̾� ��ġ�� transform.position ���ٰ� �������� ����
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

    void Jump()
    {
        if (!isJumping || !onFloor) return;

        
        //rigid.MovePosition(transform.position + Vector3.up); //�̷����ϸ� �׳� ������ �ܼ��� ���̸�ŭ �̵���Ű�� ������ ���ڿ�������

        rigid.AddForce(Vector3.up * 3f, ForceMode.Impulse); //addForce(�����ִ� ����,�������)�� ������ �������� ���� �ִ� �Լ�
                                                            //ForceMode�� �������� ���� �ʿ��ϹǷ� impulse
        
        isJumping = false;
    }
     void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && onFloor)
            isJumping = true;
    }
}
