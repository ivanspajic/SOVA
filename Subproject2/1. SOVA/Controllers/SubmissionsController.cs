using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace _1._SOVA.Controllers
{
    [ApiController]
    [Route("api/Submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionRepository submissionRepository;

        public SubmissionsController(ISubmissionRepository submissionRepository)
        {
            this.submissionRepository = submissionRepository;
        }

        
    }
}

