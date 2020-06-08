using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject UIBorderModel;
    public Text MissionUI;
    private Dictionary<int,GameObject> _borderModels = new Dictionary<int,GameObject>(); 
    
    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void Start()
    {
        setText();
    }

    public void addUser(int index)
    {
        var newModel = GameObject.Instantiate(UIBorderModel,this.transform);
        _borderModels.Add(index,newModel);
    }

    public void test(int index,Vector3 position)
    {
        var model = _borderModels[index];

        if (model == null)
            return;
        
        var x = Screen.width - UIBorderModel.GetComponent<RectTransform>().rect.width;
        var y = Screen.height - UIBorderModel.GetComponent<RectTransform>().rect.height;
        
        if (position.x > 1)
            position.x = 1;

        if (position.x < 0)
            position.x = 0;
        
        if (position.y > 1)
            position.y = 1;

        if (position.y < 0)
            position.y = 0;

        var uiPosition_x = 30 + x * position.x;
        var uiPosition_y = 30 + y * position.y;
        model.transform.position = new Vector3(30 + x * position.x,30 + y * position.y, 0);

        var angle = AngleTo(new Vector2(uiPosition_x, uiPosition_y),new Vector2(Screen.width / 2, Screen.height / 2));
        
        model.transform.eulerAngles = new Vector3(0,0,270 + angle);
        
        model.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, - model.transform.eulerAngles.z);
    }

    public void setUI(int uid, RenderTexture rt)
    {
        if (_borderModels[uid] == null)
            return;

        var ri = _borderModels[uid].GetComponentInChildren<RawImage>();
        ri.texture = rt;
    }

    public void setActive(int uid, bool value)
    {
        if (_borderModels[uid] == null)
            return;
        
        if(_borderModels[uid].active != value)
            _borderModels[uid].SetActive(value);
    }

    public void setText()
    {
        string UIText = "";
        
        var dics = MissionLogic.Instance.requiredItems();

        foreach (var item in dics)
        {
            switch (item.Key)
            {
                case ItemKind.Branch :
                    UIText += " [나뭇가지 아이템]이 ";
                    break;
                case ItemKind.Mountain :
                    UIText += " [산 아이템]이 ";
                    break;
                case ItemKind.Rock :
                    UIText += " [바위 아이템]이 ";
                    break;
                case ItemKind.Stone :
                    UIText += " [돌 아이템]이 ";
                    break;
                case ItemKind.Tree :
                    UIText += " [나무 아이템]이 ";
                    break;
            }
            UIText += item.Value + "개,";
        }

        UIText = UIText.Substring(0, UIText.Length - 1);
        UIText += " 필요합니다.";
        MissionUI.text = UIText;
    }

    private float AngleTo(Vector2 pos, Vector2 target)
    {
        Vector2 difference = target - pos;
        float sign = (target.y < pos.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, difference) * sign;
    }

    public Vector3 multiple(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
