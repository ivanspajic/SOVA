using Microsoft.EntityFrameworkCore;
using Models;

namespace Data_Layer.Database_Context
{
    static class ModelBuilderExtensions
    {
        public static string ModifyString(string str)
        {
            var count = 0;
            var parts = new string[4];

            foreach (var c in str)
            {
                if (char.IsUpper(c))
                {
                    count++;
                }
                parts[count - 1] += c;
            }

            var result = parts[0];

            for (var i = 1; i < count; i++)
            {
                if (parts[i] == "")
                    break;
                result = result + "_" + parts[i];
            }

            return result.ToLower().Replace("ı", "i");
        }

        public static void CreateMap(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = ModifyString(entityType.GetTableName());
                entityType.SetTableName(tableName);

                foreach (var property in entityType.GetProperties())
                {
                    var propertyName = ModifyString(property.Name);

                    if (property.Name.Contains("String"))
                    {
                        propertyName = property.Name.Substring(0, property.Name.Length - "String".Length).ToLower();
                    }

                    property.SetColumnName(propertyName);
                }
            }
        }
    }

    public class SOVAContext : DbContext
    {
        private readonly string _connectionString;
        public SOVAContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<SearchResult> SearchResults { get; set; }
        public DbSet<CloudElement> CloudElements { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<LinkPost> LinkPosts { get; set; }
        public DbSet<Marking> Markings { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionsTag> QuestionsTags { get; set; }
        public DbSet<SoMember> SoMembers { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserHistory> UserHistory { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.CreateMap();

            modelBuilder.Entity<SearchResult>().HasNoKey().Property(x => x.Id).HasColumnName("postid");
            modelBuilder.Entity<CloudElement>().HasNoKey().Property(x => x.Text).HasColumnName("word");

            modelBuilder.Entity<Annotation>().HasKey(a => new { a.SubmissionId, a.UserId });
            modelBuilder.Entity<Question>().HasKey(q => q.SubmissionId);
            modelBuilder.Entity<Answer>().HasKey(a => a.SubmissionId);
            modelBuilder.Entity<Marking>().HasKey(m => new { m.UserId, m.SubmissionId });
            modelBuilder.Entity<QuestionsTag>().HasKey(q => new { q.QuestionId, q.TagId });
            modelBuilder.Entity<UserHistory>().HasKey(uh => new { uh.UserId, uh.HistoryId });
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Marking>().HasOne(m => m.Question).WithMany().HasForeignKey(m => m.SubmissionId);
            modelBuilder.Entity<Annotation>().HasOne(a => a.Question).WithMany().HasForeignKey(a => a.SubmissionId);
            modelBuilder.Entity<Question>().HasOne(q => q.Submission).WithMany().HasForeignKey(q => q.SubmissionId);
            modelBuilder.Entity<Answer>().HasOne(a => a.Question).WithMany(a => a.Answers).HasForeignKey(a => a.ParentId);
            modelBuilder.Entity<Answer>().HasOne(a => a.Submission).WithMany().HasForeignKey(a => a.SubmissionId);
            modelBuilder.Entity<Comment>().HasOne(c => c.ParentSubmission).WithMany(c => c.Comments).HasForeignKey(c => c.SubmissionId);
            modelBuilder.Entity<Comment>().HasOne(c => c.Submission).WithMany().HasForeignKey(c => c.Id);
            modelBuilder.Entity<LinkPost>().HasOne(l => l.Question).WithMany(l => l.LinkedPosts).HasForeignKey(l => l.QuestionId);
            modelBuilder.Entity<LinkPost>().HasOne(l => l.LinkedPost).WithMany().HasForeignKey(l => l.LinkPostId);
            modelBuilder.Entity<Question>().HasOne(q => q.Submission).WithMany().HasForeignKey(q => q.SubmissionId);
            modelBuilder.Entity<QuestionsTag>().HasOne(a => a.Question).WithMany(a => a.QuestionsTags).HasForeignKey(a => a.QuestionId);
            modelBuilder.Entity<QuestionsTag>().HasOne(a => a.Tag).WithMany().HasForeignKey(a => a.TagId);
            modelBuilder.Entity<Submission>().HasOne(a => a.SoMember).WithMany().HasForeignKey(a => a.SoMemberId);
            modelBuilder.Entity<UserHistory>().HasOne(a => a.History).WithMany().HasForeignKey(a => a.HistoryId);
        }
    }
}
