using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private SpriteRenderer sprite;

    private PolygonCollider2D poly2D;

    private Shoot shoot;

    private float _x;
    public float _y;

    private float _z;
    private bool isDown;

    public float speed;
    private float dir;      // direção que o bird vai voar

    private bool isCorner;  // indica que colidiu com corner esquerdo ou direito

    private bool isDeath;
    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        poly2D = GetComponent<PolygonCollider2D>();
        shoot = GetComponent<Shoot>();

        _z = transform.rotation.eulerAngles.z;

        dir = 1f;       // inicia voando pra direita
        isDown = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.y <= -10f){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(isDeath)
            return;

        if(shoot.isClick || rb2D.velocity.x > 6f || rb2D.velocity.x < -6f){

            if(rb2D.velocity.x > 6f){
                dir = 1;
            }else if(rb2D.velocity.x < -6f){
                dir = -1;
            }

            isCorner = false;
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Space)){
            force();
        }

        if(Input.GetAxisRaw("Horizontal") == 1f && !isCorner){
            dir = 1f;
            isCorner = false;
        }else if(Input.GetAxisRaw("Horizontal") == -1f && !isCorner){
            dir = -1f;
            isCorner = false;
        }else if(isCorner){
            dir = 0f;
            _x = 0f;
        }

        if(dir == 1){
            // direita
            _x = speed;
            sprite.flipX = false;
        }else if(dir == -1){
            // esquerda
            _x = -speed;
            sprite.flipX = true;
        }

        rb2D.velocity = new Vector2( _x,rb2D.velocity.y);   // para velocidade horizontal

        rotBird();
    }

    void rotBird(){

        if(isDeath)
            return;

        if(shoot.isClick)
            return;
        
        if(dir == 1){

            if(_z >= -40f && isDown){

                _z -= Time.deltaTime * 150f;
                transform.rotation = Quaternion.Euler(0f,0f,_z);

            }else if(_z <= 37f && !isDown){

                _z += Time.deltaTime * 150f;
                transform.rotation = Quaternion.Euler(0f,0f,_z);

            }else if(_z >= 37){
                isDown = true;  // indica que tá caindo
            }

        }else if(dir == -1){
            
            
            if(_z >= -40f && !isDown){
                // sobe
                _z -= Time.deltaTime * 150f;
                transform.rotation = Quaternion.Euler(0f,0f,_z);

            }else if(_z <= 37f && isDown){
                // desce
                _z += Time.deltaTime * 150f;
                transform.rotation = Quaternion.Euler(0f,0f,_z);

            }else if(_z <= -40f){
                isDown = true;  // indica que tá caindo
            }

        }
        
    }

    public void force(){
        isDown = false;     // indica que NAO tá caindo
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);   // para velocidade de queda
        rb2D.AddForce(new Vector2(rb2D.velocity.x, _y));

    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "GroundShader"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(col.gameObject.tag == "Orc" && (rb2D.velocity.x > 6f || rb2D.velocity.x < -6f)){
            GameController.instance.OrcKill();  // indica que matou um orc
            Destroy(col.gameObject);
        }else if(col.gameObject.tag == "Orc"){
            Death();
        }
    }

    private void OnCollisionStay2D(Collision2D coll) {
        if(coll.gameObject.tag == "Corner"){
            isCorner = true;
        }
    }

    void Death(){
        isDeath = true;
        poly2D.enabled = false;
    }
}
