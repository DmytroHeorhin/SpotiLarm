using SpotiLarm;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("config.json").Build();

//Spotify.ClientId = config.GetSection("spotifyClientId").Value;
//Spotify.ClientSecret = config.GetSection("spotifyClientSecret").Value;
//await Spotify.LogIn();

var playTime = DateTime.Parse(config.GetSection("playTime").Value);
Clock.WaitUntill(playTime);

//SystemSound.DecreaseVolume();
//await Spotify.Play();

for (int i = 0; i < 100; i++)
{
    SystemSound.IncreaseVolume();
    Clock.WaitHalfAMinute();
}

Console.ReadKey();
