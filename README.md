# unity_basic
A repository for recording the process of learning how to use Unity before a Unity projec

### 에셋 적용

+ 내 에셋스토어 - 갖고오고싶은 옵젝 import - 적용

### Unity LifeCycle

              
#### 초기화 영역  -->  물리연산 영역 --> 게임로직 영역 --> 해체영역
####            활성화영역                          비활성화영역   
+ 초기화 영역
  + Awake() : 최초로 로딩이 될때, 딱 1번 실행됨 여러 기본값을 설정함
  + Start() : 업데이트 시작전에 딱 1번 실행됨

+ 활성화 : 옵젝 켜질 때 실행, 여러 번 실행 가능(로그인같은 느낌)

+ 물리연산 영역
  + FixedUpdate() : 유니티 엔진이 물리연산을 하기 전에 실행됨 업데이트함수는 1초에 여러번 작동되는데
이 함수는 1초에 50번 사용됨 고정된 실행 주기로 CPU많이 사용 
물리연산과 리지디바디에 대한 로직이 들어감

+ 게임로직 영역
    + Update() : 물리연산에 관련된 로직을 제외한 나머지 주기적으로 변하는 로직을 넣을때 사용하는 하나의 프레임마다 실행되고, 최대 1초당 60프레임까지 실행됨 (프레임이 초당 70이라도 60번만)
  FixedUpdate(고정된 실행 주기)와는 다르게 환경에 따라 실행 주기가 떨어질 수 있음, 

    + LateUpdate() : 모든 업데이트  로직들이 실행이 끝난 뒤에 마지막으로 호출되는 함수
(보통 이곳에는 캐릭터를 따라가는 카메라 로직의 후처리를 담는곳)
Update()와 실행주기가 같음. 물리연산 영역의 FixedUpdate와 게임로직 영역의 Update등의 업데이트 과정이 끝난뒤 
실행이 되기 때문이다

+ 비활성화 : 게임로직 영여과 해체 영역 사이 실행 (로그아웃?)

+ 해체영역
  + Destory() : 옵젝 삭제되기 전에 실행, 옵젝 삭제전 무언가를 남길때 사용(rpg게임에서 드롭아이템 있을때, 이곳에서 구현하나?)



### 3D 오브젝트의 컴포넌트
+ Transform
  - 기본 속성, 위치 각도 크기를 설정 할 수 있음
+ Rigidbody  
  - 물리 속성, 무게와 물리효과를 설정할 수 있는 컴포넌트
  - Mass : 무게, 높을수록 충돌이 무거워짐,두 물체가 충돌할 때 영향을 줌, 자유낙하가 빨라지지는 않음
  - Drag : 낙하 저항 , 높을수록 천천히 낙하
  - Angular Drag : 회전 저항 ,높을 수록 천천히 회전
  - IsKinematic : 외부 물리 효과의 영항을 받는 지의 여부, 옵션을 키면 오직 애니메이션, 스크립트에 의해서만 움직임
  - Interpolation : 고정 프레임에서 물리 효과를 부드럽게 할 수 있음, 일반적으로 플레이어 캐릭터에 저굥ㅇ됨
  - Collithon Detection : 연속 충돌 감지 옵션, 빠른 옵젝이 다른 옵젝을 충돌하지 않고, 통과해버리는 현상을 방지함
  - Constraints : 위치나 회전을 고정시키는 옵션
+ Collider
  - 충돌 속성 이벤트를 위한 트리거로도 사용가능 ( 플레이어옵젝이 함정 옵젝이랑 충돌하면 함정 발동 등)
  - 연두색 경계선으로 표현됨, 형태는 여러가지로 만들 수 있음, 
### 플레이어 옵젝에 중력적용 하기

충돌은 collider 기준이기 때문에 실제 오브젝트의 매쉬와는 상관없이 collider에 따라 충돌이 다르게 일어 날 수 있음
collder 크기를 플레이어옵젝에 발부분과 맞겠금 값을 조절해주었음 
***
###  이동 구현

` public float speed = 1f; //player 움직일때 이동속도 `
이동속도를 전역변수로 만들었음 접근자를 public으로 만들면 유니티에서도 값을 수정할 수 있음

라이프 사이클중에는 Awake() 와 FixedUpdate()사용 FixedUpdate를 사용하는 이유는 이동 로직에 리지드바디(Rigidbody)를 쓰기 때문  
키입력은 Update() 안에서 구현, 각각의 프레임 안에서 정확한 입력을 받기 위해
  
+ Update () : 
```
 hAxis = Input.GetAxisRaw("Horizontal"); // d키 입력받으면 x축에서 +방향(왼쪽)으로 이동 a키를 입력받으면 -방향(오른쪽)
 vAxis = Input.GetAxisRaw("Vertical"); // w키 입력받으면 y축에서 방향(윗쪽)으로 이동 s키를 입력받으면 -방향(오른쪽)
```
+ Awake() :  
` rigid = GetComponent<Rigidbody>();`
  GetComponent<T> >> 스크립트가 가지고 있는 오브젝트의 다른 컴포넌트 가져올때 사용하는 함수

+ FixedUpdate() :  
Move(hAxis, vAxis), Turn(), 플레이어를 이동시키는 Move함수와 플레이어가 이동방향을 보게끔 만드는 Turn함수 호출

+ Move()

```
void Move(float h, float v)
    {
        moveVec.Set(h, 0, v);
        rigid.MovePosition ( transform.position + moveVec * speed * Time.deltaTime );
        //현재 플레이어 위치인 transform.position 에다가 움직임을 더함 
        //Time.deltaTime은 시스템 여건에 따라 다른 프레임에 따라서 로직 결과가 달라지는 것을 방지해줌
        // 60프레임일때나, 40프레임일때나 이동한 거리는 동일하도록 서로 보정해주는 역할을 함
    }
    
```
    
    
+ Turn()

```
void Turn()
    {
        if (hAxis == 0 && vAxis == 0) return; //이동중이 아니면 return
        //Quaternion 회전만을 다루는 클래스 회전값을 정할 수 있다
        //Transform 같이 X, Y, Z 축을 직접 접근하여 변경x 제공해주는 함수를 사용해야됨

        Quaternion rotation = Quaternion.LookRotation(moveVec);

        rigid.rotation = Quaternion.Slerp(rigid.rotation, rotation, 0.01f * Time.deltaTime);
        //Slerp을 안넣으면 순식간에 옵젝이 휙 돌아가서 회전하는 느낌이 안듦

        rigid.MoveRotation(rotation);
    }
```
***
### 점프 구현(1단점프만 점프중에 또 점프가 적용되지 않도록)

+ Updata 라이프 사이클에 "Jump" 버튼 입력 받음(디폴트키는 스페이스바)  
```
bool isJumping // 점프키를 눌렀는지 안눌렀는지
if (Input.GetButtonDown("Jump") && onFloor)
            isJumping = true;
            
```

+ FixedUpdate()에서 jump 함수 호출

+ Jump()
```
void Jump()
    {
        if (!isJumping || !onFloor) return; //점프키를 입력받지 않았을때 && 바닥위에 있지않을때(밑에서 설명) 아무작업없이 리턴 

         //이렇게하면 그냥 옵잭을 단순히 높이만큼 이동시키기 때문에 부자연스러움 기각
        //rigid.MovePosition(transform.position + Vector3.up); 
        
        rigid.AddForce(Vector3.up * 3f, ForceMode.Impulse); //addForce(힘을주는 방향,포스모드)는 정해진 방향으로 힘을 주는 함수
                                                            //ForceMode는 순간적인 힘이 필요하므로 impulse
        
        isJumping = false; //점프를 마친 상태로 되돌려줌 이게 없으면 한번 점프버튼 누르면 쭉 위로 가버림
    }
```

* 점프키를 그냥 구현했는데 문제가 발생했음  

내가 원하는건 1단 점프 다단점프가 가능하면 점프상태에서 계속 점프를 하면서 맵밖으로 나갈 수가 있음
그래서 바닥에 있는지 아닌지를 collider의 트리거 기능을 이용해서 구분

```
    //충돌 중인 이벤트 바닥에 닿아있음
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == floor)//바닥에 닿아있다
        {
            onFloor = true;
        }
    }
    //충돌이 끝나는 이벤트 바닥에서 떨어져 있음
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == floor)//바닥에서 떨어졌다
        {
            onFloor = false;
        }
    }
```
이걸 추가한 이후에 점프중에 점프가 안되도록 1단점프만 가능하도록 구현함
***
### 플레이어 애니메이션 추가 +걷기 추가

* 플레이어 걷기(walk)구현 
먼저 left shift를 눌렀을때, 걷는 행동을 할 수 있게만드는 "Walk" 버튼을 추가했음
버튼 추가는 unity Project Settings --> Input Manager에서 추가가능

이 캐릭터가 걷고있는지아닌지를 판단해주는 bool값을 전역변수로 추가
```
bool walkDown; //플레이어가 걷고있는지 아닌지
```

Update() 
``` 
walkDown = Input.GetButton("Walk"); 쉬프트 누르는동안 walkDown = True; walk버튼은 input매니저에서 따로 left shift로 따로 추가함(보편적으로 게임에서 쉬프트눌러야 걷기라서)
```
Move() Move함수 인자에 isWalk를 추가해서 이 캐릭터가 걷고 있는지 아닌지를 판단
```
void Move(float h, float v, bool isWalk)
    {
        moveVec.Set(h, 0, v);

        rigid.MovePosition(transform.position + moveVec * (isWalk ? speed * 0.7f : speed) * Time.deltaTime);
        //현재 플레이어 위치인 transform.position 에다가 움직임을 더함 
        //iswalk가 참이면 기존 speed에 7/10의 속도로 감
    }
```

* 애니메이션 추가 (애니메이션 모델은 에셋에 있는걸 활용)
  - Asset에 Animator controller(애니메이션을 컨트롤하는 컴포넌트) 추가 --> player 오브젝트에 자식 오브젝트중 Mesh Object에 애니메이션 컨트롤러 (??? : 왜 여기다가 추가해야되지? player 오브젝트에 직접추가하면 안되는건가?)

* Mecanim(애니메이션 시스템)
  - 구성요소 : 애니메이터, 아바타 , 애니메이션 클립, 애니메이터 컨트롤러
  - 메카님을 이용하는데 가장 먼저 필요한 것이 바로 Animator Component 
  - 애니메이터 컨트롤러
    <img src="img/애니메이션 컨트롤러.PNG">
    애니메이션 컨트롤러는 'state machine' 과 거기에 연결된 animation clip을 갖는 에셋  
    화살표가 트렌지션, 네모박스가 스테이트
    
    컴포넌트가 아닌 에셋이라서 게임 오브젝트에 직접 적용할 수가 없음 (??? 궁금증 해결,,)
    애니메이터의 프로퍼터에 있는 애니메이터 컨트롤러로 링크를 설정해서 사용함
  
    Idle 스테이트 : 플레이거 가만히 있을 때 적용되는 애니메이션 약간 숨쉬는 듯한 느낌임..  
    Run 스테이트 : 플레이가 뛸때 적용되는 애니메이션  
    Walk 스테이트 : 플레이어가 걸을때 적용되는 애니메이션 (Run + left shift눌렀을때,)  
  
    Idle --> Run : Idle 스테이트에서 isRun이 True 일때( horizontal , vertical버튼이 눌렸을때) Run 스테이트로 이동  
    Idle --> Walk : isRun이 True 일때 + isWalk가 True 일때(walk버튼이 눌렸을때) Walk 스테이트로 이동  
    
    Run --> Idle : Run 스테이트에서 isRun이 False 일때, Idle 스테이트로 이동  
    Run --> walk : Run 스테이트에서 isWalk가 True 일때, Walk 스테이트로 이동  
    
    Walk --> Idle : Walk 스테이트에서 isRun False 일때, Idle 스테이트로 이동(Idle은 이동하지않은 상태니깐 isRun이 False여야 됨)  
    Walk --> Run : Walk 스테이트에서 isWalk False 일때, Run 스테이트로 이동  
  
  - Script
    전역변수에 Animator 객체 생성  
  
      Awake  
    ```
      ani = GetComponentInChildren<Animator>(); //animator 초기화
    ```
      Update
    ```
      ani.SetBool("isRun", moveVec != Vector3.zero);
      ani.SetBool("isWalk", walkDown); 
    ```
    
