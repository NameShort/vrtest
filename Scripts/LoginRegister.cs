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

    // 인증을 관리할 객체
    public static FirebaseAuth auth;
    public static FirebaseUser user;
    public static string uid;


    void Awake()
    {
        // 객체 초기화
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        Infotext.text = "로그인";
    }
    public void login()
    {
        // 제공되는 함수 : 이메일과 비밀번호로 로그인 시켜 줌
        auth.SignInWithEmailAndPasswordAsync(email_loginField.text, pass_loginField.text).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(email_loginField.text + " 로 로그인 하셨습니다.");
                    //씬 이동
                    if (user != null)
                    {
                        uid = user.UserId;
                        Debug.Log("scene1 uid: " + uid);
                    }
                    isLogin = true;
                }
                else
                {
                    Debug.Log("로그인에 실패하셨습니다.");
                }
            }
        );
    }
    public void register()
    {
        // 제공되는 함수 : 이메일과 비밀번호로 회원가입 시켜 줌
        auth.CreateUserWithEmailAndPasswordAsync(email_loginField.text, pass_loginField.text).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    
                    Debug.Log(email_loginField.text + "로 회원가입\n");
                    user = task.Result;
                    uid = user.UserId;
                    wirteNewUser(uid);
                }
                else
                {
                    Debug.Log("회원가입 실패");
                    //회원가입 실패 문구
                    isResist=false;
                }
            }
            );
    }

    private void Update()
    {
        if (!isResist)
        {
            Infotext.text = "회원가입 실패";
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
