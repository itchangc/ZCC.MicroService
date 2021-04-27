using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Storage.MicroService.Common;
using Storage.MicroService.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCC.MicroService.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [Route("decrease/{productId}/{count}")]
        [HttpGet]
        public ApiResponse DecreaseStock(long productId, int count)
        {
            try
            {
                _storageService.DecreaseStock(productId, count);
            }
            catch (Exception)
            {
                return ApiResponse.Fail("扣减库存失败");
            }
            return ApiResponse.OK();
        }
    }
}
