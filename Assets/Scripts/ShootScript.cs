using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject icicle;
    public Transform icicleTransform;
    public bool canFire;
    private float fireTimer;
    public float fireCooldown;
    public float chargeTime;
    private float currCharge;
    public FreezeRadiusScript freezeRadius;
    public GameObject audioPlayer;
    public AudioScript audioSource;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        audioSource = audioPlayer.GetComponent<AudioScript>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float zRot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRot);

        if (!canFire)
        {
            fireTimer += Time.deltaTime;
            if(fireTimer > fireCooldown)
            {
                canFire = true;
                fireTimer = 0;
            }
        }
        //charged shot
        if(canFire && Input.GetMouseButton(1))
        {
            stopFreeze();
            if (currCharge >= chargeTime)
            {
                canFire = false;
                currCharge = 0;
                Instantiate(icicle, icicleTransform.position, Quaternion.identity);
                audioSource.playShoot();
            }
            else
            {
                //TODO: charge sfx/visuals
                currCharge += Time.deltaTime;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            startFreeze();
        }
        if (Input.GetMouseButtonUp(0))
        {
            stopFreeze();
        }
        if (Input.GetMouseButtonUp(1))
        {
            currCharge = 0;
        }
        //instant fire
        /*if (Input.GetMouseButton(1) && canFire)
        {
            canFire = false;
            Instantiate(icicle, icicleTransform.position, Quaternion.identity);
        }*/
    }
    public void startFreeze()
    {
        freezeRadius.gameObject.SetActive(true);
    }
    public void stopFreeze()
    {
        freezeRadius.gameObject.SetActive(false);
    }
}
