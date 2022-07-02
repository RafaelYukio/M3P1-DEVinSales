using DevInSales.Core.Data.DTOs.ApiDTOs;

namespace DevInSales.Core.Interfaces
{
    public interface IStateService
    {
        List<ReadState> GetAll(string? name);
        ReadState GetById(int stateId);
    }
}