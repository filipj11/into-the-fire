using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindUI : MonoBehaviour
{
    public static RebindUI Instance { get; private set; }

    private const string REBIND_TEXT = "Waiting for input...";

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button backButton;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    private void Awake()
    {
        Instance = this;
        Hide();

        backButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
            Hide();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            Rebind(GameInput.Binding.MoveUp, moveUpText);
        });

        moveDownButton.onClick.AddListener(() =>
        {
            Rebind(GameInput.Binding.MoveDown, moveDownText);
        });

        moveLeftButton.onClick.AddListener(() =>
        {
            Rebind(GameInput.Binding.MoveLeft, moveLeftText);
        });

        moveRightButton.onClick.AddListener(() => 
        {
            Rebind(GameInput.Binding.MoveRight, moveRightText);
        });

        interactButton.onClick.AddListener(() =>
        {
            Rebind(GameInput.Binding.Interact, interactText);
        });

        interactAltButton.onClick.AddListener(() =>
        {
            Rebind(GameInput.Binding.InteractAlt, interactAltText);
        });

        pauseButton.onClick.AddListener(() =>
        {
            Rebind(GameInput.Binding.Pause, pauseText);
        });
    }

    private void UpdateVisual()
    {
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Rebind(GameInput.Binding binding, TextMeshProUGUI buttonPressedText)
    {
        buttonPressedText.text = REBIND_TEXT;
        GameInput.Instance.Rebind(binding, UpdateVisual);
    }
}
