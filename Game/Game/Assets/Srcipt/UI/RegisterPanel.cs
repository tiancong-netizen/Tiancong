using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class RegisterPanel : MonoBehaviour
{
    private InputField input_UserName;
    private InputField input_Password;
    private Button btn_Ruturn;
    private Button btn_Register;
    private Button btn_display;
    /// <summary>
    /// 是否显示密码的标志位
    /// </summary>
    private bool isShowPassword = false;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRegisterPanel, Show);

        Init();
        gameObject.SetActive(false);
    }
    private void Init()
    {
        input_UserName = transform.Find("Username/input_UserName").GetComponent<InputField>();
        input_Password = transform.Find("Password/input_Password").GetComponent<InputField>();
        btn_Ruturn = transform.Find("btn_Return").GetComponent<Button>();
        btn_Ruturn.onClick.AddListener(OnBackButtonClick);
        btn_Register = transform.Find("btn_Register").GetComponent<Button>();
        btn_Register.onClick.AddListener(OnRegisterButton);
        btn_display = transform.Find("btn_display").GetComponent<Button>();

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRegisterPanel, Show);
    }
    /// <summary>
    /// 登陆按钮点击
    /// </summary>
    private void OnRegisterButton()
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
        //向服务器发送数据，注册一个用户
         //AccountDto dto = new AccountDto(input_UserName.text, input_Password.text);
         //NetMsgCenter.Instance.SendMsg(OpCode.Account,AccountCode.Register_CREQ,dto);

    }
    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnBackButtonClick()
    {
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowLoginPanel);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}