using BsbSearch.Models;
using System.Collections.Generic;

namespace BsbSearch.Test.Builders
{
    public class PartnerBuilder
    {
        private List<Partner> partners = new List<Partner>();

        public PartnerBuilder AllPartners()
        {
            partners.Add(new Partner("Team1", "url1", "Key1"));
            partners.Add(new Partner("Team2", "url2", "Key2"));
            return this;
        }
        public List<Partner> Build()
        {
            return partners;
        }

    }
}
