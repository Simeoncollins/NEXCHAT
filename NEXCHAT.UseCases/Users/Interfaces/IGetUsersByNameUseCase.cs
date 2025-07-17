using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.Users.Interfaces
{
    public interface IGetUsersByNameUseCase
    {
        Task<IEnumerable<User>> ExecuteAsync(string name = "", int pageIndex = 1, int pageSize = 50);
    }
}