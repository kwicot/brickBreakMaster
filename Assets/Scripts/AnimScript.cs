using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAnim()
    {
       // string anim = GetComponent<Animation>().clip.name;
        GetComponent<Animation>().Play("test");
    }
}
