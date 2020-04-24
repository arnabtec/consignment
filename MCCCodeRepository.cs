using MerchantDataAPI.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantDataAPI.DataAccess
{
    public class MCCCodeRepository : IMCCCodeRepository
    {
        private readonly IMongoCollection<MerchantCategoryCode> _merchantCodes;

        public MCCCodeRepository(IMerchantDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            var collection = settings.CollectionName;

            _merchantCodes = database.GetCollection<MerchantCategoryCode>(collection);
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByCode(string mccCode)
        {
            return await _merchantCodes.Find<MerchantCategoryCode>(merchantcategorycode => merchantcategorycode.MCCCode == mccCode).ToListAsync();
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByDescription(string mccdescription)
        {
            var filter = new BsonDocument(){
                            {"$or", new BsonArray(){
                                    new BsonDocument(){{"MCCCode", new BsonRegularExpression(mccdescription, "i")}},
                                    new BsonDocument(){{"MCCName", new BsonRegularExpression(mccdescription, "i")}},
                                    new BsonDocument(){{"Keywords", new BsonRegularExpression(mccdescription, "i")}},
                                    new BsonDocument(){{"MCCDescription", new BsonRegularExpression(mccdescription, "i")} }}}
                            };

            return await _merchantCodes.Find<MerchantCategoryCode>(filter).ToListAsync();
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByDescriptionAndType(string mccdescription, string transProcType)
        {
            var filter = new BsonDocument(){
                            {"$and", new BsonArray(){new BsonDocument(){
                                {"$or", new BsonArray(){
                                    new BsonDocument(){{"MCCCode", new BsonRegularExpression(mccdescription, "i")}},
                                    new BsonDocument(){{"MCCName", new BsonRegularExpression(mccdescription, "i")}},
                                    new BsonDocument(){{"Keywords", new BsonRegularExpression(mccdescription, "i")}},
                                    new BsonDocument(){{"MCCDescription", new BsonRegularExpression(mccdescription, "i")} }}}
                            },new BsonDocument(){{"MarkedforB2B", transProcType } }}} };

            return await _merchantCodes.Find<MerchantCategoryCode>(filter).ToListAsync();
        }

        public async Task<List<MerchantCategoryCode>> SearchMCCByType(string transProcType)
        {
            return await _merchantCodes.Find<MerchantCategoryCode>(merchantcategorycode => merchantcategorycode.MarkedforB2B == transProcType).ToListAsync();
        }
    }
}
