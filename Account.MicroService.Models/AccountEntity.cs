using System;

namespace Account.MicroService.Models
{
    public partial class AccountEntity
    {
        /// <summary>
        /// 账户ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 总计金额
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// 使用金额
        /// </summary>
        public decimal? Used { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal? Residue { get; set; }
    }
}
