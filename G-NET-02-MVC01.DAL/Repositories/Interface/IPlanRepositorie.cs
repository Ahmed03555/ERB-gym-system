using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using G_NET_02_MVC01.DAL.Entities;
namespace G_NET_02_MVC01.DAL.Repositories.Interface
{
    public interface IPlanRepositorie
    {
        Task<IEnumerable<Plans>> GetAll(bool isTracked ,CancellationToken ct= default);
        Task<Plans?> GetById(int id, CancellationToken ct = default);

        void Update(Plans plans);
        void Add(Plans plans);
        void Delete(Plans plans);
        Task<int> CompleteAsync();
    }
}
