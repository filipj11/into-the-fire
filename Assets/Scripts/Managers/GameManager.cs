using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnpause;

    private enum State
    {
        BeforeStart,
        Countdown,
        Playing,
        Finish
    }

    private State state;
    private float beforeStartTimer = 1f;
    private float countdownTimer = 3f;
    private float playingTimer;
    private float playingTimerMax = 100f;
    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
        state = State.BeforeStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.BeforeStart:
                beforeStartTimer -= Time.deltaTime;
                if (beforeStartTimer < 0f)
                {
                    state = State.Countdown;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Countdown:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer < 0f)
                {
                    state = State.Playing;
                    playingTimer = playingTimerMax;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                playingTimer -= Time.deltaTime;
                if (playingTimer < 0f)
                {
                    state = State.Finish;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Finish:
                break;
        }
        Debug.Log(state);
    }

    public bool IsPlaying()
    {
        return state == State.Playing;
    }

    public bool IsCountdown()
    {
        return state == State.Countdown;
    }

    public bool IsFinish()
    {
        return state == State.Finish;
    }

    public float GetCountdownTimer()
    {
        return countdownTimer;
    }

    public float GetNormalizedPlayingTimer()
    {
        return 1 - (playingTimer / playingTimerMax);
    }

    public void TogglePauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpause?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
