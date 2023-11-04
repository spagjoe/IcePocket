using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour {
    public GameObject bonusUL;
    public GameObject bonusUR;
    public GameObject bonusLL;
    public GameObject bonusLR;
    private GameObject activeZone;

    public float activeTime;
    public float swapInterval;
    public float swapTimer;

    // Start is called before the first frame update
    void Start() {
        activeZone = null;
    }

    // Update is called once per frame
    void Update() {
        if (activeZone is not null && activeZone.activeInHierarchy && swapTimer >= activeTime) {
            deactivateZone();
        }
        else if (swapTimer >= swapInterval) {
            activateZone(getRandomZone());
            swapTimer = 0;
        }
        else {
            swapTimer += Time.deltaTime;
        }
    }
    //activeZone remains populated after deactivating for randomization purposes
    private void deactivateZone() {
        activeZone.SetActive(false);
    }

    private void activateZone(GameObject zone) {
        zone.SetActive(true);
        activeZone = zone;
    }

    public GameObject getRandomZone() {
        int temp;
        GameObject newZone = null;

        do {
            temp = Random.Range(0, 4);
            Debug.Log("Activate zone " + temp);
            switch (temp) {
                case 0:
                    newZone = bonusUL;
                    break;
                case 1:
                    newZone = bonusUR;
                    break;
                case 2:
                    newZone = bonusLL;
                    break;
                case 3:
                    newZone = bonusLR;
                    break;
            }
        } while (activeZone is not null && activeZone.Equals(newZone));

        return newZone;
    }
}
