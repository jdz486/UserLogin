using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI phoneNumText;
    public Image genderImage;
    public Button memberButton;
    public Button exitButton;
    public GameObject confirmWindowPrefab;
    public Transform canvasTransform;

    private bool isMember;
    private bool pendingIsMember;

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
        RefreshUserInfo();
        exitButton.onClick.AddListener(() => ShowConfirmWindow("确定要退出登录吗？", ExitLogin));
    }

    private void RefreshUserInfo()
    {
        nameText.text = PlayerPrefs.GetString("CurrentUsername");
        string phoneNum = PlayerPrefs.GetString("CurrentPhoneNumber");
        phoneNumText.text = phoneNum.Substring(0, 3) + "****" + phoneNum.Substring(7);

        string gender = PlayerPrefs.GetString("CurrentGender");
        if (gender == "男")
        {
            genderImage.sprite = Resources.Load<Sprite>("Images/男");
        }
        else if (gender == "女")
        {
            genderImage.sprite = Resources.Load<Sprite>("Images/女");
        }
        
        isMember = PlayerPrefs.GetInt("CurrentIsMember") == 1;
        memberButton.onClick.RemoveAllListeners();
        if (isMember)
        {
            memberButton.GetComponentInChildren<TextMeshProUGUI>().text = "退出会员";
            memberButton.onClick.AddListener(() => ShowConfirmWindow("确定要退出会员吗？", () => ChangeMember(false)));
        }
        else
        {
            memberButton.GetComponentInChildren<TextMeshProUGUI>().text = "成为会员";
            memberButton.onClick.AddListener(() => ShowConfirmWindow("确定要成为会员吗？", () => ChangeMember(true)));
        }
    }

    private void ShowConfirmWindow(string message, System.Action onConfirm)
    {
        if (confirmWindowPrefab == null)
        {
            onConfirm?.Invoke();
            return;
        }

        GameObject windowInstance = Instantiate(confirmWindowPrefab, canvasTransform);
        windowInstance.GetComponent<ConfirmWindow>().Init(message, onConfirm);
    }

    private void ChangeMember(bool isMember)
    {
        int accountId = int.Parse(PlayerPrefs.GetString("CurrentUserId"));
        SendChangeMemberRequest(accountId, isMember);
    }

    private void SendChangeMemberRequest(int accountId, bool newIsMember)
    {
        pendingIsMember = newIsMember;
        LoginController.Instance?.SendChangeMemberRequest(accountId, newIsMember);
    }

    public void OnChangeMemberSuccess()
    {
        isMember = pendingIsMember;
        PlayerPrefs.SetInt("CurrentIsMember", pendingIsMember ? 1 : 0);
        PlayerPrefs.Save();
        RefreshUserInfo();
    }

    public void OnChangeMemberFailed(string errorMessage)
    {
        ShowConfirmWindow("会员状态变更失败：" + errorMessage, null);
    }

    private void ExitLogin()
    {
        PlayerPrefs.DeleteKey("CurrentUserId");
        PlayerPrefs.DeleteKey("CurrentUsername");
        PlayerPrefs.DeleteKey("CurrentPhoneNumber");
        PlayerPrefs.DeleteKey("CurrentGender");
        PlayerPrefs.DeleteKey("CurrentIsMember");
        SceneManager.LoadScene("LoginScene");
    }
}