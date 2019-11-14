using System;
using System.Collections.Generic;
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
        public void GetAnswerById_ValidArgument(int answerId)
        {
            // Act
            Answer answer = _answerRepository.GetAnswerById(answerId);

            // Assert
            Assert.Equal(answerId, answer.SubmissionId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetAnswerById_InvalidArgument(int answerId)
        {
            // Act
            Answer answer = _answerRepository.GetAnswerById(answerId);

            // Assert
            Assert.Equal(default, answer);
        }

        [Theory]
        [InlineData(106266)]
        public void GetAnswerById_AnswerWithSubmission(int answerId)
        {
            // Act
            Answer answer = _answerRepository.GetAnswerById(answerId);

            // Assert
            Assert.Equal(answerId, answer.Submission.Id);
        }

        [Theory]
        [InlineData(19)]
        public void GetNumberOfCommentsOnSubmission_ValidArgument(int submissionId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);

            int expectedNumberOfComments = databaseContext.Comments.Where(comment => comment.SubmissionId == submissionId).Count();

            // Act
            int actualNumberOfComments = _commentRepository.NoOfComments(submissionId);

            // Assert
            Assert.Equal(expectedNumberOfComments, actualNumberOfComments);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetNumberOfCommentsOnSubmission_InvalidArgument(int submissionId)
        {
            // Act
            int numberOfComments = _commentRepository.NoOfComments(submissionId);

            // Assert
            Assert.Equal(0, numberOfComments);
        }

        [Theory]
        [InlineData(19, 20, 1)]
        [InlineData(19, 5, 5)]
        [InlineData(19, 2, 10)]
        public void GetCommentsBySubmissionId_ValidArguments(int submissionId, int pageNumber, int pageSize)
        {
            // Arrange
            PagingAttributes testAttributes = new PagingAttributes()
            {
                Page = pageNumber,
                PageSize = pageSize
            };

            SOVAContext databaseContext = new SOVAContext(_connectionString);
            
            IEnumerable<Comment> expectedComments = databaseContext.Comments
                                                        .Where(comment => comment.SubmissionId == submissionId)
                                                        .Skip((pageNumber - 1) * pageSize)
                                                        .Take(pageSize);

            // Act
            IEnumerable<Comment> actualComments = _commentRepository.GetAllCommentsBySubmissionId(submissionId, testAttributes);

            // Assert
            Assert.Equal(expectedComments, actualComments);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(-1, 1, 1)]
        [InlineData(19, 1, 0)]
        [InlineData(19, 1, -2)]
        [InlineData(19, -2, 1)]
        [InlineData(-1, -1, -1)]
        public void GetCommentsBySubmissionId_InvalidArguments(int submissionId, int pageNumber, int pageSize)
        {
            // Arrange
            PagingAttributes testAttributes = new PagingAttributes()
            {
                Page = pageNumber,
                PageSize = pageSize
            };

            // Act
            IEnumerable<Comment> comments = _commentRepository.GetAllCommentsBySubmissionId(submissionId, testAttributes);

            // Assert
            Assert.Empty(comments);
        }

        [Theory]
        [InlineData(1)]
        public void GetHistoryById_ValidArgument(int historyId)
        {
            // Act
            History history = _historyRepository.GetHistoryById(historyId);

            // Assert
            Assert.Equal(historyId, history.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetHistoryById_InvalidArgument(int historyId)
        {
            // Act
            History history = _historyRepository.GetHistoryById(historyId);

            // Assert
            Assert.Equal(default, history);
        }
    }
}
