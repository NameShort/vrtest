using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Database;


public class ShowPlayerID : MonoBehaviour
{
    [SerializeField] private Text maintext;
    public GameObject cube;

    Vector3 pos;
    DatabaseReference reference;

    int sint;
    string userkey;
    string avator;
    string uid;

    private void Awake()
    {
        uid = LoginRegister.uid;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("user").Child(uid).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                sint = 1;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot data = task.Result;
                Debug.Log("uid: " + uid);
                if (data.Value != null)
                {
                    sint = 2;
                    userkey = uid;
                    pos.x = float.Parse(data.Child("houseV3").Child("x").Value.ToString());
                    pos.y = float.Parse(data.Child("houseV3").Child("y").Value.ToString());
                    pos.z = float.Parse(data.Child("houseV3").Child("z").Value.ToString());
                    avator = data.Child("user_avt").Value.ToString();
                }
            }
        });
    }

    void Update()
    {
        switch (sint)
        {
            case 1:
                maintext.text = "데이터 불러오기 실패";
                break;
            case 2:
                maintext.text = userkey + "\n Avator: " + avator + "\n Vector3: X: " + pos.x + "\n Vector3: Y: " + pos.y + "\n Vector3: Z: " + pos.z;
                break;
        }
    }

    public void onClickLoad()
    {
        cube.transform.position = pos;
    }

    public void onClickSave()
    {
        pos = cube.transform.position;
        Dictionary<string, object> Result = new Dictionary<string, object>();
        Result["/user/" + uid + "/houseV3/" + "x"] = pos.x;
        Result["/user/" + uid + "/houseV3/" + "y"] = pos.y;
        Result["/user/" + uid + "/houseV3/" + "z"] = pos.z;
        reference.UpdateChildrenAsync(Result);
    }
}
