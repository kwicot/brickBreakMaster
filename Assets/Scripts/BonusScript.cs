using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour
{
    GameManager _gameManager;
    public int ID;
    

    Vector3 _newPos;
    bool _move = false;

    public float step;
    public bool isDebug;

    public GameObject additiveAmmo;
    public Canvas canvas;
    int _ammoId;
    private int _ownId;

    bool _activated = false;
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
        _ammoId = 10000;
        if (isDebug)
            _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (_move)
        {
            transform.position = Vector2.Lerp(transform.position, _newPos, .25f);
            if (transform.position == _newPos) _move = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ammo")
        {
            switch (ID)
            {
                case 0:
                    {
                        destroy();
                        break;
                    }
                case 1:
                    {
                        if (collision.GetComponent<AmmoScript>().canTakeExtraLife == true)
                        {

                            collision.GetComponent<AmmoScript>().takeExtraLife();
                            _activated = true;
                        }
                        break;
                    }
                case 2:
                    {
                        MakeDamageRight();
                        GetComponent<Animation>().Play();
                        _activated = true;
                        break;
                    }
                case 3:
                    {
                        MakeDamageUp();
                        GetComponent<Animation>().Play();
                        _activated = true;
                        break;
                    }
                case 4:
                    {
                        MakeDamageUp();
                        MakeDamageRight();
                        GetComponent<Animation>().Play();
                        _activated = true;
                        break;
                    }
                case 5:
                    {
                        destroy();
                        break;
                    }
                case 6:
                    {
                        
                        Physics2D.IgnoreLayerCollision(8, 11);
                        Physics2D.IgnoreLayerCollision(11, 12);
                        Physics2D.IgnoreLayerCollision(11, 11);
                        AmmoScript ammo = collision.GetComponent<AmmoScript>();
                        Vector2 dir = ammo.getDirect();
                        float speed = ammo.getSpeed();
                        ammo.ChangeDirect();
                        Vector2 position = ammo.gameObject.transform.position;
                        GameObject additiveammo = Instantiate(additiveAmmo, position, Quaternion.identity,canvas.transform);
                        additiveammo.GetComponent<AmmoScript>().setParram(dir, speed, _ammoId);
                        additiveammo.GetComponent<AmmoScript>().gameManager = _gameManager;
                        _ammoId++;
                        _gameManager.AddAmmo(additiveammo);
                        _activated = true;
                        break;
                    }
                case 7:
                    {
                        Vector2 dir;
                        int a;
                        a = Random.Range(0, 11);
                        int b;
                        b = Random.Range(1, 5);
                        if (a >= 6)
                        {
                            dir = Vector2.up + Vector2.right * b;
                        }
                        else
                        {
                            dir = Vector2.up + Vector2.left * b;
                        }
                        collision.GetComponent<AmmoScript>().ChangeDirect(dir.normalized);
                        _activated = true;
                        break;
                    }

            }
        }
        

    }
    public void MakeDamageRight()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.left);
        for(int i = 0; i < hit.Length; i++)
        {
            if(hit[i].collider.gameObject.GetComponent<BlockScript>() != null)
            {
                hit[i].collider.gameObject.GetComponent<BlockScript>().TakeDamage();
            }
        }
        hit = Physics2D.RaycastAll(transform.position, Vector2.right);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.GetComponent<BlockScript>() != null)
            {
                hit[i].collider.gameObject.GetComponent<BlockScript>().TakeDamage();
            }
        }
    }
    public void MakeDamageUp()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.up);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.GetComponent<BlockScript>() != null)
            {
                hit[i].collider.gameObject.GetComponent<BlockScript>().TakeDamage();
            }
        }
        hit = Physics2D.RaycastAll(transform.position, Vector2.down);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.GetComponent<BlockScript>() != null)
            {
                hit[i].collider.gameObject.GetComponent<BlockScript>().TakeDamage();
            }
        }
    }

    public void setParram(Vector3 posittion,GameManager game)
    {
        _gameManager = game;
    }
    public void setParram(GameManager game)
    {
        _gameManager = game;
        
    }
    public void setParram(GameManager game, int id, Vector3 posittion, float sstep)
        {
            transform.position = posittion;
            _gameManager = game;
            _ownId = id;
        step = sstep;
        }

    public void movePosition()
    {
        Vector3 screenposition = Camera.main.WorldToScreenPoint(transform.position);
        screenposition.y -= step;
        //transform.position = Vector2.Lerp(transform.position,Camera.main.ScreenToWorldPoint(screenposition),1);
        _newPos = Camera.main.ScreenToWorldPoint(screenposition);
        _move = true;
        if (isDebug)
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(screenposition));
            Debug.Log(screenposition);
        }
        if (_activated) destroy();
        //if (screenposition.y <= 110) gameManager.GameOver();
        if (screenposition.y <= 110) Destroy(gameObject);
    }

    public void destroy()
    {
        _gameManager.TakedBonus(ID, this.gameObject);
    }
    public void SetSize(float size)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(size,size,0);
    }

}
