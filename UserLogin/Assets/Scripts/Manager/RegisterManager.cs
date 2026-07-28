using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    public static RegisterManager Instance { get; private set; }
    public TMP_InputField userInput;
    public TMP_InputField passwordInput;
    public TMP_InputField phoneNumberInput;
    public Button manButton;
    public Button womanButton;

    private string selectedGender = "未知";

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
        manButton.onClick.AddListener(() => SelectGender(true));
        womanButton.onClick.AddListener(() => SelectGender(false));
    }

    public void Register()
    {
        string username = userInput.text.Trim();
        string password = passwordInput.text.Trim();
        string phoneNumber = phoneNumberInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            InterfaceManager.Instance.SetTipText("用户名不能为空。");
            return;
        }
        else if (!IsUsernameValid(username))
        {
            InterfaceManager.Instance.SetTipText("用户名需为1-20位中文、英文或数字，且不能是纯数字。");
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            InterfaceManager.Instance.SetTipText("密码不能为空。");
            return;
        }
        else if (!IsPasswordValid(password))
        {
            InterfaceManager.Instance.SetTipText("密码需为6-15位英文或数字。");
            return;
        }
        if (string.IsNullOrEmpty(phoneNumber))
        {
            InterfaceManager.Instance.SetTipText("手机号不能为空。");
            return;
        }
        else if (!IsPhoneValid(phoneNumber))
        {
            InterfaceManager.Instance.SetTipText("手机号格式不合法。");
            return;
        }
        if (selectedGender == "未知")
        {
            InterfaceManager.Instance.SetTipText("请选择性别。");
            return;
        }

        LoginController.Instance.SendRegisterRequest(username, password, phoneNumber, selectedGender);
    }

    public void OnRegisterSuccess()
    {
        InterfaceManager.Instance.SetInterface(false);
    }

    public void OnRegisterFailed(string message)
    {
        InterfaceManager.Instance.SetTipText(message);
    }

    public void Clear()
    {
        userInput.text = "";
        passwordInput.text = "";
        phoneNumberInput.text = "";
        selectedGender = "未知";
    }

    private bool IsUsernameValid(string username)
    {
        return Regex.IsMatch(username, "^[A-Za-z0-9\u4e00-\u9fa5]{1,20}$") && !Regex.IsMatch(username, "^\\d+$");
    }

    private bool IsPasswordValid(string password)
    {
        return Regex.IsMatch(password, "^[A-Za-z0-9]{6,15}$");
    }

    private bool IsPhoneValid(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, "^1[3-9]\\d{9}$");
    }

    private void SelectGender(bool isMan)
    {
        if (selectedGender == "未知" || isMan != (selectedGender == "男"))
        {
            manButton.transform.Find("checkmark").GetComponent<Image>().enabled = isMan;
            womanButton.transform.Find("checkmark").GetComponent<Image>().enabled = !isMan;
            selectedGender = isMan ? "男" : "女";
        }
    }
}