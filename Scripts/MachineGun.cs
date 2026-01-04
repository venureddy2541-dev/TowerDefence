using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class MachineGun : Weapon
{
    bool triggered = false;

    void OnEnable()
    {
        triggered = false;
    }

    protected override void PlayAudio()
    {
        if(!triggered)
        {
            StartCoroutine(fireRateBasedAudio());
        }
    }

    protected override void StopAudio()
    {
        StopCoroutine(fireRateBasedAudio());
        triggered = false;
    }

    IEnumerator fireRateBasedAudio()
    {
        triggered = true;
        while(enemyDitected && this.gameObject.activeSelf)
        {
            audioSource.Play();
            yield return new WaitForSeconds(bulletFireRate);
        }
    }
}
