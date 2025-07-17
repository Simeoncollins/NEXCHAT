using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.ReactionManagement.Interfaces
{
    public interface IGetReactionsUseCase
    {
        Task<IEnumerable<Reaction>> ExecuteAsync();
    }
}