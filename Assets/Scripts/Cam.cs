using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{

    public Transform player;

    public bool isFollowY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate(){
        if(!isFollowY){
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }else{
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}
