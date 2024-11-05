using System;
using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class GetLocation : MonoBehaviour
{
	public LocationInfo Info;
	public float latitude;
	public float longitude;
	public WeatherData weatherData;
	private string IPAddress;


    public float GetLatitude()
    {
        return latitude;
    }
    public float GetLongitude()
    {
        return longitude;
    }

    void Start() {
		StartCoroutine(GetIP());
	}

	private IEnumerator GetIP()
	{

        Debug.Log("IP: Creating Request...");

        UnityWebRequest www = new UnityWebRequest("https://api.ipify.org?format=json");
		www.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("IP: Requesting...");

        yield return www.SendWebRequest();

        Debug.Log("IP: Response Achieved...");

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            yield break;
        }

        IPResponse ipResponse = JsonUtility.FromJson<IPResponse>(www.downloadHandler.text);

        // Display the IP address in the console
        Debug.Log("Public IP Address: " + ipResponse.ip);

        IPAddress = ipResponse.ip;
		StartCoroutine (GetCoordinates());
	}

	private IEnumerator GetCoordinates()
	{

        Debug.Log("COORDS: Creating Request...");

        UnityWebRequest www = new UnityWebRequest("http://ip-api.com/json/" + IPAddress);
        www.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("COORDS: Requesting...");

        yield return www.SendWebRequest();

        Debug.Log("COORDS: Response Achieved...");

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            yield break;
        }

        Info = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
		latitude = Info.lat;
		longitude = Info.lon;
		GetComponent<WeatherData>().Begin();
	}
}

public class IPResponse
{
    public string ip;
}

[Serializable]
public class LocationInfo
{
	public string status;
	public string country;
	public string countryCode;
	public string region;
	public string regionName;
	public string city;
	public string zip;
	public float lat;
	public float lon;
	public string timezone;
	public string isp;
	public string org;
	public string @as;
	public string query;
}


