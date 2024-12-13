using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public GameObject playerController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDyingAnimationComplete()
    {
        playerController.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
