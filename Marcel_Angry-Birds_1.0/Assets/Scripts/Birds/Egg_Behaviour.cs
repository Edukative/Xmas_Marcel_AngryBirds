using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg_Behaviour : MonoBehaviour
{
    private PolygonCollider2D Collider;
    private Rigidbody2D RigiEgg;
    public PointEffector2D Point;
    public GameObject Explosion;
    private SpriteRenderer Sprite;
    private float Lerp1;
    private Transform GCamera;

    // Start is called before the first frame update
    void Start()
    {

        RigiEgg = GetComponent<Rigidbody2D>();
        Point = GetComponent<PointEffector2D>();
        Collider = GetComponent<PolygonCollider2D>();
        

        RigiEgg.velocity = new Vector2(0, -10);
        Sprite = GetComponent<SpriteRenderer>();


    }

    private void Update()
    {
        Lerp1 = Lerp1 + 0.005f;

        if (Lerp1 > 1)
        {

            Lerp1 = 1f;

        }

        GCamera = GameObject.Find("Main Camera").GetComponent<Transform>();


        GCamera.transform.position = Vector2.Lerp(GCamera.transform.position, transform.position, Lerp1);


        GCamera.transform.position += new Vector3(0, 0, -10);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        RigiEgg.velocity = Vector2.zero;
        RigiEgg.isKinematic = true;
        Collider.usedByEffector = true;

        var Exp = Instantiate(Explosion, transform.position, Quaternion.identity);

        Destroy(Sprite);

        Destroy(Exp, 0.25f);
        Destroy(gameObject, 0.25f);
        


    }
}
