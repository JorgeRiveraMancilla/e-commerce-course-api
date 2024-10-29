using CloudinaryDotNet.Actions;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The service for photos.
    /// </summary>
    public interface IPhotoService
    {
        /// <summary>
        /// Add an image to the cloudinary.
        /// </summary>
        /// <param name="file">
        /// The file to upload.
        /// </param>
        /// <returns>
        /// The result of the image upload.
        /// </returns>
        Task<ImageUploadResult> AddImageAsync(IFormFile file);

        /// <summary>
        /// Delete an image from the cloudinary.
        /// </summary>
        /// <param name="publicId">
        /// The public id of the image to delete.
        /// </param>
        /// <returns>
        /// The result of the image deletion.
        /// </returns>
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
