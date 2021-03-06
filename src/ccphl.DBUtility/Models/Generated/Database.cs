



















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `mysql`
//     Provider:               `MySql.Data.MySqlClient`
//     Connection String:      `server=192.168.1.209;database=db_ccphl;user id=root;password=**zapped**;Charset=utf8`
//     Schema:                 ``
//     Include Views:          `False`



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace Model
{

	public partial class MySqlHelper : Database
	{
		public MySqlHelper() 
			: base("mysql")
		{
			CommonConstruct();
		}

		public MySqlHelper(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			MySqlHelper GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static MySqlHelper GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new MySqlHelper();
        }

		[ThreadStatic] static MySqlHelper _instance;
		
		public override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		public override void OnEndTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        

		public class Record<T> where T:new()
		{
			public static MySqlHelper repo { get { return MySqlHelper.GetInstance(); } }
			public bool IsNew() { return repo.IsNew(this); }
			public object Insert() { return repo.Insert(this); }

			public void Save() { repo.Save(this); }
			public int Update() { return repo.Update(this); }

			public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
			public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }
			public static int Update(Sql sql) { return repo.Update<T>(sql); }
			public int Delete() { return repo.Delete(this); }
			public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }
			public static int Delete(Sql sql) { return repo.Delete<T>(sql); }
			public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }
			public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }
			public static bool Exists(string sql, params object[] args) { return repo.Exists<T>(sql, args); }
			public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }
			public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }
			public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }
			public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }
			public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }
			public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }
			public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }
			public static T Single(Sql sql) { return repo.Single<T>(sql); }
			public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }
			public static T First(Sql sql) { return repo.First<T>(sql); }
			public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }
			public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }
			public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }
			public static List<T> Fetch(long page, long itemsPerPage, Sql sql) { return repo.Fetch<T>(page, itemsPerPage, sql); }
			public static List<T> SkipTake(long skip, long take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }
			public static List<T> SkipTake(long skip, long take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }
			public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }
			public static Page<T> Page(long page, long itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }
			public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }
			public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }

		}

	}
	



    
	[TableName("ccphl_ads")]


	[PrimaryKey("AdsID")]



	[ExplicitColumns]
    public partial class ad : MySqlHelper.Record<ad>  
    {



		[Column] public int AdsID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? AdsPositionID { get; set; }





		[Column] public string AdsName { get; set; }





		[Column] public string LinkUrl { get; set; }





		[Column] public string ImagePath { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public DateTime? StartTime { get; set; }





		[Column] public DateTime? EndTime { get; set; }





		[Column] public int? Click { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_adsposition")]


	[PrimaryKey("AdsPositionID")]



	[ExplicitColumns]
    public partial class adsposition : MySqlHelper.Record<adsposition>  
    {



		[Column] public int AdsPositionID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string PositionNode { get; set; }





		[Column] public int? PositionLayer { get; set; }





		[Column] public string PositionCode { get; set; }





		[Column] public string PositionName { get; set; }





		[Column] public int? ParentID { get; set; }





		[Column] public string Description { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_article")]


	[PrimaryKey("ArticleID")]



	[ExplicitColumns]
    public partial class article : MySqlHelper.Record<article>  
    {



		[Column] public long ArticleID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public int? ColumnID { get; set; }





		[Column] public long? ContentID { get; set; }





		[Column] public string ArticleCode { get; set; }





		[Column] public string ArticleName { get; set; }





		[Column] public string ArticleSubName { get; set; }





		[Column] public string ArticleStyle { get; set; }





		[Column] public string ArticleSource { get; set; }





		[Column] public string ArticleAuthor { get; set; }





		[Column] public string LinkOutUrl { get; set; }





		[Column] public string HtmlLinkUrl { get; set; }





		[Column] public string ThumbPath { get; set; }





		[Column] public string AudioPath { get; set; }





		[Column] public string Template { get; set; }





		[Column] public string VideoPath { get; set; }





		[Column] public string Keywords { get; set; }





		[Column] public string Description { get; set; }





		[Column] public string SEO_Title { get; set; }





		[Column] public string SEO_Keywords { get; set; }





		[Column] public string SEO_Description { get; set; }





		[Column] public int? Click { get; set; }





		[Column] public int? Comment { get; set; }





		[Column] public sbyte? IsCom { get; set; }





		[Column] public sbyte? IsTop { get; set; }





		[Column] public sbyte? IsRed { get; set; }





		[Column] public sbyte? IsHot { get; set; }





		[Column] public sbyte? IsPost { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public sbyte? IsDelete { get; set; }





		[Column] public int? AddUserID { get; set; }





		[Column] public int? UpdateUserID { get; set; }





		[Column] public DateTime? AddTime { get; set; }





		[Column] public DateTime? PostTime { get; set; }





		[Column] public DateTime? UpdateTime { get; set; }



	}

    
	[TableName("ccphl_article_attach")]


	[PrimaryKey("AttachID")]



	[ExplicitColumns]
    public partial class article_attach : MySqlHelper.Record<article_attach>  
    {



		[Column] public long AttachID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public long? ArticleID { get; set; }





		[Column] public string FileName { get; set; }





		[Column] public string FilePath { get; set; }





		[Column] public int? FileSize { get; set; }





		[Column] public string FileExt { get; set; }





		[Column] public int? Point { get; set; }





		[Column] public int? Click { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_article_content")]


	[PrimaryKey("ContentID")]



	[ExplicitColumns]
    public partial class article_content : MySqlHelper.Record<article_content>  
    {



		[Column] public long ContentID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public string Text { get; set; }



	}

    
	[TableName("ccphl_article_image")]


	[PrimaryKey("ImageID")]



	[ExplicitColumns]
    public partial class article_image : MySqlHelper.Record<article_image>  
    {



		[Column] public long ImageID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public long? ArticleID { get; set; }





		[Column] public string ThumbnailUrl { get; set; }





		[Column] public string ImageUrl { get; set; }





		[Column] public string Description { get; set; }





		[Column] public sbyte? IsSingle { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_channel")]


	[PrimaryKey("ChannelID")]



	[ExplicitColumns]
    public partial class channel : MySqlHelper.Record<channel>  
    {



		[Column] public int ChannelID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string ChannelCode { get; set; }





		[Column] public string ChannelName { get; set; }





		[Column] public string LinkOutUrl { get; set; }





		[Column] public string ThumbPath { get; set; }





		[Column] public sbyte? PageType { get; set; }





		[Column] public string IndexTemplate { get; set; }





		[Column] public string ListTemplate { get; set; }





		[Column] public string DetailTemplate { get; set; }





		[Column] public string Keywords { get; set; }





		[Column] public string Description { get; set; }





		[Column] public string SEO_Title { get; set; }





		[Column] public string SEO_Keywords { get; set; }





		[Column] public string SEO_Description { get; set; }





		[Column] public sbyte? IsAlbums { get; set; }





		[Column] public sbyte? IsAttach { get; set; }





		[Column] public sbyte? IsEdit { get; set; }





		[Column] public sbyte? IsSystem { get; set; }





		[Column] public int? PageSize { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_column")]


	[PrimaryKey("ColumnID")]



	[ExplicitColumns]
    public partial class column : MySqlHelper.Record<column>  
    {



		[Column] public int ColumnID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public string ColumnNode { get; set; }





		[Column] public int? ColumnLayer { get; set; }





		[Column] public string ColumnCode { get; set; }





		[Column] public string ColumnName { get; set; }





		[Column] public int? ParentID { get; set; }





		[Column] public sbyte? PageType { get; set; }





		[Column] public string LinkOutUrl { get; set; }





		[Column] public string HtmlLinkUrl { get; set; }





		[Column] public string ThumbPath { get; set; }





		[Column] public string IndexTemplate { get; set; }





		[Column] public string ListTemplate { get; set; }





		[Column] public string DetailTemplate { get; set; }





		[Column] public string Keywords { get; set; }





		[Column] public string Description { get; set; }





		[Column] public string SEO_Title { get; set; }





		[Column] public string SEO_Keywords { get; set; }





		[Column] public string SEO_Description { get; set; }





		[Column] public sbyte? IsAlbums { get; set; }





		[Column] public sbyte? IsAttach { get; set; }





		[Column] public sbyte? IsEdit { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_comment")]


	[PrimaryKey("CommentID")]



	[ExplicitColumns]
    public partial class comment : MySqlHelper.Record<comment>  
    {



		[Column] public long CommentID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public int? CommentType { get; set; }





		[Column] public long? ArticleID { get; set; }





		[Column] public string ArticleTitle { get; set; }





		[Column] public string NickName { get; set; }





		[Column] public string IPAddress { get; set; }





		[Column] public string Text { get; set; }





		[Column] public sbyte? IsReply { get; set; }





		[Column] public string ReplyText { get; set; }





		[Column] public DateTime? ReplyTime { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_download")]


	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class download : MySqlHelper.Record<download>  
    {



		[Column] public int ID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public string Thumb { get; set; }





		[Column] public string Description { get; set; }





		[Column] public string FileName { get; set; }





		[Column] public string FilePath { get; set; }





		[Column] public int? FileSize { get; set; }





		[Column] public string FileExt { get; set; }





		[Column] public int? Click { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_link")]


	[PrimaryKey("LinkID")]



	[ExplicitColumns]
    public partial class link : MySqlHelper.Record<link>  
    {



		[Column] public int LinkID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public string LinkName { get; set; }





		[Column] public string LinkUrl { get; set; }





		[Column] public string ThumbPath { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public int? Click { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_message")]


	[PrimaryKey("MessageID")]



	[ExplicitColumns]
    public partial class message : MySqlHelper.Record<message>  
    {



		[Column] public long MessageID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public int? MessageType { get; set; }





		[Column] public string IPAddress { get; set; }





		[Column] public string Text { get; set; }





		[Column] public sbyte? IsReply { get; set; }





		[Column] public string ReplyText { get; set; }





		[Column] public DateTime? ReplyTime { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_region")]


	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class region : MySqlHelper.Record<region>  
    {



		[Column] public int ID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public string Node { get; set; }





		[Column] public int? Layer { get; set; }





		[Column] public int? ParentID { get; set; }





		[Column] public string RegionCode { get; set; }





		[Column] public string RegionName { get; set; }





		[Column] public string RegionUrl { get; set; }





		[Column] public string DepartmentCode { get; set; }





		[Column] public string OrganCode { get; set; }





		[Column] public string WeatherCode { get; set; }





		[Column] public string ClientIP { get; set; }





		[Column] public string Longitude { get; set; }





		[Column] public string Latitude { get; set; }





		[Column] public string Icon { get; set; }





		[Column] public string ImageUrl { get; set; }





		[Column] public string DistanceEducationUrl { get; set; }





		[Column] public string Remark { get; set; }





		[Column] public int? SortNo { get; set; }





		[Column] public int? IsOpen { get; set; }





		[Column] public int? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_site_channel")]


	[ExplicitColumns]
    public partial class site_channel : MySqlHelper.Record<site_channel>  
    {



		[Column] public int? ChannelID { get; set; }





		[Column] public int? SiteID { get; set; }



	}

    
	[TableName("ccphl_system_column_function")]


	[ExplicitColumns]
    public partial class system_column_function : MySqlHelper.Record<system_column_function>  
    {



		[Column] public int ColumnID { get; set; }





		[Column] public int FunctionID { get; set; }



	}

    
	[TableName("ccphl_system_dictionary")]


	[PrimaryKey("DictionaryID")]



	[ExplicitColumns]
    public partial class system_dictionary : MySqlHelper.Record<system_dictionary>  
    {



		[Column] public int DictionaryID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public string DictionaryNode { get; set; }





		[Column] public int? DictionaryLayer { get; set; }





		[Column] public string DictionaryCode { get; set; }





		[Column] public string DictionaryName { get; set; }





		[Column] public int? ParentID { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_system_function")]


	[PrimaryKey("FunctionID")]



	[ExplicitColumns]
    public partial class system_function : MySqlHelper.Record<system_function>  
    {



		[Column] public int FunctionID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public string FunctionName { get; set; }





		[Column] public string FunctionUrl { get; set; }





		[Column] public string Icon { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_system_log")]


	[PrimaryKey("LogID")]



	[ExplicitColumns]
    public partial class system_log : MySqlHelper.Record<system_log>  
    {



		[Column] public long LogID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public int? UserID { get; set; }





		[Column] public string UserName { get; set; }





		[Column] public string ClientHost { get; set; }





		[Column] public string OperateType { get; set; }





		[Column] public string RelatedEntity { get; set; }





		[Column] public string RelatedKey { get; set; }





		[Column] public string LogCotent { get; set; }





		[Column] public int? LogType { get; set; }





		[Column] public DateTime? OperatedTime { get; set; }



	}

    
	[TableName("ccphl_system_module")]


	[PrimaryKey("ModuleID")]



	[ExplicitColumns]
    public partial class system_module : MySqlHelper.Record<system_module>  
    {



		[Column] public int ModuleID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public string ModuleNode { get; set; }





		[Column] public string ModuleCode { get; set; }





		[Column] public int? ModuleLayer { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public int? ParentID { get; set; }





		[Column] public string ModuleName { get; set; }





		[Column] public string ModuleUrl { get; set; }





		[Column] public string ModuleKey { get; set; }





		[Column] public string Icon { get; set; }





		[Column] public long? ButtonAuthority { get; set; }





		[Column] public string Remark { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public sbyte? IsSystem { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_system_module_function")]


	[ExplicitColumns]
    public partial class system_module_function : MySqlHelper.Record<system_module_function>  
    {



		[Column] public int ModuleID { get; set; }





		[Column] public int FunctionID { get; set; }



	}

    
	[TableName("ccphl_system_role")]


	[PrimaryKey("RoleID")]



	[ExplicitColumns]
    public partial class system_role : MySqlHelper.Record<system_role>  
    {



		[Column] public int RoleID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public string RoleName { get; set; }





		[Column] public int? RoleType { get; set; }





		[Column] public string ThumbPath { get; set; }





		[Column] public long? DataAuthority { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_system_role_column_button")]


	[ExplicitColumns]
    public partial class system_role_column_button : MySqlHelper.Record<system_role_column_button>  
    {



		[Column] public int? RoleID { get; set; }





		[Column] public int? ColumnID { get; set; }





		[Column] public long? ButtonAuthority { get; set; }



	}

    
	[TableName("ccphl_system_role_column_function")]


	[ExplicitColumns]
    public partial class system_role_column_function : MySqlHelper.Record<system_role_column_function>  
    {



		[Column] public int RoleID { get; set; }





		[Column] public int ColumnID { get; set; }





		[Column] public int FunctionID { get; set; }



	}

    
	[TableName("ccphl_system_role_module_button")]


	[ExplicitColumns]
    public partial class system_role_module_button : MySqlHelper.Record<system_role_module_button>  
    {



		[Column] public int RoleID { get; set; }





		[Column] public int ModuleID { get; set; }





		[Column] public long? ButtonAuthority { get; set; }



	}

    
	[TableName("ccphl_system_role_module_function")]


	[ExplicitColumns]
    public partial class system_role_module_function : MySqlHelper.Record<system_role_module_function>  
    {



		[Column] public int? RoleID { get; set; }





		[Column] public int? ModuleID { get; set; }





		[Column] public int? FunctionID { get; set; }



	}

    
	[TableName("ccphl_system_role_user")]


	[ExplicitColumns]
    public partial class system_role_user : MySqlHelper.Record<system_role_user>  
    {



		[Column] public int RoleID { get; set; }





		[Column] public int UserID { get; set; }



	}

    
	[TableName("ccphl_system_site")]


	[PrimaryKey("SiteID")]



	[ExplicitColumns]
    public partial class system_site : MySqlHelper.Record<system_site>  
    {



		[Column] public int SiteID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public string SiteCode { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public int? SiteLayer { get; set; }





		[Column] public string SiteName { get; set; }





		[Column] public int? ParentID { get; set; }





		[Column] public string ICP { get; set; }





		[Column] public string Company { get; set; }





		[Column] public string Address { get; set; }





		[Column] public string Tel { get; set; }





		[Column] public string Fax { get; set; }





		[Column] public string Email { get; set; }





		[Column] public string CopyRight { get; set; }





		[Column] public string CloseReason { get; set; }





		[Column] public string CountCode { get; set; }





		[Column] public string ThumbPath { get; set; }





		[Column] public string DomainName { get; set; }





		[Column] public string DomainNames { get; set; }





		[Column] public string ManagePath { get; set; }





		[Column] public string TemplatePath { get; set; }





		[Column] public string HtmlPath { get; set; }





		[Column] public string IndexTemplate { get; set; }





		[Column] public string ListTemplate { get; set; }





		[Column] public string DetailTemplate { get; set; }





		[Column] public short? WaterType { get; set; }





		[Column] public short? WaterPos { get; set; }





		[Column] public int? WaterTransparency { get; set; }





		[Column] public string WaterFont { get; set; }





		[Column] public int? WaterFontSize { get; set; }





		[Column] public string WaterText { get; set; }





		[Column] public string WaterImgUrl { get; set; }





		[Column] public string SiteTitle { get; set; }





		[Column] public string SiteKeywords { get; set; }





		[Column] public string SiteDiscription { get; set; }





		[Column] public int? OrderBy { get; set; }





		[Column] public short? Status { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_system_user")]


	[PrimaryKey("UserID")]



	[ExplicitColumns]
    public partial class system_user : MySqlHelper.Record<system_user>  
    {



		[Column] public int UserID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public int? SiteID { get; set; }





		[Column] public string SiteNode { get; set; }





		[Column] public string UserName { get; set; }





		[Column] public string Password { get; set; }





		[Column] public string Salt { get; set; }





		[Column] public string RealName { get; set; }





		[Column] public string EnglishName { get; set; }





		[Column] public string Signature { get; set; }





		[Column] public string Introduction { get; set; }





		[Column] public string Photo { get; set; }





		[Column] public int? Sex { get; set; }





		[Column] public string IDCard { get; set; }





		[Column] public string Address { get; set; }





		[Column] public string Email { get; set; }





		[Column] public string Mobile { get; set; }





		[Column] public string QQ { get; set; }





		[Column] public int? DepartmentID { get; set; }





		[Column] public int? OrganID { get; set; }





		[Column] public string Position { get; set; }





		[Column] public int? Level { get; set; }





		[Column] public sbyte? IsAdmin { get; set; }





		[Column] public sbyte? Status { get; set; }





		[Column] public int? UserType { get; set; }





		[Column] public string CertificateID { get; set; }





		[Column] public string LastLoginIP { get; set; }





		[Column] public DateTime? LastLoginTime { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_system_user_column_button")]


	[ExplicitColumns]
    public partial class system_user_column_button : MySqlHelper.Record<system_user_column_button>  
    {



		[Column] public int UserID { get; set; }





		[Column] public int ColumnID { get; set; }





		[Column] public long ButtonAuthority { get; set; }





		[Column] public sbyte Status { get; set; }



	}

    
	[TableName("ccphl_system_user_column_function")]


	[ExplicitColumns]
    public partial class system_user_column_function : MySqlHelper.Record<system_user_column_function>  
    {



		[Column] public int UserID { get; set; }





		[Column] public int ColumnID { get; set; }





		[Column] public int FunctionID { get; set; }





		[Column] public sbyte Status { get; set; }



	}

    
	[TableName("ccphl_system_user_module_button")]


	[ExplicitColumns]
    public partial class system_user_module_button : MySqlHelper.Record<system_user_module_button>  
    {



		[Column] public int UserID { get; set; }





		[Column] public int ModuleID { get; set; }





		[Column] public long ButtonAuthority { get; set; }





		[Column] public sbyte Status { get; set; }



	}

    
	[TableName("ccphl_system_user_module_function")]


	[ExplicitColumns]
    public partial class system_user_module_function : MySqlHelper.Record<system_user_module_function>  
    {



		[Column] public int UserID { get; set; }





		[Column] public int ModuleID { get; set; }





		[Column] public int FunctionID { get; set; }





		[Column] public sbyte Status { get; set; }



	}

    
	[TableName("ccphl_system_web_column")]


	[ExplicitColumns]
    public partial class system_web_column : MySqlHelper.Record<system_web_column>  
    {



		[Column] public int? SiteID { get; set; }





		[Column] public int? RoleID { get; set; }





		[Column] public int? ChannelID { get; set; }





		[Column] public int? ColumnType { get; set; }





		[Column] public long? ButtonAuthority { get; set; }



	}

    
	[TableName("ccphl_t_browsers")]


	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class t_browser : MySqlHelper.Record<t_browser>  
    {



		[Column] public long ID { get; set; }





		[Column] public string UserID { get; set; }





		[Column] public string UserCode { get; set; }





		[Column] public string RegionCode { get; set; }





		[Column] public string PlatForms { get; set; }





		[Column] public string Browsers { get; set; }





		[Column] public string Ver { get; set; }





		[Column] public string Engines { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_t_browsers_all")]


	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class t_browsers_all : MySqlHelper.Record<t_browsers_all>  
    {



		[Column] public long ID { get; set; }





		[Column] public string RegionCode { get; set; }





		[Column] public string PlatForms { get; set; }





		[Column] public string Browsers { get; set; }





		[Column] public string Ver { get; set; }





		[Column] public string Engines { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("ccphl_t_user")]


	[PrimaryKey("ID")]



	[ExplicitColumns]
    public partial class t_user : MySqlHelper.Record<t_user>  
    {



		[Column] public long ID { get; set; }





		[Column] public string GUID { get; set; }





		[Column] public string RegionCode { get; set; }





		[Column] public string UserID { get; set; }





		[Column] public string UserCode { get; set; }





		[Column] public string UserName { get; set; }





		[Column] public string Mobile { get; set; }





		[Column] public string Password { get; set; }





		[Column] public int? Status { get; set; }





		[Column] public DateTime? UpdateTime { get; set; }





		[Column] public DateTime? AddTime { get; set; }



	}

    
	[TableName("regioninfo")]


	[PrimaryKey("RegionCode", autoIncrement=false)]

	[ExplicitColumns]
    public partial class regioninfo : MySqlHelper.Record<regioninfo>  
    {



		[Column] public string RegionCode { get; set; }





		[Column] public string RegionName { get; set; }





		[Column] public int? Level { get; set; }





		[Column] public string ApiUrl { get; set; }





		[Column] public string TvWebsiteUrl { get; set; }





		[Column] public short? IsEnable { get; set; }





		[Column] public string ActCode { get; set; }





		[Column] public string ORGCode { get; set; }





		[Column] public string DepartmentID { get; set; }





		[Column] public string ClassID { get; set; }





		[Column] public string DotNetApi { get; set; }





		[Column] public string JavaApi { get; set; }



	}


}



