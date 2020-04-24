using MerchantDataAPI.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDataAPI.DataAccess
{
    public class MCCCodeFlatFileRepository : IMCCCodeRepository
    {
        private readonly List<MerchantCategoryCode> _merchantCodes;
        private readonly IConfiguration _iConfig;

        public MCCCodeFlatFileRepository(IConfiguration iConfig)
        {
            _iConfig = iConfig;
            
            var path = Path.Combine(Environment.CurrentDirectory, _iConfig.GetSection("DBFlatFilePath").Value);
            var myJsonString = File.ReadAllText(path);
            _merchantCodes = JsonConvert.DeserializeObject<List<MerchantCategoryCode>>(myJsonString);
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByCode(string mccCode)
        {
            return await Task.Run(() => _merchantCodes.Where(x => x.MCCCode == mccCode).ToList());
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByDescription(string mccdescription)
        {
            string _mccdescription = mccdescription.ToUpper();
            return await Task.Run(() => _merchantCodes.Where(x => x.MCCCode.ToUpper().Contains(_mccdescription)
                                                           || x.MCCName.ToUpper().Contains(_mccdescription)
                                                           || x.Keywords.ToUpper().Contains(_mccdescription)
                                                           || x.MCCDescription.ToUpper().Contains(_mccdescription))
                                   .Select(x => x).ToList());
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByDescriptionAndType(string mccdescription, string transProcType)
        {
            string _mccdescription = mccdescription.ToUpper();
            return await Task.Run(()=>_merchantCodes.Where(x => (x.MCCCode.ToUpper().Contains(_mccdescription)
                                                           || x.MCCName.ToUpper().Contains(_mccdescription)
                                                           || x.Keywords.ToUpper().Contains(_mccdescription)
                                                           || x.MCCDescription.ToUpper().Contains(_mccdescription))
                                                           && x.MarkedforB2B.Equals(transProcType)).ToList());
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByType(string transProcType)
        {
            return await Task.Run(() => _merchantCodes.Where(x => x.MarkedforB2B == transProcType).ToList());
        }
    }
}
