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
    // Missing Tests:
    //
    // - getting questions including all of their things
    // - getting all bookmarks containing all respective posts
    // - getting questions by search queries
    // - (questiontag repository)
    // - get somember by id
    // - tag repository (maybe for displaying all relevant questions when clicking on a tag?)
    // - get number of stored search results for a user
    // - get user's search history by id paginated
    // - get user by id
    // - get user by username
    // - create user
    // - update user
    public class DataServiceTests
    {
        private readonly string _connectionString = "host=localhost;db=stackoverflow;uid=postgres;pwd=";

        [Fact]
        public void CreateAnnotation_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            AnnotationRepository annotationRepository = new AnnotationRepository(databaseContext);


            string annotation = "Test Annotation";
            int submissionId = 19;

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            Annotation actualAnnotation = annotationRepository.Create(annotation, submissionId, testUser.Id);

            // Assert
            Assert.Equal(annotation, actualAnnotation.AnnotationString);
            Assert.Equal(submissionId, actualAnnotation.SubmissionId);
            Assert.Equal(testUser.Id, actualAnnotation.UserId);
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

            int submissionId = 19;
            string updatedAnnotation = "Test Test";

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            EnsureTestAnnotationExistsThroughContext_ReturnsTestAnnotation(testUser.Id);

            // Act
            bool updated = annotationRepository.Update(updatedAnnotation, submissionId, testUser.Id);

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

            int submissionId = 19;

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            bool deleted = annotationRepository.Delete(submissionId, testUser.Id);

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

            int submissionId = 19;

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            EnsureTestAnnotationExistsThroughContext_ReturnsTestAnnotation(testUser.Id);

            // Act
            Annotation annotation = annotationRepository.GetBySubmissionAndUserIds(submissionId, testUser.Id);

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

            int submissionId = 19;

            PagingAttributes testAttributes = new PagingAttributes();



            // Act
            IEnumerable<Comment> comments = commentRepository.GetAllCommentsBySubmissionId(submissionId, testAttributes);

            // Arrange
            Assert.All(comments, (comment) => Assert.NotNull(comment.CommentSubmission));
        }

        [Fact]
        public void GetUserHistoryByUserId_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes();

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            IEnumerable<UserHistory> userHistory = userHistoryRepository.GetUserHistoryByUserId(testUser.Id, testAttributes);

            // Assert
            Assert.True(userHistory != null);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetUserHistoryByUserId_InvalidArgument(int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes();

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", testUser.Id, "testing");

            // Act
            IEnumerable<UserHistory> history = userHistoryRepository.GetUserHistoryByUserId(userId, testAttributes);

            // Assert
            Assert.Equal(default, history);
        }

        [Fact]
        public void GetUserHistoryByUserId_IncludesHistory()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes();

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            databaseContext.SearchResults.FromSqlRaw("SELECT * FROM best_match_weighted({0}, {1})", testUser.Id, "testing");

            // Act
            IEnumerable<UserHistory> histories = userHistoryRepository.GetUserHistoryByUserId(testUser.Id, testAttributes);

            // Assert
            Assert.All(histories, (history) => Assert.NotNull(history.History));
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

            int submissionId = 19;

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            Marking marking = databaseContext.Markings.Find(submissionId, testUser.Id);
            if (marking == null)
            {
                databaseContext.Markings.Add(new Marking
                {
                    SubmissionId = submissionId,
                    UserId = testUser.Id
                });
            }

            databaseContext.SaveChanges();

            // Act
            bool bookmarked = markingRepository.IsMarked(submissionId, testUser.Id);

            // Assert
            Assert.True(bookmarked);

            // Clean-Up
            marking = databaseContext.Markings.FirstOrDefault(b => b.SubmissionId == submissionId && b.UserId == testUser.Id);
            databaseContext.Markings.Remove(marking);

            databaseContext.SaveChanges();
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

            int submissionId = 19;

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // First make sure it is not bookmarked.
            markingRepository.RemoveBookmark(submissionId, testUser.Id);

            // Act
            bool bookmarked = markingRepository.AddBookmark(submissionId, testUser.Id);

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
            bool bookmarked = markingRepository.AddBookmark(submissionId, userId);

            // Assert
            Assert.False(bookmarked);
        }

        [Fact]
        public void DeleteBookmarkOnSubmissionForUser_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            MarkingRepository markingRepository = new MarkingRepository(databaseContext);

            int submissionId = 19;

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Create a bookmark for test to dlelte if it doesn't exist.
            markingRepository.AddBookmark(submissionId, testUser.Id);

            // Act
            bool bookmarked = markingRepository.RemoveBookmark(submissionId, testUser.Id);

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

        public User EnsureTestUserExistsThroughContext_ReturnsTestUser()
        {
            SOVAContext databaseContext = new SOVAContext(_connectionString);

            string testUsername = "testUsername";
            string testPassword = "testPassword";
            string testSalt = "testSalt";

            User testUser = databaseContext.Users.FirstOrDefault(u => u.Username == testUsername);

            if (testUser != null) return testUser;

            databaseContext.Users.Add(new User
            {
                Username = testUsername,
                Password = testPassword,
                Salt = testSalt
            });

            databaseContext.SaveChanges();

            return databaseContext.Users.Where(user => user.Username == testUsername).First();
        }

        public Annotation EnsureTestAnnotationExistsThroughContext_ReturnsTestAnnotation(int userId)
        {
            SOVAContext databaseContext = new SOVAContext(_connectionString);

            string annotation = "Test Annotation";
            int submissionId = 19;

            Annotation testAnnotation = databaseContext.Annotations.FirstOrDefault(annotation => annotation.SubmissionId == submissionId && annotation.UserId == userId);

            if (testAnnotation != null) return testAnnotation;

            databaseContext.Annotations.Add(new Annotation
            {
                AnnotationString = annotation,
                SubmissionId = submissionId,
                UserId = userId
            });

            databaseContext.SaveChanges();

            return databaseContext.Annotations.FirstOrDefault(annotation => annotation.SubmissionId == submissionId && annotation.UserId == userId);
        }
    }
}
