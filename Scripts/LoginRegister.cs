using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LoginRegister : MonoBehaviour
{
    [SerializeField] InputField email_loginField;
    [SerializeField] InputField pass_loginField;
    [SerializeField] Text Infotext;

    bool isResist=true;
    bool isLogin = false;

    // ������ ������ ��ü
    public static FirebaseAuth auth;
    public static FirebaseUser user;
    public static string uid;


    void Awake()
    {
        // ��ü �ʱ�ȭ
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        Infotext.text = "�α���";
    }
    public void login()
    {
        // �����Ǵ� �Լ� : �̸��ϰ� ��й�ȣ�� �α��� ���� ��
        auth.SignInWithEmailAndPasswordAsync(email_loginField.text, pass_loginField.text).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(email_loginField.text + " �� �α��� �ϼ̽��ϴ�.");
                    //�� �̵�
                    if (user != null)
                    {
                        uid = user.UserId;
                        Debug.Log("scene1 uid: " + uid);
                    }
                    isLogin = true;
                }
                else
                {
                    Debug.Log("�α��ο� �����ϼ̽��ϴ�.");
                }
            }
        );
    }
    public void register()
    {
        // �����Ǵ� �Լ� : �̸��ϰ� ��й�ȣ�� ȸ������ ���� ��
        auth.CreateUserWithEmailAndPasswordAsync(email_loginField.text, pass_loginField.text).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    
                    Debug.Log(email_loginField.text + "�� ȸ������\n");
                    user = task.Result;
                    uid = user.UserId;
                    wirteNewUser(uid);
                }
                else
                {
                    Debug.Log("ȸ������ ����");
                    //ȸ������ ���� ����
                    isResist=false;
                }
            }
            );
    }

    private void Update()
    {
        if (!isResist)
        {
            Infotext.text = "ȸ������ ����";
        }

        if (isLogin)
        {
            SceneManager.LoadScene("NextScene");
        }
    }

    public class User
    {
        public Vector3 houseV3;
        public string user_avt;
        public User()
        {
            this.user_avt = "default";
            this.houseV3 = new Vector3(0,0,0);
        }
    }

    DatabaseReference reference;

    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
    public void wirteNewUser(string UID)
    {
        User user = new User();
        Debug.Log(" SaveDatabase");
        string json = JsonUtility.ToJson(user);

        reference.Child("user").Child(UID).SetRawJsonValueAsync(json);
    }

}
