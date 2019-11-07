using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/Comment")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            this._commentRepository = commentRepository;
        }

        
    }
}

