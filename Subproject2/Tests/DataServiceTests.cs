using System;
using System.Collections.Generic;
using System.Linq;
using _0._Models;
using _3._Data_Layer;
using _3._Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Xunit;

namespace Tests
{
    public class DataServiceTests
    {
        private readonly string _connectionString = "host=localhost;db=stackoverflow;uid=postgres;pwd=";

        private readonly AnnotationRepository _annotationRepository;
        private readonly AnswerRepository _answerRepository;
        private readonly CommentRepository _commentRepository;
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
        public void CreateAnnotation_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            string annotation = "Test Annotation";
            int submissionId = 19;
            int userId = 1;

            // Act
            Annotation actualAnnotation = annotationRepository.Create(annotation, submissionId, userId);

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
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Act
            Annotation actualAnnotation = annotationRepository.Create(annotation, submissionId, userId);

            // Assert
            Assert.Equal(default, actualAnnotation);
        }

        [Fact]
        public void GetAnnotationBySubmissionAndUserIds_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            string annotation = "Test Annotation";
            int submissionId = 19;
            int userId = 1;

            // Act
            Annotation actualAnnotation = annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);

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
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Act
            Annotation actualAnnotation = annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);

            // Assert
            Assert.Equal(default, actualAnnotation);
        }

        [Fact]
        public void UpdateAnnotationOnSubmissionForUser_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            // Create annotation in case it does not exist
            int submissionId = 19;
            int userId = 1;
            annotationRepository.Create("Creating annotation", submissionId, userId);

            string updatedAnnotation = "Test Test";

            // Act
            bool updated = annotationRepository.Update(updatedAnnotation, submissionId, userId);

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
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Act
            bool updated = annotationRepository.Update(annotation, submissionId, userId);

            // Assert
            Assert.False(updated);
        }

        [Fact]
        public void DeleteAnnotationOnSubmissionForUser_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            int submissionId = 19;
            int userId = 1;

            // Act
            bool deleted = annotationRepository.Delete(submissionId, userId);

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
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Act
            bool deleted = annotationRepository.Delete(submissionId, userId);

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
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Act 
            annotationRepository.Create(annotation, submissionId, userId);

            // Assert
            Assert.True(databaseContext.Annotations.Count() <= 1);
        }

        [Fact]
        public void GetAnnotationBySubmissionAndUserIds_AnnotationWithSubmission()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);

            // Create annotation in case it does not exist
            int submissionId = 19;
            int userId = 1;
            annotationRepository.Create("Creating annotation", submissionId, userId);

            // Act
            Annotation annotation = annotationRepository.GetBySubmissionAndUserIds(submissionId, userId);

            // Assert
            Assert.Equal(submissionId, annotation.SubmissionId);
        }

        [Fact]
        public void GetAnswerById_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnswerRepository answerRepository = new AnswerRepository(databaseContext);

            int answerId = 106266;

            // Act
            Answer answer = answerRepository.GetAnswerById(answerId);

            // Assert
            Assert.Equal(answerId, answer.SubmissionId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetAnswerById_InvalidArgument(int answerId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnswerRepository answerRepository = new AnswerRepository(databaseContext);

            // Act
            Answer answer = answerRepository.GetAnswerById(answerId);

            // Assert
            Assert.Equal(default, answer);
        }

        [Fact]
        public void GetAnswerById_IncludesSubmissions_IncludesComments_ThenIncludesSubmissions()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnswerRepository answerRepository = new AnswerRepository(databaseContext);

            int answerId = 106266;

            // Act
            Answer answer = answerRepository.GetAnswerById(answerId);

            // Assert
            Assert.NotNull(answer.Submission);
            Assert.All(answer.Comments, (comment) => Assert.NotNull(comment.CommentSubmission));
        }

        [Fact]
        public void GetAnswersByQuestionId_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnswerRepository answerRepository = new AnswerRepository(databaseContext);

            int questionId = 19;

            PagingAttributes testAttributes = new PagingAttributes();

            // Act
            IEnumerable<Answer> answers = answerRepository.GetAnswersForQuestionById(questionId, testAttributes);

            // Assert
            Assert.All(answers, (answer) => Assert.Equal(questionId, answer.ParentId));
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(-1, 1, 1)]
        public void GetAnswersByQuestionId_InvalidArguments(int questionId, int pageSize, int pageNumber)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnswerRepository answerRepository = new AnswerRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes()
            {
                Page = pageNumber - 1,
                PageSize = pageSize
            };

            // Act
            IEnumerable<Answer> answers = answerRepository.GetAnswersForQuestionById(questionId, testAttributes);

            // Assert
            Assert.Equal(default, answers);
        }

        [Fact]
        public void GetAnswersByQuestionId_IncludesSubmission_IncludesComments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnswerRepository answerRepository = new AnswerRepository(databaseContext);

            int questionId = 19;

            PagingAttributes testAttributes = new PagingAttributes();

            // Act
            IEnumerable<Answer> answers = answerRepository.GetAnswersForQuestionById(questionId, testAttributes);

            // Assert
            Assert.All(answers, (answer) =>
            {
                Assert.NotNull(answer.Submission);
                Assert.All(answer.Comments, (comment) => Assert.NotNull(comment.CommentSubmission));
            });
        }

        [Fact]
        public void GetNumberOfCommentsOnSubmission_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            CommentRepository commentRepository = new CommentRepository(databaseContext);

            int submissionId = 19;
            int expectedNumberOfComments = databaseContext.Comments.Where(comment => comment.SubmissionId == submissionId).Count();

            // Act
            int actualNumberOfComments = commentRepository.NoOfComments(submissionId);

            // Assert
            Assert.Equal(expectedNumberOfComments, actualNumberOfComments);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetNumberOfCommentsOnSubmission_InvalidArgument(int submissionId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            CommentRepository commentRepository = new CommentRepository(databaseContext);

            // Act
            int numberOfComments = commentRepository.NoOfComments(submissionId);

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
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            CommentRepository commentRepository = new CommentRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes()
            {
                Page = pageNumber - 1,
                PageSize = pageSize
            };

            IEnumerable<Comment> expectedComments = databaseContext.Comments
                                                        .Where(comment => comment.SubmissionId == submissionId)
                                                        .Skip(pageNumber * pageSize)
                                                        .Take(pageSize);

            // Act
            IEnumerable<Comment> actualComments = commentRepository.GetAllCommentsBySubmissionId(submissionId, testAttributes);

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
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            CommentRepository commentRepository = new CommentRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes()
            {
                Page = pageNumber - 1,
                PageSize = pageSize
            };

            // Act
            IEnumerable<Comment> comments = commentRepository.GetAllCommentsBySubmissionId(submissionId, testAttributes);

            // Assert
            Assert.Empty(comments);
        }

        [Fact]
        public void GetCommentsBySubmissionId_IncludeSubmissions()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            CommentRepository commentRepository = new CommentRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            int submissionId = 19;

            PagingAttributes testAttributes = new PagingAttributes();

            // Act
            IEnumerable<Comment> comments = commentRepository.GetAllCommentsBySubmissionId(submissionId, testAttributes);

            // Arrange
            Assert.All(comments, (comment) => Assert.NotNull(comment.CommentSubmission));
        }

        [Fact]
        public void GetHistoryByUserId_ValidArgument()
        {
            // Arrange
            PagingAttributes testAttributes = new PagingAttributes();
            int userId = 1;

            // Act
            var userHistory = _userHistoryRepository.GetUserHistoryByUserId(userId, testAttributes);

            // Assert
            Assert.True(userHistory != null);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetHistoryById_InvalidArgument(int userId)
        {
            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            // Do a search so database has some history for test to find. (In case it history doesn't have any entry, the test will fail.)
            _questionRepository.SearchQuestions("testing", 1, new PagingAttributes());
            // Arrange
            PagingAttributes testAttributes = new PagingAttributes();

            // Act
            var history = _userHistoryRepository.GetUserHistoryByUserId(userId, testAttributes);

            // Assert
            Assert.Equal(default, history);
        }

        [Fact]
        public void GetLinkPostByQuestionAndLinkedPostIds_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            LinkPostRepository linkPostRepository = new LinkPostRepository(databaseContext);

            int questionId = 6173;
            int linkedPostId = 1732348;

            // Act
            LinkPost linkPost = linkPostRepository.GetByQuestionAndLinkedPostIds(questionId, linkedPostId);

            // Assert
            Assert.Equal(questionId, linkPost.QuestionId);
            Assert.Equal(linkedPostId, linkPost.LinkPostId);
        }

        [Theory]
        [InlineData(0, 1732348)]
        [InlineData(-1, 1732348)]
        [InlineData(6173, 0)]
        [InlineData(6173, -1)]
        [InlineData(-1, -1)]
        public void GetLinkPostByQuestionAndLinkedPostIds_InvalidArgument(int questionId, int linkedPostId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            LinkPostRepository linkPostRepository = new LinkPostRepository(databaseContext);

            // Act
            LinkPost linkPost = linkPostRepository.GetByQuestionAndLinkedPostIds(questionId, linkedPostId);

            // Assert
            Assert.Equal(default, linkPost);
        }

        [Fact]
        public void GetLinkPostByQuestionAndLinkedPostIds_IncludeQuestion_IncludeSubmission()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            LinkPostRepository linkPostRepository = new LinkPostRepository(databaseContext);

            int questionId = 6173;
            int linkedPostId = 1732348;

            // Act
            LinkPost linkPost = linkPostRepository.GetByQuestionAndLinkedPostIds(questionId, linkedPostId);

            // Assert
            Assert.NotNull(linkPost.LinkedPost.Submission);
        }

        [Fact]
        public void GetBookmarkBySubmissionAndUserIds_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            int submissionId = 19;
            int userId = 1;

            markingRepository.RemoveBookmark(submissionId, userId);
            // Act
            markingRepository.AddBookmark(submissionId, userId);
            bool bookmarked = markingRepository.IsMarked(submissionId, userId);
            markingRepository.RemoveBookmark(submissionId, userId);

            // Assert
            Assert.True(bookmarked);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(19, 0)]
        [InlineData(19, -1)]
        [InlineData(-1, -1)]
        public void GetBookmarkBySubmissionAndUserIds_InvalidArguments(int submissionId, int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Act
            bool bookmarked = markingRepository.IsMarked(submissionId, userId);

            // Assert
            Assert.False(bookmarked);
        }

        [Fact]
        public void GetNumberOfBookmarkedSubmissions_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            int userId = 1;

            // Act
            int bookmarkedSubmissions = markingRepository.NoOfMarkings(userId);

            // Assert
            Assert.True(bookmarkedSubmissions > -1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetNumberOfBookmarkedSubmissions_InvalidArgument(int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Act
            int bookmarkedSubmissions = markingRepository.NoOfMarkings(userId);

            // Assert
            Assert.Equal(0, bookmarkedSubmissions);
        }

        [Fact]
        public void CreateBookmarkOnSubmissionForUser_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");

            int submissionId = 19;
            int userId = 1;

            // Act
            bool bookmarked = _markingRepository.AddBookmark(submissionId, userId);

            // Assert
            Assert.True(bookmarked);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(19, 0)]
        [InlineData(19, -1)]
        [InlineData(-1, -1)]
        public void CreateBookmarkOnSubmissionForUser_InvalidArguments(int submissionId, int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Act
            bool bookmarked = _markingRepository.AddBookmark(submissionId, userId);

            // Assert
            Assert.False(bookmarked);
        }

        [Fact]
        public void DeleteBookmarkOnSubmissionForUser_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Create User in case they don't exist
            _userRepository.CreateUser("testUser", "testPassword", "testSalt");


            int submissionId = 19;
            int userId = 1;

            // Create a bookmark for test to dlelte if it doesn't exist.
            markingRepository.AddBookmark(submissionId, userId);

            // Act
            bool bookmarked = markingRepository.RemoveBookmark(submissionId, userId);

            // Assert
            Assert.True(bookmarked);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(19, 0)]
        [InlineData(19, -1)]
        [InlineData(-1, -1)]
        public void DeleteBookmarkOnSubmissionForUser_InvalidArguments(int submissionId, int userId)
        {
            // Assert
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            // Act
            bool bookmarked = markingRepository.RemoveBookmark(submissionId, userId);

            // Assert
            Assert.False(bookmarked);
        }

        [Fact]
        public void GetQuestionById_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);

            int questionId = 19;

            // Act
            Question question = questionRepository.GetById(questionId);

            // Assert
            Assert.Equal(questionId, question.SubmissionId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetQuestionById_InvalidArgument(int questionId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);

            // Act
            Question question = questionRepository.GetById(questionId);

            // Assert
            Assert.Equal(default, question);
        }

        [Theory]
        [InlineData("test search", 1)]
        [InlineData("test search again", null)]
        public void GetNumberOfQuestionSearchResults_ValidArguments(string query, int? userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);

            int expectedResultCount = databaseContext.Questions.Include(question => question.Submission)
                                         .Where(question => question.Submission.Body.ToLower()
                                             .Contains(query.ToLower()) && question.Title.ToLower()
                                             .Contains(query.ToLower())).Count();

            // Act
            int actualResultCount = questionRepository.NoOfResults(query, userId);

            // Assert
            Assert.Equal(expectedResultCount, actualResultCount);
        }

        [Theory]
        [InlineData("", 1)]
        [InlineData("", null)]
        [InlineData(" ", 1)]
        [InlineData(" ", null)]
        [InlineData(null, -1)]
        public void GetNumberOfQuestionSearchResults_InvalidArguments(string query, int? userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);

            // Act
            int resultCount = questionRepository.NoOfResults(query, userId);

            // Assert
            Assert.Equal(0, resultCount);
        }
    }
}