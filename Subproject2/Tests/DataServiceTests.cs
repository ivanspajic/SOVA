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
        public void GetLatestTenQuestions()
        {
            using var db = new SOVAContext();
            var service = new QuestionRepository(db);
            var submissions = service.GetLatestTenQuestions().ToList();
            Assert.Equal(10, submissions.Count);
        }
    }
}
