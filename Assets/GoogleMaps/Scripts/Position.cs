using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position  {

    void Start() {
    }
    public void GetLoc() {
      
    }
   public IEnumerator StartingLoc()
    {
       
        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("Start Loc "+(20-maxWait)*50+"%");
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
          //  print("Timed out");
         //   return false;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {

           // return false;            print("Unable to determine device location");
        }
        else
         //   print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        GoogleMap.Instance.SetPos(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Input.location.Stop();
    }
}
