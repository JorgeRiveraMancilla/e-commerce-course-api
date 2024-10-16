using e_commerce_course_api.DTOs;

namespace e_commerce_course_api.Interfaces
{
    /// <summary>
    /// The address repository.
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Gets the address by identifier.
        /// </summary>
        /// <param name="userId">
        /// The identifier.
        /// </param>
        /// <returns>
        /// The address data transfer object if found; otherwise, null.
        /// </returns>
        /// <exception cref="Exception">
        /// The user is not found.
        /// </exception>
        Task<AddressDto?> GetAddressByUserIdAsync(int userId);
    }
}
