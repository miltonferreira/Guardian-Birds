using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject[] columns;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(dropColumns());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

IEnumerator dropColumns(){
    
    GameObject c = Instantiate(columns[0]);
    c.transform.position = new Vector2(player.position.x + 14f, transform.position.y);

    yield return new WaitForSeconds(5f);
    StartCoroutine(dropColumns());

}

}
