using System;
using System.Collections.Generic;
using System.Linq;

namespace RDJTPServer.Helpers
{
    public class Request
    {
        public string Method;
        public string Path;
        public string Date;
        public string Body;
    }
    public class Response
    {
        public string Status { get; set; }
        public string Body { get; set; }
    }

    public class Validation
    {
        public Response ValidateRequest(Request request)
        {
            var response = new Response();
            var errorResponse = new List<string>();
            var allowedMethods = new[] { "create", "read", "update", "delete", "echo", "testing" };
            const string pathPrefix = "/api/categories";

            if (request.Method == null)
            {
                errorResponse.Add("Missing method in header.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
            }

            if (request.Method != null && !allowedMethods.Contains(request.Method))
            {
                errorResponse.Add("Illegal method request provided.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
                return response;
            }

            if (request.Path == null)
            {
                if (request.Method == "delete")
                {
                    response.Status = $"{StatusCode.BadRequest}";
                }
                if (request.Method == "echo")
                {
                    response.Status = StatusCode.Ok;
                    return response;
                }

                errorResponse.Add("Missing resource in request header.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
            }

            if (request.Date == null)
            {
                errorResponse.Add("Missing date in request header.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
            }

            if (request.Date != null && request.Date != Utilities.UnixTimestamp())
            {
                errorResponse.Add("Illegal date in request header.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
                return response;
            }

            if (request.Method != "delete" && request.Method != "read" && request.Body == null)
            {
                Console.WriteLine("Am I here");
                errorResponse.Add("Missing body in header.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
                return response;
            }

            if (request.Body != null && !Utilities.IsValidJson(request.Body))
            {
                errorResponse.Add("Illegal body.");
                response.Status = $"{StatusCode.BadRequest}: {string.Join(", ", errorResponse)}";
                return response;
            }

            if (request.Path != null && !request.Path.StartsWith(pathPrefix))
            {
                response.Status = StatusCode.BadRequest;
                return response;
            }

            if (request.Path != null)
            {
                var pathSplit = request.Path.Split("/");    // Array looks like this: ["","api","categories"]
                switch (request.Method)
                {
                    case "read":
                        if (pathSplit.Length > 3)
                        {
                            var isInt = int.TryParse(pathSplit[3], out _);
                            response.Status = (isInt && request.Path.StartsWith(pathPrefix)) ? StatusCode.Ok : StatusCode.BadRequest;
                        }
                        else
                        {
                            response.Status = StatusCode.Ok;
                        }
                        return response;
                    case "create":
                        response.Status = (pathSplit.Length > 3)
                            ? response.Status = StatusCode.BadRequest
                            : StatusCode.Ok;
                        return response;
                    case "update":
                        response.Status = (pathSplit.Length <= 3)
                            ? response.Status = StatusCode.BadRequest
                            : StatusCode.Ok;
                        return response;
                    case "delete":
                        response.Status = (pathSplit.Length <= 3)
                            ? response.Status = StatusCode.BadRequest
                            : StatusCode.Ok;
                        return response;
                    default:
                        response.Status = StatusCode.BadRequest;
                        return response;
                }
            }



            return response;

        }

    }
}