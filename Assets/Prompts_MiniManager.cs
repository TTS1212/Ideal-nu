using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompts_MiniManager : MonoBehaviour
{
    public GameObject Gamepad_, Keyboard_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.device == "Gamepad")
        {
            Gamepad_.SetActive(true);
            Keyboard_.SetActive(false);
        }
        else if (PlayerMovement.device == "Keyboard")
        {
            Gamepad_.SetActive(false);
            Keyboard_.SetActive(true);
        }
    }
}
