using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }
    public TMP_Dropdown accountSelection;
    public TMP_InputField userInput;
    public TMP_InputField passwordInput;
    public Toggle rememberAccountToggle;

    private readonly Dictionary<string, string> rememberedAccounts = new Dictionary<string, string>();

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
        LoadRememberedAccounts();
        accountSelection.onValueChanged.AddListener(OnAccountSelected);
    }

    public void Login()
    {
        string usernameOrPhone = userInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(usernameOrPhone))
        {
            InterfaceManager.Instance.SetTipText("账号不能为空。");
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            InterfaceManager.Instance.SetTipText("密码不能为空。");
            return;
        }

        LoginController.Instance.SendLoginRequest(usernameOrPhone, password);
    }

    public void OnLoginSuccess(string username, string password)
    {
        if (rememberAccountToggle.isOn)
        {
            SaveRememberedAccount(username, password);
        }
        SceneManager.LoadScene("UserScene");
    }

    public void OnLoginFailed(string message)
    {
        InterfaceManager.Instance.SetTipText(message);
    }

    public void Clear()
    {
        userInput.text = "";
        passwordInput.text = "";
    }

    private void SaveRememberedAccount(string account, string password)
    {
        if (rememberedAccounts.ContainsKey(account))
        {
            rememberedAccounts[account] = password;
        }
        else
        {
            rememberedAccounts.Add(account, password);
        }

        List<string> accountPasswordPairs = new List<string>();
        foreach (var kvp in rememberedAccounts)
        {
            accountPasswordPairs.Add($"{kvp.Key}||{kvp.Value}");
        }
        PlayerPrefs.SetString("RememberedAccounts", string.Join("\n", accountPasswordPairs));
        PlayerPrefs.Save();
    }

    private void LoadRememberedAccounts()
    {
        rememberedAccounts.Clear();
        string saved = PlayerPrefs.GetString("RememberedAccounts", string.Empty);
        if (!string.IsNullOrEmpty(saved))
        {
            string[] pairs = saved.Split('\n');
            foreach (string pair in pairs)
            {
                string[] accountPassword = pair.Split(new string[] { "||" }, System.StringSplitOptions.None);
                if (accountPassword.Length == 2 && !string.IsNullOrEmpty(accountPassword[0]))
                {
                    string account = accountPassword[0];
                    string password = accountPassword[1];
                    if (rememberedAccounts.ContainsKey(account))
                    {
                        rememberedAccounts[account] = password;
                    }
                    else
                    {
                        rememberedAccounts.Add(account, password);
                    }
                }
            }
        }

        accountSelection.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (var kvp in rememberedAccounts)
        {
            if (!string.IsNullOrEmpty(kvp.Key))
            {
                options.Add(new TMP_Dropdown.OptionData(kvp.Key));
            }
        }
        accountSelection.AddOptions(options);
    }

    public void OnAccountSelected(int index)
    {
        if (accountSelection != null && index >= 0 && index < accountSelection.options.Count)
        {
            string selectedAccount = accountSelection.options[index].text;
            if (rememberedAccounts.TryGetValue(selectedAccount, out string password))
            {
                userInput.text = selectedAccount;
                passwordInput.text = password;
            }
        }
    }
}