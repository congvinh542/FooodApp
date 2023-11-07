using ClickBuy_Api.DTOs.Queries.Base;
using ClickBuy_Api.Service.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ClickBuy_Api.WebApp.Areas.Administrator.Controllers.Base
{
    // T: Service
    // T1: Query
    // T2: View
    // T3: Filter

    public class CRUDBaseControlle<T, T1, T2, T3> : Controller
    {
        protected readonly T _service;
        public CRUDBaseControlle(T service)
        {
            _service = service;
        }
        #region CRUD Base
        //  POST: Base
        [HttpPost]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilter<T3> queryFilter)
        {
            queryFilter.PageNumber = queryFilter.PageNumber > 0 ? queryFilter.PageNumber : 1;
            queryFilter.PageSize = queryFilter.PageSize > 0 ? queryFilter.PageSize : 10;

            var getMethod = _service.GetType().GetMethod("GetPageList");
            if (getMethod == null)
            {
                return Json(new { Message = "GetPageList method not found", Success = false });
            }

            var data = await (Task<DataResult<T2>>)getMethod.Invoke(_service, new object[] { queryFilter });

            queryFilter.TotalItems = data.TotalRecords.Value;

            return Ok(data);
        }

        // POST: Base/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] T1 query)
        {
            try
            {
                var method = _service.GetType().GetMethod("CreateAsync");
                if (method == null)
                {
                    return Json(new { Message = "CreateAsync method not found", Success = false });
                }
                var result = await (Task<DataResult<bool>>)method.Invoke(_service, new object[] { query });
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Json(new { Message = ex.Message, Success = false });
            }

        }

        [HttpPost]
        // POST: Base/Delete/5
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            var method = _service.GetType().GetMethod("DeleteAsync");
            if (method == null)
            {
                return Json(new { Message = "DeleteAsync method not found", Success = false });
            }
            var result = await (Task<DataResult<int>>)method.Invoke(_service, new object[] { id });
            return Ok(result);
        }

        [HttpPost]
        // POST: Base/Update/5
        public async Task<IActionResult> Update([FromBody] T1 query, string id)
        {
            var method = _service.GetType().GetMethod("UpdateAsync");
            if (method == null)
            {
                return BadRequest("UpdateAsync method not found");
            }
            var result = await (Task<DataResult<int>>)method.Invoke(_service, new object[] { query, id });
            return Ok(result);
        }

        [HttpPost]
        // POST: Base/GetByIdAync/5
        public async Task<IActionResult> GetById(string id)
        {
            var method = _service.GetType().GetMethod("GetByIdAsync");
            if (method == null)
            {
                return Json(new { Message = "GetByIdAsync method not found", Success = false });
            }
            var result = await (Task<DataResult<T2>>)method.Invoke(_service, new object[] { id });

            return Ok(result);
        }
        [HttpPost]
        // POST: Base/GetByCodeAsync/5
        public async Task<IActionResult> GetByCode(string code)
        {
            var method = _service.GetType().GetMethod("GetByCodeAsync");
            if (method == null)
            {
                return Json(new { Message = "GetByCodeAsync method not found", Success = false });
            }
            var result = await (Task<DataResult<T2>>)method.Invoke(_service, new object[] { code });

            return Ok(result);
        }
        #endregion

    }
}