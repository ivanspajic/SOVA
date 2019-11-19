using System.Collections.Generic;
using System.Linq;
using Data_Layer;
using Data_Layer.Database_Context;
using Microsoft.EntityFrameworkCore;
using Models;
using Xunit;

namespace Tests
{
    // Missing Tests:
    //
    // - getting all bookmarks containing all respective posts (needs refactoring)
    // - getting questions by search queries                   (needs refactoring)
    // - tag repository (maybe for displaying all relevant questions when clicking on a tag?) (repository missing)
    public class DataServiceTests
    {
        private readonly string _connectionString = "host=localhost;db=stackoverflow;uid=postgres;pwd=";

        //For RUC's database connection
        // private readonly string _connectionString = "host=rawdata.ruc.dk;db=raw4;uid=raw4;pwd=yzOrEFi)";
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
            Assert.Null(actualAnnotation);
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
            annotationRepository.Create(annotation, submissionId, userId);
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
            Assert.Null(actualAnnotation);
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
            Assert.Null(answer);
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

            // Act
            IEnumerable<Answer> answers = answerRepository.GetAnswersForQuestionById(questionId);

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
            Assert.Null(answers);
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
                                                        .Skip(testAttributes.Page * testAttributes.PageSize)
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

        [Theory]
        [InlineData(20, 0)]
        [InlineData(5, 0)]
        [InlineData(2, 0)]
        public void GetUserHistoryByUserId_ValidArguments(int pageSize, int pageNumber)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes
            {
                Page = pageNumber,
                PageSize = pageSize
            };

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", testUser.Id, "testing");

            IEnumerable<UserHistory> expectedHistories = databaseContext.UserHistory
                .Where(userHistory => userHistory.UserId == testUser.Id)
                .Skip(testAttributes.Page * testAttributes.PageSize)
                .Take(testAttributes.PageSize);

            // Act
            IEnumerable<UserHistory> actualHistories = userHistoryRepository.GetUserHistoryByUserId(testUser.Id, testAttributes);

            // Assert
            Assert.Equal(expectedHistories, actualHistories);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(-1, 1, 1)]
        [InlineData(1, 1, 0)]
        [InlineData(1, 1, -2)]
        [InlineData(1, -2, 1)]
        [InlineData(-1, -1, -1)]
        public void GetUserHistoryByUserId_InvalidArguments(int userId, int pageNumber, int pageSize)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            PagingAttributes testAttributes = new PagingAttributes
            {
                Page = pageNumber - 1,
                PageSize = pageSize
            };

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            databaseContext.SearchResults.FromSqlRaw("SELECT * from best_match_weighted({0}, {1})", testUser.Id, "testing");

            // Act
            IEnumerable<UserHistory> histories = userHistoryRepository.GetUserHistoryByUserId(userId, testAttributes);

            // Assert
            Assert.Null(histories);
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
        public void GetUserHistoryCountByUserId_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            int count = userHistoryRepository.NoOfUserHistory(testUser.Id);

            // Assert
            Assert.True(count > -1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetUserHistoryCountByUserId_InvalidArgument(int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserHistoryRepository userHistoryRepository = new UserHistoryRepository(databaseContext);

            // Act
            int count = userHistoryRepository.NoOfUserHistory(userId);

            // Assert
            Assert.Equal(0, count);
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
            Assert.Null(linkPost);
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
            Assert.Null(question);
        }

        [Fact]
        public void GetNumberOfQuestionSearchResults_ValidArguments_UserIdNull()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);
            var query = "test search";
            int? userId = null;
            int expectedResultCount = databaseContext.Questions
                .Include(question => question.Submission)
                .Count(question => question.Submission.Body.ToLower()
                .Contains(query.ToLower()) && question.Title.ToLower()
                .Contains(query.ToLower()));

            // Act
            int actualResultCount = questionRepository.NoOfResults(query, userId);

            // Assert
            Assert.Equal(expectedResultCount, actualResultCount);
        }
        [Fact]
        public void GetNumberOfQuestionSearchResults_ValidArguments_UserId()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);
            var query = "test search again";
            int userId = EnsureTestUserExistsThroughContext_ReturnsTestUser().Id;
            int expectedResultCount = databaseContext.Questions
                .Include(question => question.Submission)
                .Count(question => question.Submission.Body.ToLower()
                                       .Contains(query.ToLower()) && question.Title.ToLower()
                                       .Contains(query.ToLower()));

            // Act
            int actualResultCount = questionRepository.NoOfResults(query, userId);

            // Assert
            Assert.Equal(expectedResultCount, actualResultCount);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData(null, -1)]
        public void GetNumberOfQuestionSearchResults_InvalidArguments_InvalidUser(string query, int? userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);

            // Act
            int resultCount = questionRepository.NoOfResults(query, userId);

            // Assert
            Assert.Equal(0, resultCount);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetNumberOfQuestionSearchResults_InvalidArguments_ValidUser(string query)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);
            var userId = EnsureTestUserExistsThroughContext_ReturnsTestUser().Id;

            // Act
            int resultCount = questionRepository.NoOfResults(query, userId);

            // Assert
            Assert.Equal(0, resultCount);
        }

        [Fact]
        public void GetQuestionById_IncludesSubmission_IncludesCommentsSubmissions_IncludesTags_IncludesAnswersSubmissions_IncludesAnswersCommentsSubmissions()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            QuestionRepository questionRepository = new QuestionRepository(databaseContext);

            int submissionId = 19;

            // Act
            Question question = questionRepository.GetById(submissionId);

            // Assert
            Assert.NotNull(question.Submission);
            Assert.NotNull(question.Comments);
            Assert.All(question.Comments, (comment) => Assert.NotNull(comment.CommentSubmission));
            Assert.NotNull(question.QuestionsTags);
            Assert.All(question.QuestionsTags, (questionsTag) => Assert.NotNull(questionsTag.Tag));
            Assert.NotNull(question.Answers);
            Assert.All(question.Answers, (answer) => Assert.NotNull(answer.Submission));
            Assert.All(question.Answers, (answer) => Assert.NotNull(answer.Comments));
            Assert.All(question.Answers, (answer) => Assert.All(answer.Comments, (comment) => Assert.NotNull(comment.CommentSubmission)));
        }

        [Fact]
        public void GetSoMemberById_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            SoMemberRepository soMemberRepository = new SoMemberRepository(databaseContext);

            int soMemberId = 1;
            string soMemberName = "Jeff Atwood";

            // Act
            SoMember soMember = soMemberRepository.GetSoMemberById(soMemberId);

            // Assert
            Assert.Equal(soMemberName, soMember.DisplayName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetSoMemberbyId_InvalidArgument(int soMemberId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            SoMemberRepository soMemberRepository = new SoMemberRepository(databaseContext);

            // Act
            SoMember soMember = soMemberRepository.GetSoMemberById(soMemberId);

            // Assert
            Assert.Null(soMember);
        }

        [Fact]
        public void GetUserById_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            User expectedUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            User actualUser = userRepository.GetUserById(expectedUser.Id);

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetUserById_InvalidArgument(int userId)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            // Act
            User testUser = userRepository.GetUserById(userId);

            // Assert
            Assert.Null(testUser);
        }

        [Fact]
        public void GetUserByUsername_ValidArgument()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            User expectedUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            User actualUser = userRepository.GetUserByUsername(expectedUser.Username);

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetUserByUsername_InvalidArgument(string username)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            // Act
            User testUser = userRepository.GetUserByUsername(username);

            // Assert
            Assert.Null(testUser);
        }

        [Fact]
        public void CreateUser_ValidArguments()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            string username = "John Miller";
            string password = "hunter2";
            string salt = "pretendthisisrandomstuff";

            // Act
            User testUser = userRepository.CreateUser(username, password, salt);

            // Assert
            Assert.Equal(username, testUser.Username);
            Assert.Equal(password, testUser.Password);
            Assert.Equal(salt, testUser.Salt);

            // Cleanup
            userRepository.DeleteUser(testUser.Username);
        }

        [Theory]
        [InlineData("", "hunter2", "pretendthisisrandomstuff")]
        [InlineData(" ", "hunter2", "pretendthisisrandomstuff")]
        [InlineData(null, "hunter2", "pretendthisisrandomstuff")]
        [InlineData("John Miller", "", "pretendthisisrandomstuff")]
        [InlineData("John Miller", " ", "pretendthisisrandomstuff")]
        [InlineData("John Miller", null, "pretendthisisrandomstuff")]
        [InlineData("John Miller", "hunter2", "")]
        [InlineData("John Miller", "hunter2", " ")]
        [InlineData("John Miller", "hunter2", null)]
        [InlineData(null, null, null)]
        public void CreateUser_InvalidArguments(string username, string password, string salt)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            // Act
            User actualUser = userRepository.CreateUser(username, password, salt);

            // Assert
            User expectedUser = databaseContext.Users.FirstOrDefault(user => user.Username == username && user.Password == password && user.Salt == salt);

            Assert.Null(expectedUser);
            Assert.Null(actualUser);
        }

        [Theory]
        [InlineData("John Milli", null, null)]
        [InlineData(null, "hunter1", "newsaltwhodis")]
        public void UpdateUser_ValidArguments_SomeAlwaysNotNull(string username, string password, string salt)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            User actualUser = userRepository.UpdateUser(testUser.Id, username, password, salt);

            // Assert
            User expectedUser = databaseContext.Users.Find(testUser.Id);

            Assert.Equal(expectedUser, actualUser);

            // Cleanup 
            userRepository.UpdateUser(testUser.Id, "testUsername", null, null);
        }

        [Fact]
        public void UpdateUserRemainsUnchanged_ValidArguments_AllNull()
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();
            string username = null;
            string password = null;
            string salt = null;

            // Act
            User actualUser = userRepository.UpdateUser(testUser.Id, username, password, salt);

            // Assert
            User expectedUser = databaseContext.Users.Find(testUser.Id);

            Assert.Equal(expectedUser, actualUser);
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData(" ", " ", "testtest")]
        public void UpdateUserRemainsUnchanged_InvalidArguments(string username, string password, string salt)
        {
            // Arrange
            SOVAContext databaseContext = new SOVAContext(_connectionString);
            UserRepository userRepository = new UserRepository(databaseContext);

            User testUser = EnsureTestUserExistsThroughContext_ReturnsTestUser();

            // Act
            User actualUser = userRepository.UpdateUser(testUser.Id, username, password, salt);

            // Assert
            User expectedUser = databaseContext.Users.Find(testUser.Id);

            Assert.Equal(expectedUser, actualUser);
        }
    }
}
