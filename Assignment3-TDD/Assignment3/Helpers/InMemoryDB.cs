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

        public List<Category> Categories;

        public InMemoryDb()
        {
            Categories = new List<Category>
            {
                new Category
                {
                    Cid = 1,
                    Name = "Beverages"
                },
                new Category
                {
                    Cid = 2,
                    Name = "Condiments"
                },
                new Category
                {
                    Cid = 3,
                    Name = "Confections"
                }
            };
        }
    }

}