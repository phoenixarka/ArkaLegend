using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindManager : MonoBehaviour {

    private static KeyBindManager instance;

    public static KeyBindManager Instance
    {
        get
        {
            if (instance == null) {
                instance = FindObjectOfType<KeyBindManager>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public Dictionary<string, KeyCode> KeyBinds { get; private set; }

    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    private string bindName;

    // Use this for initialization
    void Start () {
        KeyBinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("UP", KeyCode.W);
        BindKey("DOWN", KeyCode.S);
        BindKey("LEFT", KeyCode.A);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACTION1", KeyCode.Alpha1);
        BindKey("ACTION2", KeyCode.Alpha2);
        BindKey("ACTION3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind) {
        Dictionary<string, KeyCode> currDictionary = KeyBinds;

        if (key.Contains("ACTION")) {
            currDictionary = ActionBinds;
        }

        if (!currDictionary.ContainsKey(key))
        {
            currDictionary.Add(key, keyBind);
        }
        else if (currDictionary.ContainsValue(keyBind)) {
            // find key already signed
            string currKey = currDictionary.FirstOrDefault(x => x.Value == keyBind).Key;
            
            // un-bind original
            currDictionary[currKey] = KeyCode.None;
            UIManager.Instance.UpdateKey(key, KeyCode.None);
        }

        // sign new
        currDictionary[key] = keyBind;
        UIManager.Instance.UpdateKey(key, keyBind);
        bindName = string.Empty;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KeyBindOnClick(string bindName) {
        this.bindName = bindName;
    }

    public void OnGUI()
    {
        if (bindName != string.Empty) {
            Event e = Event.current;
            if (e.isKey) {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
