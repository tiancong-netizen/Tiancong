using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPenel : MonoBehaviour
{

    private InputField input_UserName;
    private InputField input_Password;
    private Button btn_Login;
    private Button btn_Register;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowLoginPanel, Show);
        Init();
        gameObject.SetActive(true);
    }
    private void Init()
    {
        input_UserName = transform.Find("Input_Username").GetComponent<InputField>();
        input_Password = transform.Find("Input_Password").GetComponent<InputField>();
        btn_Login = transform.Find("btn_Login").GetComponent<Button>();
        btn_Login.onClick.AddListener(OnLoginButtonClick);
        btn_Register = transform.Find("btn_Register").GetComponent<Button>();
        btn_Register.onClick.AddListener(OnRegisterButtonClick);
    }
 
    private void OnDestroy()
    {
        EventCenter.AddListener(EventDefine.ShowRegisterPanel, Show);
    }
    /// <summary>
    /// 注册点击按钮
    /// </summary>
    private void OnRegisterButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowRegisterPanel);
    }
    /// <summary>
    /// 登陆按钮点击
    /// </summary>
    private void OnLoginButtonClick()
    {
        if (input_UserName.text == null || input_UserName.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入用户名");
            //Debug.Log("请输入用户名");
            return;
        }
        if (input_Password.text == null || input_Password.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入密码");
            //Debug.Log("请输入密码");
            return;
        }
     
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}