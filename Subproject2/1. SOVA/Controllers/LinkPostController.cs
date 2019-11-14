using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0._Models;
using _2._Data_Layer_Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/linkpost")]
    public class LinkPostController : ControllerBase
    {
        private readonly ILinkPostRepository _linkPostRepository;
        private IMapper _mapper;

        public LinkPostController(ILinkPostRepository linkPostRepository, IMapper mapper)
        {
            _linkPostRepository = linkPostRepository;
            _mapper = mapper;
        }

        [HttpGet("{questionId}", Name = nameof(GetLinkPostByQuestionId))]
        public IEnumerable<LinkPost> GetLinkPostByQuestionId(int questionId)
        {
            return _linkPostRepository.GetByQuestionAndLinkedPostIds(questionId);
        }
    }
}
