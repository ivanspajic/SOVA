namespace RDJTPServer.Helpers
{
    public class HandleRequest
    {
        public Response Respond(Request request)
        {
            var response = new Response();
            var dbOperations = new DbOperations();
            InMemoryDb.Category result;
            switch (request.Method)
            {
                case "create":
                    var createdCategory = dbOperations.CreateCategory(request.Body);
                    response.Body = createdCategory.ToJson();
                    response.Status = StatusCode.Created;
                    return response;

                case "read":
                    if (request.Path.Split("/").Length == 3)
                    {
                        response.Body = dbOperations.GetAllCategories();
                    }
                    else
                    {
                        result = dbOperations.ReadCategory(Utilities.IdFromPath(request.Path));
                        if (result == null)
                        {
                            response.Status = StatusCode.NotFound;
                            return response;
                        }
                        response.Body = result.ToJson();
                    }
                    response.Status = StatusCode.Ok;
                    return response;

                case "update":
                    var updatedData = dbOperations.UpdateCategory(Utilities.IdFromPath(request.Path), request.Body);
                    if (updatedData == null)
                    {
                        response.Status = StatusCode.NotFound;
                        return response;
                    }
                    response.Body = updatedData.ToJson();
                    response.Status = StatusCode.Updated;
                    return response;

                case "delete":
                    var deleteOperation = dbOperations.DeleteCategory(Utilities.IdFromPath(request.Path));
                    if (deleteOperation != true)
                    {
                        response.Status = StatusCode.NotFound;
                        return response;
                    }
                    response.Status = StatusCode.Ok;
                    return response;

                case "echo":
                    response.Body = request.Body;
                    response.Status = StatusCode.Ok;
                    return response;

                default:
                    return response;
            }
        }
    }
}
