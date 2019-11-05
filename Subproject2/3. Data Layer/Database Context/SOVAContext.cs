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
            var firstPart = "";
            var secondPart = "";
            var thirdPart = "";

            foreach (char c in str)
            {
                if (char.IsUpper(c))
                {
                    count++;
                    switch (count)
                    {
                        case 1:
                            firstPart += c;
                            break;
                        case 2:
                            secondPart += c;
                            break;
                        case 3:
                            thirdPart += c;
                            break;
                    }

                }
            }

            if (count > 1)
            {
                firstPart = firstPart + "_" + secondPart;
            }

            if(count > 2)
            {
                firstPart = firstPart + "_" + thirdPart;
            }

            return firstPart.ToLower();

        }

        public static void CreateMap(
            this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = ModifyString(entityType.GetTableName());                
                entityType.SetTableName(tableName);

                foreach (var property in entityType.GetProperties())
                {
                    var propertyName = ModifyString(property.Name);
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
