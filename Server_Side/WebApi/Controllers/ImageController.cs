using BL;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using System.Web.Mvc;

namespace WebApi.Controllers
{
    [RoutePrefix("WebApi/Image")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageController : ApiController
    {
        [Route("InsertImages")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertImages()
        {
            return Ok(await InitImages.InsertImages());
        }
        [Route("GetImages")]
        [HttpGet]
        public IHttpActionResult GetImages()
        {
            return Ok(Images.GetImages());
        }
        [Route("InsertGroom")]
        [HttpPost]
        public IHttpActionResult InsertImagesGroom()
        {
            return Ok(InitGroom.InsertGroom());
        }
    }
}