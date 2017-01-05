using UnityEngine;
using System.Collections;

public class GoogleMap : Singleton<GoogleMap>
{
    public enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }
    public string keyMaps;
    public bool loadOnStart = true;
    public bool autoLocateCenter = true;
    public GoogleMapLocation centerLocation;
    public float zoom = 13;
    public MapType mapType;
    public int size = 512;
    public bool doubleResolution = false;
    public GoogleMapMarker[] markers;
    public GoogleMapPath[] paths;
    float tempZoom = 0;
    public bool GetMyLocation;
    private Position myPos;
    void Start()
    {
        myPos = new Position();
        tempZoom = zoom;
        if (loadOnStart) Refresh();
        if (GetMyLocation) {
            StartCoroutine(myPos.StartingLoc());
        }
    }
    void Update() {
        if (tempZoom != zoom) {
            Refresh();
            tempZoom = zoom;
        }
    }
    public void Refresh()
    {
        if (autoLocateCenter && (markers.Length == 0 && paths.Length == 0))
        {
            Debug.LogError("Auto Center will only work if paths or markers are used.");
        }
        StartCoroutine(_Refresh());
    }
    public void SetZoom(float zooms) {
        if (zoom < 18) {
            zoom -= zooms;
            Refresh();
        }
        
    }
    public float speed;
    public void SetPos(float lat,float longt) {
        centerLocation.latitude = lat;
        centerLocation.longitude = longt;
        Refresh();
    }
    public void Move(Vector2 v) {
         
        centerLocation.latitude -= (v.y/speed);
        centerLocation.longitude -= (v.x/speed);
        Refresh();
    }
    IEnumerator _Refresh()
    {
        var url = "http://maps.googleapis.com/maps/api/staticmap";
        var qs = "";
        if (!autoLocateCenter)
        {
            if (centerLocation.address != "")
                qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
            else
            {
                qs += "center=" + WWW.UnEscapeURL(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));
            }

            qs += "&zoom=" + zoom.ToString();
        }
        qs += "&size=" + WWW.UnEscapeURL(string.Format("{0}x{0}", size));
        qs += "&scale=" + (doubleResolution ? "2" : "1");
        qs += "&maptype=" + mapType.ToString().ToLower();
        var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
        qs += "&sensor=" + (usingSensor ? "true" : "false");

        foreach (var i in markers)
        {
            qs += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", i.size.ToString().ToLower(), i.color, i.label);
            foreach (var loc in i.locations)
            {
                if (loc.address != "")
                    qs += "|" + WWW.UnEscapeURL(loc.address);
                else
                    qs += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
            }
        }

        foreach (var i in paths)
        {
            qs += "&path=" + string.Format("weight:{0}|color:{1}", i.weight, i.color);
            if (i.fill) qs += "|fillcolor:" + i.fillColor;
            foreach (var loc in i.locations)
            {
                if (loc.address != "")
                    qs += "|" + WWW.UnEscapeURL(loc.address);
                else
                    qs += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
            }
        }
 
         
        var req = new WWW(url + "?" + qs+ "&key=" + keyMaps);
        
       
        yield return req;
        Debug.Log(req.texture);
        GetComponent<Renderer>().material.mainTexture = req.texture;
    }


}

public enum GoogleMapColor
{
    black,
    brown,
    green,
    purple,
    yellow,
    blue,
    gray,
    orange,
    red,
    white
}

[System.Serializable]
public class GoogleMapLocation
{
    public string address;
    public float latitude;
    public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
    public enum GoogleMapMarkerSize
    {
        Tiny,
        Small,
        Mid
    }
    public GoogleMapMarkerSize size;
    public GoogleMapColor color;
    public string label;
    public GoogleMapLocation[] locations;

}

[System.Serializable]
public class GoogleMapPath
{
    public int weight = 5;
    public GoogleMapColor color;
    public bool fill = false;
    public GoogleMapColor fillColor;
    public GoogleMapLocation[] locations;
}