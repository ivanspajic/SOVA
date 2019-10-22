using System.Collections.Generic;

namespace RDJTPServer.Helpers
{
    public class InMemoryDb
    {
        public class Category
        {
            public int Cid;
            public string Name;
        }

        public List<Category> Categories = new List<Category>();

        public InMemoryDb()
        {
            Categories.Add(new Category
            {
                Cid = 1,
                Name = "Beverages"
            });
            Categories.Add(
                new Category
                {
                    Cid = 2,
                    Name = "Condiments"
                });
            Categories.Add(
                new Category
                {
                    Cid = 3,
                    Name = "Confections"
                });
        }
    }
}