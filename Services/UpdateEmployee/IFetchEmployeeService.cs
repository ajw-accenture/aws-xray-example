using System.Threading.Tasks;
using Shared;

namespace UpdateEmployee
{
    public interface IFetchEmployeeService {
        Task<Employee> ByPersonnelId(string id);
    }
}
