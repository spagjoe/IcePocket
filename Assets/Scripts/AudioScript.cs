using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfxDink, sfxSplash, sfxBonus, sfxShoot, sfxBomb;
    // Start is called before the first frame update
    
    public void playDink() {
        src.PlayOneShot(sfxDink);
    }

    public void playSplash() {
        src.PlayOneShot(sfxSplash);
    }

    public void playBonus() {
        src.PlayOneShot(sfxBonus);
    }

    public void playShoot() {
        src.PlayOneShot(sfxShoot);
    }

    public void playBomb() {
        src.PlayOneShot(sfxBomb);
    }
}
