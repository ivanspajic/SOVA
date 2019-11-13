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
        public void CreateAnnotationOnSubmissionForUser_InvalidArguments(string annotation, int submissionId, int userId)
        {
            // Act
            Annotation actualAnnotation = _annotationRepository.Create(annotation, submissionId, userId);

            // Assert
            Assert.Equal(default, actualAnnotation);
        }

        [Theory]
        [InlineData("Test Annotation", 19, 1)]
        public void GetAnnotationBySubmissionAndUserIds_ValidArguments(string annotation, int submissionId, int userId)
        {
            // Act
            Annotation actualAnnotation = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);

            // Assert
            Assert.Equal(annotation, actualAnnotation.AnnotationString);
            Assert.Equal(submissionId, actualAnnotation.SubmissionId);
            Assert.Equal(userId, actualAnnotation.UserId);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(19, 0)]
        [InlineData(19, -1)]
        [InlineData(0, 0)]
        public void GetAnnotationBySubmissionAndUserIds_InvalidArguments(int submissionId, int userId)
        {
            // Act
            Annotation actualAnnotation = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);

            // Assert
            Assert.Equal(default, actualAnnotation);
        }

        [Theory]
        [InlineData("Test Test", 19, 1)]
        public void UpdateAnnotationOnSubmissionForUser_ValidArguments(string annotation, int submissionId, int userId)
        {
            // Act
            bool updated = _annotationRepository.Update(annotation, submissionId, userId);

            // Assert
            Assert.True(updated);
        }

        [Theory]
        [InlineData("", 19, 1)]
        [InlineData(" ", 19, 1)]
        [InlineData(null, 19, 1)]
        [InlineData("Test Test", 0, 1)]
        [InlineData("Test Test", -1, 1)]
        [InlineData("Test Test", 19, 0)]
        [InlineData("Test Test", 19, -1)]
        [InlineData("", 0, 0)]
        public void UpdateAnnotationOnSubmissionForUser_InvalidArguments(string annotation, int submissionId, int userId)
        {
            // Act
            bool updated = _annotationRepository.Update(annotation, submissionId, userId);

            // Assert
            Assert.False(updated);
        }

        [Theory]
        [InlineData(19, 1)]
        public void DeleteAnnotationOnSubmissionForUser_ValidArguments(int submissionId, int userId)
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
        public void DeleteAnnotationOnSubmissionForUser_InvalidArguments(int submissionId, int userId)
        {
            // Act
            bool deleted = _annotationRepository.Delete(submissionId, userId);

            // Assert
            Assert.False(deleted);
        }

        [Theory]
        [InlineData("Something here", 19, 1)]
        [InlineData("Something else here", 19, 1)]
        public void CheckAtMostOneAnnotationOnSubmissionForUser_CreateAnnotation(string annotation, int submissionId, int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);

            // Act 
            _annotationRepository.Create(annotation, submissionId, userId);

            // Assert
            Assert.True(databaseContext.Annotations.Count() <= 1);
        }

        [Theory]
        [InlineData(19, 1)]
        public void GetAnnotationBySubmissionAndUserIds_AnnotationWithSubmission(int submissionId, int userId)
        {
            // Act
            Annotation annotation = _annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);

            // Assert
            Assert.Equal(submissionId, annotation.Submission.Id);
        }

        [Theory]
        [InlineData(106266)]
        public void GetAnswerById_ValidArgument(int id)
        {
            // Act
            Answer answer = _answerRepository.GetAnswerById(id);

            // Assert
            Assert.Equal(id, answer.SubmissionId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetAnswerById_InvalidArgument(int id)
        {
            // Act
            Answer answer = _answerRepository.GetAnswerById(id);

            // Assert
            Assert.Equal(default, answer);
        }

        [Theory]
        [InlineData(106266)]
        public void GetAnswerById_AnswerWithSubmission(int id)
        {
            // Act
            Answer answer = _answerRepository.GetAnswerById(id);

            // Assert
            Assert.Equal(id, answer.Submission.Id);
        }
    }
}
