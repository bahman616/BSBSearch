using BsbSearch.Models;
using System.Collections.Generic;

namespace BsbSearch.Test.Builders
{
    // This is to build bsbRecords with different settings/parameters.
    public class BsbRecordBuilder
    {
        private List<BsbRecord> bsbRecords = new List<BsbRecord>();

        public BsbRecordBuilder AllRecordsWithAllFields()
        {
            bsbRecords.Add(new BsbRecord("1", "012002", "ANZ", "ANZ Smart Choice", "115 Pitt Street", "Sydney", "NSW", "2000", "PEH"));
            bsbRecords.Add(new BsbRecord("2", "985555", "HSB", "Hongkong & Shanghai Banking Aust", "Level 32  580 George Street", "Sydney", "NSW", "2000", "PEH"));
            bsbRecords.Add(new BsbRecord("4", "083710", "NAB", "CT 3352 Waverley BBC-BB3 I", "541 Blackburn Rd", "Mount Waverley", "VIC", "3149", "PEH"));
            return this;
        }
        public BsbRecordBuilder SingleRecordsWithAllFields()
        {
            bsbRecords.Add(new BsbRecord("1", "012002", "ANZ", "ANZ Smart Choice", "115 Pitt Street", "Sydney", "NSW", "2000", "PEH"));
            return this;
        }
        public List<BsbRecord> Build()
        {
            return bsbRecords;
        }

    }
}
