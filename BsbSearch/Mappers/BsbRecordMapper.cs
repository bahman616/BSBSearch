using BsbSearch.Models;
using BsbSearch.ViewModels;

namespace BsbSearch.Mappers
{
    public static class BsbRecordMapper
    {
        public static BsbRecordViewModel? Map(BsbRecord? bsbRecord) 
        {
            if (bsbRecord == null)
                return null;

            return new BsbRecordViewModel()
            { 
                Address = bsbRecord.Address,
                FICode = bsbRecord.FICode,
                Id = bsbRecord.Id,
                Name = bsbRecord.Name,  
                Number = bsbRecord.Number,
                PaymentSystems = bsbRecord.PaymentSystems,
                PostCode= bsbRecord.PostCode,
                State = bsbRecord.State,
                Suburb = bsbRecord.Suburb
            };
        }
    }
}
