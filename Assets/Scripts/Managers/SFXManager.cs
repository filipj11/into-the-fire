using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SFX_VOLUME = "SFXVolume";

    public static SFXManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipReferences;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.Instance.OnDeliveryFail += DeliveryManager_OnDeliveryFail;
        CuttingCounter.OnCutAny += CuttingCounter_OnCutAny;
        Player.Instance.OnPickup += Player_OnPickup;
        BaseCounter.OnObjectPlace += BaseCounter_OnObjectPlace;
        TrashCan.OnThrowAway += TrashCan_OnThrowAway;
    }

    private void TrashCan_OnThrowAway(object sender, EventArgs e)
    {
        TrashCan trashCan = sender as TrashCan;
        PlaySFX(audioClipReferences.trash, trashCan.transform.position);
    }

    private void BaseCounter_OnObjectPlace(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySFX(audioClipReferences.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickup(object sender, EventArgs e)
    {
        PlaySFX(audioClipReferences.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnCutAny(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySFX(audioClipReferences.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnDeliveryFail(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySFX(audioClipReferences.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySFX(audioClipReferences.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySFX(AudioClip[] audioClips, Vector3 pos, float volumeMult = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClips[UnityEngine.Random.Range(0, audioClips.Length)], pos, volumeMult * volume);
    }

    public void PlayCountdownSFX()
    {
        PlaySFX(audioClipReferences.warning, Vector3.zero);
    }

    public void PlayWarningSFX(Vector3 pos)
    {
        PlaySFX(audioClipReferences.warning, pos);
    }

    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
