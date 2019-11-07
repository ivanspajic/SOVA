using System;
using System.IO;
using System.Linq;
using _3._Data_Layer;
using _3._Data_Layer.Database_Context;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests
{
    public class DataServiceTests
    {
        [Fact]
        public void GetRandomTenQuestions()
        {
            using var db = new SOVAContext("host=localhost;db=stackoverflow;uid=postgres;pwd=");
            var service = new QuestionRepository(db);
            var submissions = service.GetRandomTenQuestions().ToList();
            Assert.Equal(10, submissions.Count);
        }
    }
}
