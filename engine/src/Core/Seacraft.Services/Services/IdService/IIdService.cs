using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Services
{

    public interface IIdService
    {

        Task<long> GetDistributedIdAsync(CancellationToken? token = null);

    }

}