using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using static SpotifyAPI.Web.Scopes;

namespace SpotiLarm
{
    internal static class Spotify
    {
        private static EmbedIOAuthServer _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);  // Make sure "http://localhost:5000/callback" is in your spotify application as redirect uri!

        private static SpotifyClient _spotifyClient;

        public static string ClientId { get; set; }
        //public static string ClientSecret { get; set; }

        public static async Task Play()
        {
            var devicesResponse = await _spotifyClient.Player.GetAvailableDevices();

            await _spotifyClient.Player.ResumePlayback(new PlayerResumePlaybackRequest() 
            { 
                DeviceId = devicesResponse.Devices.Single(d => d.Name == "EPUAKYIW1807").Id 
            });
        }

        public static async Task LogIn()
        {
            var (verifier, challenge) = PKCEUtil.GenerateCodes();

            await _server.Start();
            _server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await _server.Stop();
                PKCETokenResponse token = await new OAuthClient()
                  .RequestToken(new PKCETokenRequest(ClientId, response.Code, _server.BaseUri, verifier));

                var authenticator = new PKCEAuthenticator(ClientId, token);
                var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(authenticator);
                _spotifyClient = new SpotifyClient(config);
            };
            _server.ErrorReceived += OnErrorReceived;

            var request = new LoginRequest(_server.BaseUri, ClientId, LoginRequest.ResponseType.Code)
            {
                CodeChallenge = challenge,
                CodeChallengeMethod = "S256",
                Scope = new List<string> { AppRemoteControl, Streaming, UserModifyPlaybackState, UserReadPlaybackState }
            };

            BrowserUtil.Open(request.ToUri());
        }
    
        //public static async Task LogIn()
        //{
        //    await _server.Start();

        //    _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
        //    _server.ErrorReceived += OnErrorReceived;

        //    var request = new LoginRequest(_server.BaseUri, ClientId, LoginRequest.ResponseType.Code)
        //    {
        //        Scope = new List<string> { AppRemoteControl, Streaming, UserModifyPlaybackState, UserReadPlaybackState }
        //    };
        //    BrowserUtil.Open(request.ToUri());
        //}

        //private static async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        //{
        //    await _server.Stop();

        //    var config = SpotifyClientConfig.CreateDefault();
        //    var tokenResponse = await new OAuthClient(config).RequestToken(
        //      new AuthorizationCodeTokenRequest(ClientId, ClientSecret, response.Code, new Uri("http://localhost:5000/callback")));

        //    _spotifyClient = new SpotifyClient(tokenResponse.AccessToken);
        //}

        private static async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }
    }
}
