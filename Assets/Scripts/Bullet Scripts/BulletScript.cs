using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody myBody;
    
    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    public void Move(float speed)
    {
        myBody.AddForce(transform.forward.normalized * speed);
        
        Invoke("DeactivateGameObject", 5f);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == MyTags.OBSTACLE_TAG)
        {
            gameObject.SetActive(false);
        }
    }
}
