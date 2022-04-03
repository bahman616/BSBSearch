using BsbSearch.Models;
using System.Collections.Generic;

namespace BsbSearch.Test.Builders
{
    public class PartnerBuilder
    {
        private List<Partner> partners = new List<Partner>();

        public PartnerBuilder AllPartners()
        {
            partners.Add(new Partner("Team1", "http://url1.com", "Key1", false));
            partners.Add(new Partner("Team2", "http://url2.com", "Key2", false));
            return this;
        }
        public List<Partner> Build()
        {
            return partners;
        }

    }
}
