using api_stoke.Domain.Entities;
using System.Runtime.CompilerServices;

namespace api_stoke.Domain.Interfaces
{
    public interface IPanelRepositories
    {
        Task<Panel> Create(Panel panel);
        Task<Panel?> Get(Guid userId);

        Task Update(Panel panel);
    }
}
