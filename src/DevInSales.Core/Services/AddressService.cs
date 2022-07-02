using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class AddressService : IAddressService
  {
    private readonly DataContext _context;
    public AddressService(DataContext context)
    {
      _context = context;
    }
    public List<ReadAddress> GetAll(int? stateId, int? cityId, string? street, string? cep)
    {
      var query = _context.Addresses
          .Include(a => a.City)
          .Include(a => a.City.State)
          .AsQueryable();
      if (cityId.HasValue)
        query = query.Where(x => x.CityId == cityId);
      if (stateId.HasValue)
        query = query.Where(x => x.City.StateId == stateId);
      if (!string.IsNullOrEmpty(street))
        query = query.Where(x => x.Street.ToUpper().Contains(street.ToUpper()));
      if (!string.IsNullOrEmpty(cep))
        query = query.Where(x => x.Cep == cep);
      return query.Select(x => ReadAddress.AddressToReadAddress(x)).ToList();
    }

    public void Add(Address address)
    {
      _context.Addresses.Add(address);
      _context.SaveChanges();
    }

    public Address? GetById(int addressId)
    {
      return _context.Addresses.FirstOrDefault(p => p.Id == addressId);
    }
    public void Delete(Address address)
    {
      _context.Addresses.Remove(address);
      _context.SaveChanges();
    }
    public void Update(Address address)
    {
      _context.Addresses.Update(address);
      _context.SaveChanges();
    }
  }
}