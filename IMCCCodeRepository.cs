using MerchantDataAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDataAPI.DataAccess
{
    public interface IMCCCodeRepository
    {
        Task<List<MerchantCategoryCode>> SearchMCCByCode(string mccCode);
        Task<List<MerchantCategoryCode>> SearchMCCByDescription(string description);
        Task<List<MerchantCategoryCode>> SearchMCCByDescriptionAndType(string description, string type);
        Task<List<MerchantCategoryCode>> SearchMCCByType(string type);
    }
}
