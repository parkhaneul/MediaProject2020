using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    //private Dictionary<int,Image> _borderModels = new Dictionary<int,Image>(); 
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
            _instance = this;
    }

    public void setActive(bool Value)
    {
        if(UIBorderModel.active != Value)
            UIBorderModel.SetActive(Value);
    }

    public void test(Vector3 position)
    {
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
        
        UIBorderModel.transform.position = new Vector3(30 + x * position.x,30 + y * position.y, 0);
    }

    public Vector3 multiple(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    /*
    public void setBorderPosition(int index, int degree)
    {
        var model = _borderModels[index];

        model.transform.position = new Vector3(0,0,0);
    }*/
}
