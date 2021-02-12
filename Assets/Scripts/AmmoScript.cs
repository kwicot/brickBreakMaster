using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class AmmoScript : MonoBehaviour
{
    public GameManager gameManager;

    public int ID;
    bool extraLife;
    public bool canTakeExtraLife = true;
    float Speed;
    Vector2 Direct;
    Rigidbody2D rb;




    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8);
        rb = GetComponent<Rigidbody2D>();
        MoveAmmo();
    }

    void Update()
    {
    }
    void MoveAmmo()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Direct * Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //if (collision.gameObject.tag != "Ammo")
        //{
        //    Vector2 direction = Vector2.Reflect(Direct, collision.GetContact(0).normal);
        //    Direct = direction;
        //    MoveAmmo();

        //}
        if (collision.gameObject.tag == "bottom")
        {

            if (extraLife)
            {

                //gameObject.GetComponent<Image>().SetEnabled(false);
                extraLife = false;
                GetComponent<Image>().color = Color.white;
            }
            else
            {

                gameManager.DestroyedAmmo(transform.position, ID);

            }
        }
    }

    public void setParram(Vector2 direct, float speed,int id)
    {
        Direct = direct;
        Speed = speed;
        ID = id;
    }
    public float getSpeed()
    {
        return Speed;
    }

    public void toHome(Vector2 pos)
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        extraLife = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(pos * Speed);
    }


    public void takeExtraLife()
    {
        if (canTakeExtraLife)
        {
            extraLife = true;
            canTakeExtraLife = false;
            GetComponent<Image>().color = Color.yellow;

        }
    }
    public Vector2 getDirect()
    {
        Vector2 curDir = rb.velocity;
        Vector2 right = transform.right;
        return (curDir + right*4).normalized;
    }
    public void ChangeDirect()
    {
        Vector2 currDir = rb.velocity;
        Vector2 right = transform.right;
        Vector2 newdir;
        newdir = currDir + -right*4;
        Direct = newdir.normalized;
        MoveAmmo();
    }
    public void ChangeDirect(Vector2 dir)
    {
        Direct = dir;
        MoveAmmo();
    }
    public void RandomDirect()
    {


    }
    public void SetSize(float size)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(size, size, 0);
        CircleCollider2D box = gameObject.GetComponent<CircleCollider2D>();
        box.radius = size / 2;
    }

}
