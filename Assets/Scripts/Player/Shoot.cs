using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shoot : MonoBehaviour
{

    public float power = 2.0f;
    public float life = 1.0f;
    public float dead_sense = 25f;

    public int dots = 30;

    private Vector2 startPosition;
    private bool shoot, aiming, hit_ground;

    private GameObject Dots;
    private List<GameObject> projectilesPath;

    private Rigidbody2D rb2D;

    private Collider2D mycollider;

    private SpriteRenderer sprite;

    private Vector2 _mouse;
    private bool isClickOver;

    public bool isClick {
        get{return isClickOver;}
    }

    public LayerMask layer;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        mycollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

    }

    void Start() {
        Dots = GameObject.Find("dots");
        //rb2D.isKinematic = true;
        //mycollider.enabled = false;
        startPosition = transform.position;

        projectilesPath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        
        for(int i = 0; i < projectilesPath.Count; i++){
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //_mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickMouse();

        if(isClickOver){
            rb2D.velocity = new Vector2(0f,0f);
            transform.rotation = Quaternion.Euler(0f,0f,0f);
            rb2D.gravityScale = 0f;
        }else{
            rb2D.gravityScale = 1f;
        }

        Aim();

        if(hit_ground){
            shoot = false;
        }

        if(rb2D.velocity.x > 1f){
            // direita
            sprite.flipX = false;
            
        }else if(rb2D.velocity.x < -1f){
            // esquerda
            sprite.flipX = true;
        }

        // if(hit_ground){
        //     life -= Time.deltaTime;
        //     Color c = GetComponent<Renderer>().material.GetColor("_Color");
        //     GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r, c.b, life));
            
        //     if(life < 0){

        //         // if(GameManager.instance != null){
        //         //     GameManager.instance.CreateBall();
        //         // }

        //         Destroy(gameObject);
        //     }
        // }

    }

    void Aim(){

        if(Input.GetAxis("Fire1") == 1){
            if(!aiming){
                aiming = true;
                startPosition = Input.mousePosition;
                CalculatePath();
                ShowPath();
            } else {
                CalculatePath();
            }
        } else if(aiming && !shoot){
            if(inDeadZone(Input.mousePosition) || inReleaseZone(Input.mousePosition)){
                aiming = false;
                HidePath();
                return;
            }
            //rb2D.isKinematic = false;
            //mycollider.enabled = true;
            
            shoot = true;
            aiming = false;

            rb2D.AddForce(GetForce(Input.mousePosition));
            HidePath();
        }
    }

    Vector2 GetForce(Vector3 mouse){
        return (new Vector2(startPosition.x, startPosition.y) - new Vector2(mouse.x, mouse.y)) * power;
    }

    bool inDeadZone(Vector2 mouse){
        if(Mathf.Abs(startPosition.x - mouse.x) <= dead_sense && Mathf.Abs(startPosition.y - mouse.y) <= dead_sense){
            return true;
        } else {
            return false;
        }
    }

    bool inReleaseZone(Vector2 mouse){
        if(mouse.x <= 70){
            return true;
        } else {
            return false;
        }
    }

    void CalculatePath(){
        Vector2 vel = GetForce(Input.mousePosition) * Time.fixedDeltaTime / rb2D.mass;

        for(int i = 0; i < projectilesPath.Count; i++){
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
            float t = i / 30f;
            Vector3 point = PathPoint(transform.position, vel, t);
            point.z = 1.0f;
            projectilesPath[i].transform.position = point;
        }
    }

    Vector2 PathPoint(Vector2 startP, Vector2 startVel, float t){
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
    }

    void HidePath(){
        for(int i = 0; i < projectilesPath.Count; i++){
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    void ShowPath(){
        for(int i = 0; i < projectilesPath.Count; i++){
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D target) {
        if(target.gameObject.tag == "Ground"){
            hit_ground = true;
        }
    }

    void clickMouse(){
        if (Input.GetMouseButton(0) && !isClickOver)
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, layer);
            

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                isClickOver = true;
            }
        }

        if(Input.GetMouseButtonUp(0) && isClickOver){
            isClickOver = false;
        }
    }
}
