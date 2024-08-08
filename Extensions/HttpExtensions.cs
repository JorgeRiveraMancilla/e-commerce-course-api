using System.Text.Json;
using e_commerce_course_api.RequestHelpers;

namespace e_commerce_course_api.Extensions
{
    /// <summary>
    /// The extension methods for the HTTP classes.
    /// </summary>
    public static class HttpExtensions
    {
        /// <summary>
        /// The options for the JSON serializer.
        /// </summary>
        private static readonly JsonSerializerOptions _options =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        /// <summary>
        /// Adds the pagination header to the response.
        /// </summary>
        /// <param name="response">
        /// The response to add the header to.
        /// </param>
        /// <param name="metaData">
        /// The metadata to add to the header.
        /// </param>
        public static void AddPaginationHeader(this HttpResponse response, MetaData metaData)
        {
            response.Headers.Append("Pagination", JsonSerializer.Serialize(metaData, _options));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
