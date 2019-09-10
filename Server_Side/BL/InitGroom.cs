using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAL;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BL
{
    public class InitGroom
    {
        public static EventEntities DB = new EventEntities();
        public static bool InsertGroom()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            string temp = "~/UploadFile/";
            List<Entities.ImageEntity> l = new List<Entities.ImageEntity>();
            if (httpRequest.Files.Count == 1)
            {

                var postedFile = httpRequest.Files[0];
                if (string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = HttpContext.Current.Server.MapPath(temp + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    Groom groom = new Groom();
                    groom.url = "http://localhost:50637/UploadFile/" + postedFile.FileName;
                    groom.token = getFaceToken(postedFile.InputStream);
                    groom.name = postedFile.FileName;
                    DB.Grooms.Add(groom);
                    DB.SaveChanges();
                    return true;
                }
            }

            return false;
            //return response;
        }
        private static string getFaceToken(Stream filePath)
        {
            string base64String;
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(filePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            const string API_Key = "Yoxjj0Tu2hUPY5D5K-iQ4ZkJoGm2W2r3";
            const string API_Secret = "q97dj2QUarKmaC5NdTOdBXjC4XI2USta";
            const string BaseUrl = "https://api-us.faceplusplus.com/facepp/v3/detect";
            IRestClient _client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("api_key", API_Key);
            request.AddParameter("api_secret", API_Secret);
            request.AddParameter("image_base64", base64String);
            request.AddParameter("return_attributes", "eyestatus");
            var response = _client.Execute(request);
            JObject results = JObject.Parse(response.Content);
            string faceToken = results["faces"].First.Last.First.ToString();
            return faceToken;
        }
    }
}