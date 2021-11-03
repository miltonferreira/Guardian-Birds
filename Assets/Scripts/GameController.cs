using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public GameObject[] columns;

    public Transform player;

    public int OrcsInScene;
    public int OrcsDead;

    public GameObject gate;

    public GameObject info;

    
    private void Awake() {
        instance = this;
    }

    void Start()
    {
        //StartCoroutine(dropColumns());
        OrcsInScene = GameObject.FindGameObjectsWithTag("Orc").Length;

        StartCoroutine(IEinfo());
    }

    private void Update() {
        if(Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
    }
    
    IEnumerator dropColumns(){
        
        GameObject c = Instantiate(columns[0]);
        c.transform.position = new Vector2(player.position.x + 14f, transform.position.y);

        yield return new WaitForSeconds(5f);
        StartCoroutine(dropColumns());

    }

    public void OrcKill(){

        OrcsDead++;

        if(OrcsDead == OrcsInScene){
            gate.SetActive(false);  // remove barreira pra proxima fase
        }
    }

    IEnumerator IEinfo(){
        yield return new WaitForSeconds(5f);
        if(info !=null)
            info.SetActive(false);
    }

}
