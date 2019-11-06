using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2._Data_Layer_Abstractions;

namespace _1._SOVA.Controllers
{
    public class SubmissionsController
    {
        private readonly ISubmissionRepository submissionRepository;

        public SubmissionsController(ISubmissionRepository submissionRepository)
        {
            this.submissionRepository = submissionRepository;
        }
    }
}
