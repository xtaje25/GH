using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Models
{
    public class Models { }

    #region 分页
    public class PageCriteria
    {
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        private string _Fileds = "*";
        public string Fields
        {
            get { return _Fileds; }
            set { _Fileds = value; }
        }
        private string _PrimaryKey = "ID";
        public string PrimaryKey
        {
            get { return _PrimaryKey; }
            set { _PrimaryKey = value; }
        }
        private int _PageSize = 10;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }
        private string _Sort = string.Empty;
        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        private string _Condition = string.Empty;
        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        private int _RecordCount;
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; }
        }
    }
    #endregion

    public class DWZJson
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string navTabId { get; set; }
        public string rel { get; set; }
        public string callbackType { get; set; }
        public string forwardUrl { get; set; }
        public string confirmMsg { get; set; }
    }

    public enum DWZStatusCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        OK = 200,

        /// <summary>
        /// 操作失败
        /// </summary>
        ERROR = 300,

        /// <summary>
        /// session超时
        /// </summary>
        TIMEOUT = 301,
    }

    #region mh
    public class Tab_Auth_Menu_Relation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_AuthId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_MenuId { get; set; }
    }

    public class Tab_Authorization
    {
        [Key]
        public int F_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string F_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string F_AuthType { get; set; }

        public DateTime F_CreateDate { get; set; }
    }

    public class Tab_Menu
    {
        [Key]
        public int F_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string F_Name { get; set; }

        [Required]
        [StringLength(500)]
        public string F_URL { get; set; }

        public int F_ParentId { get; set; }

        public int F_Site { get; set; }
    }

    public class Tab_Role
    {
        [Key]
        public int F_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string F_Name { get; set; }
    }

    public class Tab_Role_Auth_Relation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_RoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_AuthId { get; set; }
    }

    public class Tab_User
    {
        [Key]
        public int F_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string F_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string F_Password { get; set; }

        public DateTime F_CreateDate { get; set; }

        /// <summary>
        /// 自定义        
        /// </summary>
        public int GZHId { get; set; }

        /// <summary>
        /// 自定义        
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string GZHName { get; set; }
    }

    public class Tab_User_Role_Relation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_RoleId { get; set; }
    }

    public class Tab_GongZhongHao
    {
        [Key]
        public int F_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string F_GZHName { get; set; }

        [Required]
        [StringLength(200)]
        public string F_WXName { get; set; }

        [StringLength(1000)]
        public string F_Logo { get; set; }

        [StringLength(4000)]
        public string F_About { get; set; }

        public DateTime F_CreateDate { get; set; }
    }

    public class Tab_MHCatalog
    {
        [Key]
        public int F_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string F_Catalog { get; set; }

        [StringLength(1000)]
        public string F_Logo { get; set; }

        public int F_GZHId { get; set; }

        public int F_CreateUser { get; set; }

        public DateTime F_CreateDate { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string GZHName { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string userName { get; set; }
    }

    public class Tab_MHSale
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_SaleType { get; set; }

        public int F_Price { get; set; }

        public DateTime F_CreateDate { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string GZHName { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public int TypeValue { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public int SaleType { get; set; }
    }

    public class Tab_SaleType
    {
        [Key]
        public int F_Id { get; set; }

        public int F_Type { get; set; }

        public int F_TypeValue { get; set; }
    }

    public class Tab_User_GZH_Relation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int F_GZHId { get; set; }

        /// <summary>
        /// 自定义
        /// </summary>
        public string name { get; set; }
    }
    #endregion
}