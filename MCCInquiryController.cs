using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantDataAPI.DataAccess;
using MerchantDataAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MerchantDataAPI.Controllers
{
    [Route("api/[controller]/V1/merchantcategorycode")]
    [ApiController]
    public class MCCInquiryController : ControllerBase
    {
        private readonly IMCCCodeRepository _imcccoderepository;
 
        public MCCInquiryController(IMCCCodeRepository imcccoderepository)
        {
            _imcccoderepository = imcccoderepository;
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<List<MerchantCategoryCodeResponse>> SearchMerchantCategoryCode([FromQuery] string mccCode, string mccDescription, string transProcType)
        {
            var mccResponse = new List<MerchantCategoryCodeResponse>() { };
            var message = new StatusMessage();

            List<MerchantCategoryCode> merchantCategoryCodes = new List<MerchantCategoryCode>();

            if (!string.IsNullOrEmpty(mccCode))
            {
                merchantCategoryCodes = await _imcccoderepository.SearchMCCByCode(mccCode);
                message = new StatusMessage() { MsgCode = "MO-101", MsgDescription = "Records Returned Successfully", MsgType = "Information", Service = "merchantcategorycode", Api = "SearchMerchantCategoryCode" };
            }
            else if (!string.IsNullOrEmpty(mccDescription) && !string.IsNullOrEmpty(transProcType))
            {
                merchantCategoryCodes = await _imcccoderepository.SearchMCCByDescriptionAndType(mccDescription, transProcType);
                message = new StatusMessage() { MsgCode = "MO-101", MsgDescription = "Records Returned Successfully", MsgType = "Information", Service = "merchantcategorycode", Api = "SearchMerchantCategoryCode" };
            }
            else if (!string.IsNullOrEmpty(mccDescription))
            {
                merchantCategoryCodes = await _imcccoderepository.SearchMCCByDescription(mccDescription);
                message = new StatusMessage() { MsgCode = "MO-101", MsgDescription = "Records Returned Successfully", MsgType = "Information", Service = "merchantcategorycode", Api = "SearchMerchantCategoryCode" };
            }
            else if (!string.IsNullOrEmpty(transProcType))
            {
                merchantCategoryCodes = await _imcccoderepository.SearchMCCByType(transProcType);
                message = new StatusMessage() { MsgCode = "MO-101", MsgDescription = "Records Returned Successfully", MsgType = "Information", Service = "merchantcategorycode", Api = "SearchMerchantCategoryCode" };
            }
            else
            {
                message = new StatusMessage() { MsgCode = "MO-102", MsgDescription = "The Request parameters or query did not match the data rules. Please try Again.", MsgType = "Information", Service = "merchantcategorycode", Api = "SearchMerchantCategoryCode" };
                mccResponse.Add(new MerchantCategoryCodeResponse { Result = merchantCategoryCodes, StatusMessage = message });
                return mccResponse;
            }

            if (merchantCategoryCodes.Count() <= 0)
            {
                message = new StatusMessage() { MsgCode = "MO-103", MsgDescription = "The Request information not found. Please try again; changing parameters for search.", MsgType = "Information", Service = "merchantcategorycode", Api = "SearchMerchantCategoryCode" };
            }

            mccResponse.Add(new MerchantCategoryCodeResponse { Result = merchantCategoryCodes, StatusMessage = message });

            return mccResponse;
        }
    }
}