using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class WeatherData : MonoBehaviour {
	private float timer;
	public float minutesBetweenUpdate;
	public string API_key;
	private float latitude = 0.0f;
	private float longitude = 0.0f;
	private bool locationInitialized;
	private TMP_Text currentWeatherText;
	private GetLocation getLocation;


    public GameObject rain;
    public GameObject clear;
    public GameObject snow;
    public GameObject clouds;


    private void Start()
    {
        rain.SetActive(false);
        clear.SetActive(false);
        snow.SetActive(false);
        clouds.SetActive(false);
        currentWeatherText = GameObject.Find("WeatherLog").GetComponent<TMP_Text>();
        currentWeatherText.SetText("Loading...");
    }

    public void Begin() {
        getLocation = GetComponent<GetLocation>();
        latitude = getLocation.GetLatitude();
        longitude = getLocation.GetLongitude();
        locationInitialized = true;
        StartCoroutine(GetWeatherInfo());
    }


    private IEnumerator GetWeatherInfo()
	{
        string mainWeather = "Clear";
        Debug.Log("WEATHER: Creating Request...");

        UnityWebRequest www = new UnityWebRequest("https://api.openweathermap.org/data/2.5/weather?lat="+latitude+"&lon="+longitude+"&appid="+API_key);
		www.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("WEATHER: Requesting...");

        yield return www.SendWebRequest();

        Debug.Log("WEATHER: Response achived!");

        if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.error);
			yield break;
		} else
		{
            WeatherInfo Info = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
            mainWeather = Info.weather[0].main.ToString();
			currentWeatherText.text = mainWeather;
        }


        // APPLY THE WEATHER
        if (mainWeather == "Rain" || mainWeather == "Drizzle" || mainWeather == "Thunderstorm")
        {
            rain.SetActive(true);
        }
        else if (mainWeather == "Snow")
        {
            snow.SetActive(true);
        }
        else if (mainWeather == "Clear")
        {
            clear.SetActive(true);
        }
        else
        {
            clouds.SetActive(true);
        }

        yield return true;
    }
}


[Serializable]
public class Coord
{
    public float lon;
    public float lat;
}

[Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

[Serializable]
public class Main
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
    public int sea_level;
    public int grnd_level;
}

[Serializable]
public class Wind
{
    public float speed;
    public int deg;
    public float gust;
}

[Serializable]
public class Rain
{
    public float _1h; // Note: C# variable names cannot start with numbers, so use an underscore.
}

[Serializable]
public class Clouds
{
    public int all;
}

[Serializable]
public class Sys
{
    public int type;
    public int id;
    public string country;
    public int sunrise;
    public int sunset;
}

[Serializable]
public class WeatherInfo
{
    public Coord coord;
    public Weather[] weather;
    public string @base;
    public Main main;
    public int visibility;
    public Wind wind;
    public Rain rain;
    public Clouds clouds;
    public int dt;
    public Sys sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;
}
