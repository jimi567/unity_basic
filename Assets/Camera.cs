using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Player;
    public Vector3 offset = new Vector3(0f,1f,-2f); //카메라를 플레이어 따라가게 만들떄 필요한 offset값
    void Start()
    {
        transform.position = Player.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + offset;
        
    }
}
