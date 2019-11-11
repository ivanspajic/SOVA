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

        [Theory]
        [InlineData("Test Annotation", 19, 1)]
        public void CreateAnnotation_ValidArguments(string annotation, int submissionId, int userId)
        {
            // Act
            Annotation actualAnnotation = _annotationRepository.Create(annotation, submissionId, userId);

            // Assert
            Assert.Equal(annotation, actualAnnotation.AnnotationString);
            Assert.Equal(submissionId, actualAnnotation.SubmissionId);
            Assert.Equal(userId, actualAnnotation.UserId);
        }

        [Theory]
        [InlineData("", 19, 1)]
        [InlineData(" ", 19, 1)]
        [InlineData(null, 19, 1)]
        [InlineData("Test Annotation", 0, 1)]
        [InlineData("Test Annotation", -1, 1)]
        [InlineData("Test Annotation", 19, 0)]
        [InlineData("Test Annotation", 19, -1)]
        [InlineData("", 0, 0)]
        public void CreateAnnotation_InvalidArguments(string annotation, int submissionId, int userId)
        {
            // Act
            Annotation actualAnnotation = _annotationRepository.Create(annotation, submissionId, userId);

            // Assert
            Assert.Equal(default, actualAnnotation);
        }

        [Theory]
        [InlineData(19, 1)]
        public void DeleteExistentAnnotation_ValidArguments(int submissionId, int userId)
        {
            // Act
            bool deleted = _annotationRepository.Delete(submissionId, userId);

            // Assert
            Assert.True(deleted);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(19, 0)]
        [InlineData(19, -1)]
        [InlineData(0, 0)]
        public void DeleteExistentAnnotation_InvalidArguments(int submissionId, int userId)
        {
            // Act
            bool deleted = _annotationRepository.Delete(submissionId, userId);

            // Assert
            Assert.False(deleted);
        }
    }
}
