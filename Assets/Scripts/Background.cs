using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    private MeshRenderer mesh;

    private float _x;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _x += Time.deltaTime * 0.1f;
        mesh.material.mainTextureOffset = new Vector2(_x, 0f);
    }
}
