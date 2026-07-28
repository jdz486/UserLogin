using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance { get; private set;}
    public GameObject LoginInterface;
    public GameObject RegisterInterface;
    public TextMeshProUGUI tipText;
    public Button mainButton;
    public Button registerButton;
    public Button backButton;

    private bool isRegister;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetInterface(false);
        mainButton.onClick.AddListener(OnMainButtonClick);
        registerButton.onClick.AddListener(() => SetInterface(true));
        backButton.onClick.AddListener(() => SetInterface(false));
    }

    public void SetTipText(string text)
    {
        tipText.text = text;
    }

    private void OnMainButtonClick()
    {
        if (isRegister)
        {
            RegisterManager.Instance.Register();
        }
        else
        {
            LoginManager.Instance.Login();
        }
    }

    public void SetInterface(bool isRegister)
    {
        LoginInterface.SetActive(!isRegister);
        RegisterInterface.SetActive(isRegister);
        mainButton.GetComponentInChildren<TextMeshProUGUI>().text = isRegister ? "注册" : "登录";
        this.isRegister = isRegister;
        if (isRegister)
        {
            RegisterManager.Instance.Clear();
        }
        else
        {
            LoginManager.Instance.Clear();
        }
        SetTipText("");
    }
}