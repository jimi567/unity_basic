using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 3f; //player 움직일때 이동속도 
    public float jumpPower = 8f;
    float hAxis; //player의 horizontalAxis값을 받을 변수
    float vAxis; // verticalAxis 값을 받을 변수
    bool isJumping; //Jump() 호출할지 말지를 결정하는 값
    bool walkDown; //플레이어가 걷고있는지 아닌지

    Vector3 moveVec = new Vector3(0,0,0); // 위 값을 인자로받는 벡터객체

    Rigidbody rigid;

    GameObject floor; //바닥 오브젝트 (점프할때 바닥에 닿아있을때만, 점프할 수 있게만들기위함)
    bool onFloor; // 바닥에 닿아있는지 아닌지

    Animator ani;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //GetComponent<T> >> 스크립트가 가지고 있는 오브젝트의 다른 컴포넌트 가져올때 사용하는 함수
        floor = GameObject.FindGameObjectWithTag("floor"); // floor태그를 추가해서 floor오브젝트에 추가해줬음

        ani = GetComponentInChildren<Animator>(); //animator 초기화
    }

 void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk"); // 쉬프트 누르는동안 walkDown = True; walk버튼은 input매니저에서 따로 left shift로 따로 추가함(보편적으로 게임에서 쉬프트눌러야 걷기라서)


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

    // 움직일때 캐릭터가 보는 방향 조정 rotation 조정
    void Turn()
    {
        if (hAxis == 0 && vAxis == 0) return;
        //Quaternion 회전만을 다루는 클래스 회전값을 정할 수 있다
        //Transform 같이 X, Y, Z 축을 직접 접근하여 변경x 제공해주는 함수를 사용해야됨

        Quaternion rotation = Quaternion.LookRotation(moveVec);

        rigid.rotation = Quaternion.Slerp(rigid.rotation, rotation, 0.01f * Time.deltaTime);
        //Slerp을 안넣으면 순식간에 옵젝이 휙 돌아가서 회전하는 느낌이 안듦

        rigid.MoveRotation(rotation);
    }

    // Update is called once per frame
    
    
    // 플레이어 옵젝의 움직임 구현
    void Move(float h, float v, bool isWalk)
    {
        moveVec.Set(h, 0, v);

        rigid.MovePosition(transform.position + moveVec * (isWalk ? speed * 0.7f : speed) * Time.deltaTime);
        //현재 플레이어 위치인 transform.position 에다가 움직임을 더함 
        //iswalk가 참이면 기존 speed에 7/10의 속도로 감
    }
    void Jump()
        {
            if (!isJumping || !onFloor) return;

        
            //rigid.MovePosition(transform.position + Vector3.up); //이렇게하면 그냥 옵잭을 단순히 높이만큼 이동시키기 때문에 부자연스러움

            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); //addForce(힘을주는 방향,포스모드)는 정해진 방향으로 힘을 주는 함수
                                                            //ForceMode는 순간적인 힘이 필요하므로 impulse
        
            isJumping = false;
        }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == floor)//바닥에 닿아있다
        {
            onFloor = true;
        }
    }
    //충돌이 끝나는 이벤트
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == floor)//바닥에서 떨어졌다
        {
            onFloor = false;
        }
    }

   
}
