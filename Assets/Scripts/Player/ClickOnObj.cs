using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class ClickOnObj : MonoBehaviour
{
     Vector2 initialPosition;
    private float distance;

    private Rigidbody2D _rb2D;

    private bool _drag;             // indica que pode mover o obj

    // atributos do mouse -------------------------
    private Vector2 _mouse;

    // Offset entre player e mouse
    [SerializeField]private Vector2 _offset;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        // pega posição do mouse no espaço da tela
        _mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(_mouse);

        //follow();
        rayHit();
        followInit(_mouse);
    }

    void followInit(Vector2 mousePosition){

        if(Input.GetMouseButton(0) && _drag){

            _rb2D.isKinematic = true;

            //temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
            //this.transform.position = Camera.main.ScreenToWorldPoint(temp);

            this.transform.position = mousePosition+_offset;
        }else{
            _rb2D.isKinematic = false;
        }

        if(Input.GetMouseButtonUp(0) && _drag){
            _drag = false;
        }
    }

    void rayHit(){

        // faz calculo de offset entre player e mouse
        if(Input.GetMouseButton(0) && !_drag){
            float _x = transform.position.x - _mouse.x;
            float _y = transform.position.y - _mouse.y;

            _offset = new Vector2(_x,_y);
        }

        //If the left mouse button is clicked.
        if (Input.GetMouseButton(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.name);
                _drag = true;
            }
        }
    }

    void follow(){
        // called the first time mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            initialPosition = transform.position;

            Vector2 rayPoint = ray.GetPoint(0);

            // Not sure but this might get you a slightly better value for distance
            distance = Vector2.Distance(transform.position, rayPoint );

            Debug.Log(distance);
        }

        // called while button stays pressed
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(distance);

            _rb2D.MovePosition(initialPosition + new Vector2(rayPoint.x, rayPoint.y));
            
        }
    }

}
