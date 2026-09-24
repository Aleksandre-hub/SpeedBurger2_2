using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundBTN2 : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Sound", 0) == 0) {
            gameObject.GetComponent<Image>().sprite = Off;
        }else if (PlayerPrefs.GetInt("Sound", 0) == 1) {
            gameObject.GetComponent<Image>().sprite = On;
        }
    }

    public void ClickedOnSound() {
        if (PlayerPrefs.GetInt("Sound", 0) == 0) {
            PlayerPrefs.SetInt("Sound", 1);
            gameObject.GetComponent<Image>().sprite = Off;
        }else if (PlayerPrefs.GetInt("Sound", 0) == 1) {
            PlayerPrefs.SetInt("Sound", 0);
            gameObject.GetComponent<Image>().sprite = On;
        }
    }
}
