using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MusicDistro.Api.Utils.Hateoas
{
    [ApiController]
    public class HateoasBaseController : ControllerBase
    {
        internal MethodNames MethodNames { get; }

        public void BuildLinksForObject<T>(T dto, object value) where T : HateoasDto
        {
            // initialize the links
            var resourceLinks = new List<Link>();

            var typeName = typeof(T).Name;
            if (typeName.ToLower().Contains("dto"))
            {
                typeName = typeName.Substring(0, typeName.ToLower().IndexOf("dto"));
            }

            // populate GET method
            var getLink = new Link
            {
                Method = Methods.GET.ToString(),
                Href = Url.Link(MethodNames.GetMethod, value),
                Rel = "Self"
            };

            if (getLink.Href != null)
            {
                resourceLinks.Add(getLink);
            }


            var postLink = new Link
            {
                Method = Methods.POST.ToString(),
                Href = Url.Link(MethodNames.PostMethod, value),
                Rel = $"Add {typeName}"
            };

            if (postLink.Href != null)
            {
                resourceLinks.Add(postLink);
            }

            var updatedLink = new Link
            {
                Method = Methods.PUT.ToString(),
                Href = Url.Link(MethodNames.PutMethod, value),
                Rel = $"Update {typeName}"
            };

            if (updatedLink.Href != null)
            {
                resourceLinks.Add(updatedLink);
            }
            

            var deleteLink = new Link
            {
                Method = Methods.DELETE.ToString(),
                Href = Url.Link(MethodNames.DeleteMethod, value),
                Rel = $"Delete {typeName}"
            };

            if (deleteLink.Href != null)
            {
                resourceLinks.Add(deleteLink);
            }


            // Populate links
            dto.ResourceLinks = resourceLinks;


        }
    }

    public enum Methods
    {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE
    }
}
