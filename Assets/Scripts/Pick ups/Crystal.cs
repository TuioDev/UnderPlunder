using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IPickable
{
    [SerializeField] private AudioClip PickUpSound;
    public void PickUpOnContact()
    {
        Player.Instance.PickingCrystal();
        //Do some flashy effect
        AudioManager.Instance.PlayOneClip(PickUpSound);
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
