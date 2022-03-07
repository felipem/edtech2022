using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        try
        {
            //todo: add the consent pieces
            List<string> consentIdentifiers = await Events.CheckForRequiredConsents();           
        }
        catch (ConsentCheckException e)
        {
            Debug.Log("Error reason = " + e.Reason.ToString());
        }
 
        Debug.Log("Done with Start!");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
