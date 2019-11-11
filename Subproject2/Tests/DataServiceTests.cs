using System;
using System.Linq;
using _0._Models;
using _3._Data_Layer;
using _3._Data_Layer.Database_Context;
using Xunit;

namespace Tests
{
    public class DataServiceTests
    {
        // Check if you can make the instances accessible from the Web API project,
        // or supplied into this class via dependency injection through the constructor.
        private readonly string _connectionString = "host=localhost;db=stackoverflow;uid=postgres;pwd=";

        private readonly AnnotationRepository _annotationRepository;
        private readonly AnswerRepository _answerRepository;
        private readonly CommentRepository _commentRepository;
        private readonly HistoryRepository _historyRepository;
        private readonly LinkPostRepository _linkPostRepository;
        private readonly MarkingRepository _markingRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly QuestionTagRepository _questionTagRepository;
        private readonly SoMemberRepository _soMemberRepository;
        private readonly TagRepository _tagRepository;
        private readonly UserHistoryRepository _userHistoryRepository;
        private readonly UserRepository _userRepository;

        public DataServiceTests()
        {
            _annotationRepository = new AnnotationRepository(new SOVAContext(_connectionString));
            _answerRepository = new AnswerRepository(new SOVAContext(_connectionString));
            _commentRepository = new CommentRepository(new SOVAContext(_connectionString));
            _historyRepository = new HistoryRepository(new SOVAContext(_connectionString));
            _linkPostRepository = new LinkPostRepository(new SOVAContext(_connectionString));
            _markingRepository = new MarkingRepository(new SOVAContext(_connectionString));
            _questionRepository = new QuestionRepository(new SOVAContext(_connectionString));
            _questionTagRepository = new QuestionTagRepository(new SOVAContext(_connectionString));
            _soMemberRepository = new SoMemberRepository(new SOVAContext(_connectionString));
            _tagRepository = new TagRepository(new SOVAContext(_connectionString));
            _userHistoryRepository = new UserHistoryRepository(new SOVAContext(_connectionString));
            _userRepository = new UserRepository(new SOVAContext(_connectionString));
        }

        [Fact]
        public void CreateAnnotation_ValidSubmissionId_ValidAnnotation()
        {
            // Arrange
            string annotation = "Test Annotation";
            int submissionId = 19;

            // Act
            Annotation actualAnnotation = _annotationRepository.Create(annotation, submissionId);

            // Assert
            Assert.Equal(annotation, actualAnnotation.AnnotationString);
        }
    }
}
