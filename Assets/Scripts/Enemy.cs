using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 50;
    public bool isDead = false;
    public bool isHit;
    private TextMeshProUGUI healthText;
    public GameObject healthGO;
    private Vector3 deathPosition;
    private Quaternion deathRotation;
    private GameObject soldier;
    private float delay = 0.433f;
    private float time = 0f;
    void Start()
    {
        health = 50;
        isDead = false;
        healthText = healthGO.GetComponent<TextMeshProUGUI>();
        healthText.rectTransform.position = transform.position + transform.forward * 0.5f + transform.up * 2;
        healthText.text = health.ToString();
        //set soldier to the gameobject attacted to this script
        soldier = gameObject;

    }

    // Update is called once per frame
    void Update()
    {  
        if (!isDead)
        {   
            healthText.rectTransform.position = transform.position + transform.forward * 0.5f + transform.up * 2;
            if (health <= 0)
            {
                // Destroy(gameObject, 1.5f);
                isDead = true;
                deathPosition = transform.position;
                deathRotation = transform.rotation;
                this.gameObject.SetActive(false);
            }
            healthText.text = health.ToString();

        } else
        {   
            healthGO.SetActive(false);
            // disable the collider
            GetComponent<Collider>().enabled = false;
            // assert that the enemy is not moving
            transform.position = deathPosition;
            transform.rotation = deathRotation;
            


            

        } 
        //make the enemy upright all the time
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    }    

    // void ResetHit()
    // {
    //     isHit = false;
    // }

    // public void shot()
    // {
    //     Destroy(gameObject, 1.5f);

    // }


}
