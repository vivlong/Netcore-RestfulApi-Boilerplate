using ServiceStack;
using System.Net;

namespace Api.ServiceModel
{
				[Api("Service Description")]
				[Tag("Core Requests")]
				[ApiResponse(HttpStatusCode.BadRequest, "Your request was not understood")]
				[ApiResponse(HttpStatusCode.InternalServerError, "Oops, something broke")]
				[Route("/hello", Summary = @"Default hello service.", Notes = "Longer description for hello service.")]
				[Route("/hello/{Name}")]

    public class Hello : IReturn<HelloResponse>
    {
								[ApiMember(Name = "Name", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
								[ApiAllowableValues("Name", Values = new string[] { "Genres", "Releases", "Contributors" })]
								public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
}
