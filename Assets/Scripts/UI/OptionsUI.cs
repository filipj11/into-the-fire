using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button rebindButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private TextMeshProUGUI musicText;

    private void Awake()
    {
        Instance = this;

        sfxButton.onClick.AddListener(() =>
        {
            SFXManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        rebindButton.onClick.AddListener(() =>
        {
            RebindUI.Instance.Show();
            Hide();
        });

        backButton.onClick.AddListener(() =>
        {
            PauseUI.Instance.Show();
            Hide();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpause += GameManager_OnGameUnpause;
        UpdateVisual();
        Hide();
    }

    private void GameManager_OnGameUnpause(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        float sfxVolume = Mathf.Round(SFXManager.Instance.GetVolume() * 10f);
        sfxText.text = $"SFX: {sfxVolume}";

        float musicVolume = Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
        musicText.text = $"Music: {musicVolume}";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
