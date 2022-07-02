using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;

namespace DevInSales.Core.Interfaces
{
    public interface ICityService
    {
        List<ReadCity> GetAll(int stateId, string? name);
        ReadCity GetById(int cityId);

        void Add(City city);
    }
}