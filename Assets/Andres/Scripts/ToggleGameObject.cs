using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    public void Toggle(){
        gameObject.SetActive(!gameObject.activeSelf);
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
