namespace RDJTPServer.Helpers
{
    public class DbOperations

    {
        InMemoryDb db = new InMemoryDb();

        public string GetAllCategories()
        {
            return db.Categories.ToJson();
        }

        public InMemoryDb.Category ReadCategory(int id)
        {
            var categoryInDb = db.Categories.Find(v => v.Cid == id);
            return categoryInDb;
        }

        public InMemoryDb.Category UpdateCategory(int id, string updateCategoryObject)
        {
            var categoryInDb = ReadCategory(id);
            if (categoryInDb == null)
            {
                return null;
            }
            categoryInDb.Name = updateCategoryObject.FromJson<InMemoryDb.Category>().Name;
            return categoryInDb;
        }

        public InMemoryDb.Category CreateCategory(string createCategoryObject)
        {
            var parsedCreateCategoryObject = createCategoryObject.FromJson<InMemoryDb.Category>();
            var nextAvailableId = 1;
            foreach (var item in db.Categories)
            {
                if (nextAvailableId != item.Cid) break;
                nextAvailableId++;
            };
            var categoryToCreate = new InMemoryDb.Category
            {
                Cid = nextAvailableId,
                Name = parsedCreateCategoryObject.Name
            };
            db.Categories.Add(categoryToCreate);
            return categoryToCreate;
        }

        public bool DeleteCategory(int idOfCategoryToDelete)
        {
            var categoryObjectToDelete = db.Categories.Find(v => v.Cid == idOfCategoryToDelete);
            if (categoryObjectToDelete == null)
            {
                return false;
            }
            db.Categories.Remove(categoryObjectToDelete);
            return true;
        }

    }
}