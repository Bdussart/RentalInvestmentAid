using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core
{
    public abstract class MustInitializeCache
    {
        protected CachingManager _cachingManager = null;
        public MustInitializeCache(CachingManager cachingManager){
            this._cachingManager = cachingManager;

        }
    }
}
