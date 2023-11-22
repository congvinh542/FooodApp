﻿using ClickBuy_Api.Database.Common;

namespace ClickBuy_Api.DTOs.Queries.Base
{
    public class BaseView
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public string? Code { get; set; } = HelperCommon.GenerateCode(5, "#");
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? CreatedBy { get; set; } = "System";
        public string? UpdatedBy { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public bool? IsDefault { get; set; } = true;
    }
}
