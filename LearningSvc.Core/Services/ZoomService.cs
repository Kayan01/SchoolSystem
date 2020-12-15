using LearningSvc.Core.ViewModels.Zoom;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LearningSvc.Core.Services
{
    public static class ZoomToken
    {
        public static string GetNewToken(string ApiKey, string ApiSecret)
        {
            // Token will be good for 20 minutes
            DateTime Expiry = DateTime.UtcNow.AddMinutes(20);

            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            // Create Security key  using private key above:
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApiSecret));

            // length should be >256b
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            var header = new JwtHeader(credentials);

            //Zoom Required Payload
            var payload = new JwtPayload
        {
            { "iss", ApiKey},
            { "exp", ts },
        };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }
    }

    public class ZoomService
    {
        private IConfiguration Configuration { get; }

        public ZoomService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<ZoomObj> GetZoomID(string className)
        {
            var zoomObj = new zoomSendingObject()
            {
                topic = className,
                type = 3,
                password = "123456789",
                settings = new Settings()
                {
                    host_video = true,
                    join_before_host = false,
                    waiting_room = true,
                    approval_type = 3,
                    audio = "both",
                    auto_recording = "none",
                    meeting_authentication = false,
                    mute_upon_entry = true,
                    participant_video = true,
                    registrants_email_notification = false,
                    show_share_button = false,
                    watermark = false
                }
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.zoom.us/v2/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                //"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOm51bGwsImlzcyI6InQ4NWNEbm5mUjZtWDZVRTFjNnBQT0EiLCJleHAiOjE2MDg1ODQxNjIsImlhdCI6MTYwNzk3OTM2Mn0.LjFP9-2cIF9s19-BbQGsgm66gjETp6N2BAbzpMN0Um8"

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ZoomToken.GetNewToken(Configuration["ZoomApiKey"], Configuration["ZoomApiSecret"]));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "users/me/meetings");
                request.Content = new StringContent(JsonConvert.SerializeObject(zoomObj),
                                                    Encoding.UTF8,
                                                    "application/json");//CONTENT-TYPE header

                var result = await client.SendAsync(request);

                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadAsStringAsync();
                    var dt = JsonConvert.DeserializeObject<zoomReturnObject>(data);

                    var zObj = new ZoomObj() { id = dt.id, start_url = dt.start_url };
                    return zObj;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
