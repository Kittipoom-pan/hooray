using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]
    public class ComentController : ControllerBase
    {
        private readonly IComentServices _IComentServices;
        public ComentController(IComentServices IComentServices)
        {
            _IComentServices = IComentServices;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("coment")]
        [Route("getallfeedcommentlist")]
        public async Task<IActionResult> GetAllFeedCommentList(int uid, string cpid, [FromQuery] PaginationFilter pageFilter, string lang)
        {
            try
            {
                var route = Request.Path.Value;
                var data = await _IComentServices.GetAllFeedCommentList(uid, cpid, pageFilter, lang, route);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("coment")]
        [Route("addnewcomment")]
        public IActionResult AddNewComment(int uid, string cpid, string comment, string tokenID, string lang)
        {
            try
            {
                var data = _IComentServices.AddNewComment( uid,  cpid,  comment,  tokenID,  lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
                
            }
        }
    }
}
