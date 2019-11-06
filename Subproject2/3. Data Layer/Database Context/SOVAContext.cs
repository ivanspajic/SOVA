using System;
using System.Collections.Generic;
using System.Text;
using _0._Models;
using Microsoft.EntityFrameworkCore;

namespace _3._Data_Layer.Database_Context
{
    static class ModelBuilderExtensions
    {
        public static string ModifyString(string str)
        {
            var count = 0;
            string[] parts = { "", "", "", "" };

            foreach (char c in str)
            {
                if (char.IsUpper(c))
                {
                    count++;
                }
                parts[count - 1] += c;
            }

            var result = parts[0];

            for (int i = 1; i < count; i++)
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
        private const string ConnectionString = "host=localhost;db=northwind;uid=postgres;pwd=";
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
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.CreateMap();
        }
    }
}
