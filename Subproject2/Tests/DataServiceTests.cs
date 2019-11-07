using System;
using System.Linq;
using _3._Data_Layer;
using _3._Data_Layer.Database_Context;
using Xunit;

namespace Tests
{
    public class DataServiceTests
    {
        [Fact]
        public void GetRandomTenQuestions()
        {
            using var db = new SOVAContext();
            var service = new QuestionRepository(db);
            var submissions = service.GetRandomTenQuestions().ToList();
            Assert.Equal(10, submissions.Count);
        }
    }
}
