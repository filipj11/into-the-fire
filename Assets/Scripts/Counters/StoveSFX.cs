using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSFX : MonoBehaviour
{
    [SerializeField] private Stove stove;

    private AudioSource audioSource;
    private float warningSFXTimer;
    private bool playWarningSFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stove.OnStateChange += Stove_OnStateChange;
        stove.OnProgressChange += Stove_OnProgressChange;
    }

    private void Update()
    {
        if (playWarningSFX)
        {
            warningSFXTimer -= Time.deltaTime;
            if (warningSFXTimer <= 0f)
            {
                float warningSFXTimerMax = .2f;
                warningSFXTimer = warningSFXTimerMax;

                SFXManager.Instance.PlayWarningSFX(stove.transform.position);
            }
        }
    }

    private void Stove_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
    {
        float playSFXProgressAmount = .5f;
        playWarningSFX = stove.IsCooked() && e.progressNormalized >= playSFXProgressAmount;
    }

    private void Stove_OnStateChange(object sender, Stove.OnStateChangeEventArgs e)
    {
        bool playSizzleSFX = e.state == Stove.State.Cooking || e.state == Stove.State.Cooked;

        if (playSizzleSFX)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
