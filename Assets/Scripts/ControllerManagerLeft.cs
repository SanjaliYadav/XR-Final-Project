using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerManagerLeft : MonoBehaviour
{
    private GameObject selectedObject;
    private Rigidbody HitObject;
    public float moveForce;
    public GameObject bullet;
    public Transform gun;
    private float fireRate = 200f;
    private float nextTimeToFire = 0.0f;
    private int bulletCount;
    private TextMeshProUGUI timerText;
    public GameObject timerTextGameObject; 
    private float timeRemaining = 60;
    public GameObject timeUp;
    public GameObject gameOver;
    public GameObject startInstructions;
    public GameObject leftGunScore;
    private TextMeshProUGUI leftGunScoreText;
    private float shootingRange = 100f;
    private Vector3 endPosition;
    private int enemyHealth = 100;
    int layerMask = 1 << 8;
    private bool hitOnce;
    private bool soundPlayedOnce;
    public GameObject dirt;
    public GameObject blood;
    public GameObject muzzleFlash; 
    public LineRenderer lineRenderer;
    public AudioSource gunshotSound;
    public AudioClip gunshotSoundClip;
    public GameObject enemy;
    private float startInstructionTime = 10f;
    public int playerHealth;
    public Transform gunPoint; 
    bool nextFireReady;

    void Start()
    {
        bulletCount = 50;
        playerHealth = 100;
        timerText = timerTextGameObject.GetComponent<TextMeshProUGUI>();
        timeUp.SetActive(false);
        gameOver.SetActive(false);
        leftGunScoreText = leftGunScore.GetComponent<TextMeshProUGUI>();
        leftGunScoreText.rectTransform.position = gun.position + Vector3.back * 0.12f + Vector3.up * 0.03f;
        SetCountText(timeRemaining);
        layerMask = ~layerMask;
        hitOnce = false;
        soundPlayedOnce = false; 
        gunshotSoundClip = gunshotSound.clip;
        lineRenderer = GetComponent<LineRenderer>();
        //decative the line renderer by default
        lineRenderer.enabled = false;
        nextFireReady = true;
    }

    void Update()
    {   SetCountText(timeRemaining);

        if (Time.time < startInstructionTime)
        {
            startInstructions.SetActive(true);
            // startInstructions.transform.position = transform.position + Vector3.forward * 1.2f;
        } else
        {
            startInstructions.SetActive(false);

            if (timeRemaining > 0 && bulletCount > 0)
            {

                float triggerVal = GetTriggerPress();
                if (triggerVal > 0 && nextFireReady)
                {
                    rayCast();
                    // nextTimeToFire = Time.time + 1f / fireRate; 
                    nextFireReady = false;
                    if (!soundPlayedOnce)
                        {
                            soundPlayedOnce = true;
                            gunshotSound.Play();
                            VibrationManager.singleton.TriggerVibration(gunshotSoundClip, OVRInput.Controller.LTouch);
                            bulletCount -= 1; 
                            
                        }

                }
                else if (triggerVal == 0 && !nextFireReady)
                {
                    hitOnce = false;
                    soundPlayedOnce = false;
                    lineRenderer.enabled = false;
                    nextFireReady = true;

                }

                timeRemaining -= Time.deltaTime;
                leftGunScoreText.rectTransform.position = gun.position + Vector3.back * 0.12f + Vector3.up * 0.05f;
                SetCountText(timeRemaining);
            }
            else if (bulletCount == 0 || playerHealth == 0)
            {
                gameOver.SetActive(true);
                timerTextGameObject.SetActive(false);
                endGame();

            }
            else
            {
                timerTextGameObject.SetActive(false);
                timeUp.SetActive(true);
                endGame();

            }

        }

    if (enemy != null)
    {
        if (enemy.GetComponent<Enemy>().isHit)
        {
            // wait for 1 second and toggle the isHit variable back to false
            StartCoroutine(WaitAndToggle(1.15f));

        }
    }
    

        
    }


   
    void SetCountText(float timeToDisplay)
    {
        timeToDisplay += 1; 
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = "Timer: " + string.Format("{0:00}:{1:00}", minutes, seconds) + " Health: " + playerHealth.ToString();
        leftGunScoreText.text = bulletCount.ToString();
    }

    // private void ApplyForce(Rigidbody body, Vector3 endPosition)
    // {
    //     Vector3 direction = endPosition - transform.position;
    //     float distance = direction.magnitude;
    //     float force = 500f;
    //     float forceMagnitude = force * distance;
    //     Vector3 forceVector = direction.normalized * forceMagnitude;
    //     body.AddForce(forceVector, ForceMode.Force);
    // }

    private void rayCast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))

        {   
            lineRenderer.enabled = true;
            selectedObject = hit.collider.gameObject;
            endPosition = hit.point;
            lineRenderer.SetPosition(0, gun.position);
            lineRenderer.SetPosition(1, endPosition);

           
            if (selectedObject.GetComponent<Enemy>() && !hitOnce)

            {   enemy = selectedObject;
                // GameObject bulletObject = (GameObject)Instantiate(bullet, gun.localPosition, gun.rotation);
                // bulletObject.GetComponent<ProjectileController>().hitpoint = hit.point; 
                
                enemyHealth = selectedObject.GetComponent<Enemy>().health;
                if (enemyHealth <= 0)
                {
                    enemyHealth = 0;
                    selectedObject.GetComponent<Enemy>().isDead = true;
                }
                else
                {
                    enemyHealth = enemyHealth - 10;
                    selectedObject.GetComponent<Enemy>().isHit = !selectedObject.GetComponent<Enemy>().isHit;
                }
                selectedObject.GetComponent<Enemy>().health = enemyHealth;
                hitOnce = true;
            }
            Collider col = selectedObject.GetComponent<Collider>(); 
            if (col.gameObject.CompareTag("Enemy"))
            {
                GameObject newBlood = Instantiate(blood, hit.point, this.transform.rotation);
            }
            else
            {
                Instantiate(dirt, hit.point, this.transform.rotation);
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
        
    }

    
    IEnumerator WaitAndToggle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        selectedObject.GetComponent<Enemy>().isHit = false;
    }

    float GetTriggerPress() 
    {
        return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

    }

    public void playerHit()
    {
        playerHealth -= 10; 
    }

    void endGame()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in gameObjectArray)
        {
            go.SetActive(false);
        }
    }

}

