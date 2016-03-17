using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using PetaPoco;
using System.Reflection;

namespace ccphl.DBUtility
{
    /// <summary>
    /// mysql操作辅助类
    /// </summary>
    public class MySqlUtility
    {
        private MySqlHelper db;
        /// <summary>
        /// DB链接初始化
        /// </summary>
        public MySqlUtility()
        {
            if (db == null)
                db = MySqlHelper.GetInstance();
        }

        #region 查询方法
        /// <summary>
        /// 判断主键是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool IsExists<T>(object id) where T : new()
        {
           return db.Exists<T>(id);
        }
        /// <summary>
        /// 判断where条件下是否存在数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool IsExists<T>(string where) where T : new()
        {
            if (!string.IsNullOrEmpty(where.Trim()))
                return db.Exists<T>(where);
            else
                return false;
        }
        /// <summary>
        /// 条件统计数据量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public long GetCount<T>(string where) where T : new()
        {
            Type t= typeof(T);
            string tablename = "";
            object[] nameattrs = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            TableNameAttribute nameattr = null;
            if (nameattrs.Length == 1)
            {
                nameattr = (TableNameAttribute)nameattrs[0];
                tablename = nameattr.Value;
            }
            if (!string.IsNullOrEmpty(where.Trim()))
                return db.ExecuteScalar<long>("SELECT COUNT(*) FROM " + tablename + " WHERE " + where);
            else
                return db.ExecuteScalar<long>("SELECT COUNT(*) FROM " + tablename);
        }
        /// <summary>
        /// 主键获取实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T GetModel<T>(object id)where T:new()
        {
            T t = new T();
            var tt = db.SingleOrDefault<T>(id);
            if (tt != null)
            {
                t = tt;
            }
            return t;
        }
        /// <summary>
        /// 按条件获取 一个实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public T GetModel<T>(string where) where T : new()
        {
            T t = new T();
            string sql = " LIMIT 1 ";
            if (!string.IsNullOrEmpty(where.Trim()))
            {
                sql = "WHERE " + where + " LIMIT 1 ";
            }
            var tt = db.SingleOrDefault<T>(sql);
            if (tt != null)
                t = tt;
            return t;
        }
        /// <summary>
        /// 按条件和排序规则获取一个实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">条件</param>
        /// <param name="order">排序规则</param>
        /// <returns></returns>
        public T GetModel<T>(string where,string order) where T : new()
        {
            T t = new T();
            string sql = " LIMIT 1 ";
            if (!string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + order + " LIMIT 1 ";
            }
            else if (string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = order + " LIMIT 1 ";
            }
            else if (!string.IsNullOrEmpty(where.Trim()) && string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + " LIMIT 1 ";
            }
            var tt = db.SingleOrDefault<T>(sql);
            if (tt != null)
                t = tt;
            return t;
        }
        /// <summary>
        /// 条件获取多条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<T> GetModelList<T>(string where) where T : new()
        {
            if (!string.IsNullOrEmpty(where.Trim()))
                return db.Fetch<T>("WHERE " + where);
            else
                return db.Fetch<T>("");
        }
        /// <summary>
        /// 按条件查询 前top条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="top">查询前top条数据</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<T> GetModelList<T>(int top,string where) where T : new()
        {
            if (!string.IsNullOrEmpty(where.Trim()))
                return db.Fetch<T>("WHERE " + where + " LIMIT "+top);
            else
                return db.Fetch<T>(" LIMIT " + top);
        }
        /// <summary>
        /// 条件获取 start后面的number条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="start">开始条数</param>
        /// <param name="number">查询的条数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<T> GetModelList<T>(int start,int number, string where) where T : new()
        {
            if (!string.IsNullOrEmpty(where.Trim()))
                return db.Fetch<T>("WHERE " + where + " LIMIT " + start + "," + number);
            else
                return db.Fetch<T>(" LIMIT " + start + "," + number);
        }
        /// <summary>
        /// 按条件和排序规则获取多条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">条件</param>
        /// <param name="order">排序规则</param>
        /// <returns></returns>
        public List<T> GetModelList<T>(string where,string order) where T : new()
        {
            string sql = "";
            if (!string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + order;
            }
            else if (string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql =order;
            }
            else if (!string.IsNullOrEmpty(where.Trim()) && string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where;
            }
            return db.Fetch<T>(sql);
        }
        /// <summary>
        /// 按条件和排序规则获取前top条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="top">数据条数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序规则</param>
        /// <returns></returns>
        public List<T> GetModelList<T>(int top, string where, string order) where T : new()
        {
            string sql = "";
            if (!string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + order + " LIMIT " + top;
            }
            else if (string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = order + " LIMIT " + top;
            }
            else if (!string.IsNullOrEmpty(where.Trim()) && string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + " LIMIT " + top;
            }
            return db.Fetch<T>(sql);
        }
        /// <summary>
        /// 按条件和排序规则获取 start后面的number条数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="start">开始条数</param>
        /// <param name="number">查询的条数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序规则</param>
        /// <returns></returns>
        public List<T> GetModelList<T>(int start, int number, string where, string order) where T : new()
        {
            string sql = "";
            if (!string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql="WHERE " + where + order + " LIMIT " + start + "," + number;
            }
            else if (string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql=order + " LIMIT " + start + "," + number;
            }
            else if (!string.IsNullOrEmpty(where.Trim()) && string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + " LIMIT " + start + "," + number;
            }
            return db.Fetch<T>(sql);
        }
        /// <summary>
        /// 分页查询 条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="page">当前页码</param>
        /// <param name="size">每页显示的数量</param>
        /// <param name="where">条件</param>
        /// <param name="count">查询数据总数</param>
        /// <returns></returns>
        public List<T> GetPageList<T>(int page, int size, string where, out long count) where T : new()
        {
            string sql = "";
            if (!string.IsNullOrEmpty(where.Trim()))
            {
                sql = "WHERE " + where;
            }
            PetaPoco.Page<T> pg = db.Page<T>(page, size, sql);
            count = pg.TotalItems;
            return pg.Items;
        }
        /// <summary>
        /// 分页查询 条件+排序规则
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="page">当前页码</param>
        /// <param name="size">每页显示的数量</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序规则</param>
        /// <param name="count">查询数据总数</param>
        /// <returns></returns>
        public List<T> GetPageList<T>(int page, int size, string where,string order, out long count) where T : new()
        {
            string sql = "";
            if (!string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where + " "+order;
            }
            else if (string.IsNullOrEmpty(where.Trim()) && !string.IsNullOrEmpty(order.Trim()))
            {
                sql = order;
            }
            else if (!string.IsNullOrEmpty(where.Trim()) && string.IsNullOrEmpty(order.Trim()))
            {
                sql = "WHERE " + where;
            }
            PetaPoco.Page<T> pg = db.Page<T>(page, size, sql);
            count = pg.TotalItems;
            return pg.Items;
        }
        #endregion 查询方法

        #region 插入方法
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public long Insert<T>(T entity) where T : new()
        {
            var objvalue = db.Insert(entity);
            return Convert.ToInt64(objvalue);
        }
        /// <summary>
        /// 插入树表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="nodefield">节点标示字段</param>
        /// <param name="layerfield">节点深度字段</param>
        /// <param name="parentfield">父级节点关联字段</param>
        /// <returns></returns>
        public long InsertTree<T>(T entity,string nodefield,string layerfield,string parentfield) where T : new()
        {
            Type type = typeof(T);
            string primarykey = "";
            object[] keyattrs = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            PrimaryKeyAttribute keyattr = null;
            if (keyattrs.Length == 1)
            {
                keyattr = (PrimaryKeyAttribute)keyattrs[0];
                primarykey = keyattr.Value;
            }
            var objvalue = db.Insert(entity);
            long id = Convert.ToInt64(objvalue);
            if (id > 0)
            {
                string nodefieldvalue = "";
                int layerfieldvalue = 1;
                int parentid = Convert.ToInt32(getProperty<T>(entity, parentfield));
                if (parentid > 0)
                {
                    var parentmodel = db.SingleOrDefault<T>(parentid);
                    if (parentmodel == null)
                    {
                        nodefieldvalue = "," + id + ",";
                        layerfieldvalue = 1;
                    }
                    else
                    {
                        nodefieldvalue = getProperty<T>(parentmodel, nodefield).ToString()+id+",";
                        layerfieldvalue = Convert.ToInt32(getProperty<T>(parentmodel, layerfield)) + 1;
                    }
                }
                else
                {
                    nodefieldvalue = "," +id + ",";
                    layerfieldvalue = 1;
                }
                db.Update<T>("SET " + nodefield + "=@0," + layerfield + "=@1 WHERE " + primarykey + "=@2", nodefieldvalue, layerfieldvalue, id);
            }
            return id;
        }
        /// <summary>
        /// 保存实体，主键值存在则更新，不存在则插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Save<T>(T entity) where T : new()
        {
            db.Save(entity);
        }
        #endregion 插入方法

        #region 更新方法
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Update<T>(T entity) where T : new()
        {
            return db.Update(entity);
        }
        /// <summary>
        /// 更新主键行的column字段值为value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键</param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long UpdateField<T>(object id, string column, object value) where T : new()
        {
            if (id == null)
            {
                return 0;
            }
            else if (id.ToString().Trim() == string.Empty)
            {
                return 0;
            }
            else if (Convert.ToInt64(id) == 0)
            {
                return 0;
            }
            if (string.IsNullOrEmpty(column.Trim()))
            {
                return 0;
            }
            Type type = typeof(T);
            string primarykey = "";
            object[] keyattrs = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            PrimaryKeyAttribute keyattr = null;
            if (keyattrs.Length == 1)
            {
                keyattr = (PrimaryKeyAttribute)keyattrs[0];
                primarykey = keyattr.Value;
            }
            if (string.IsNullOrEmpty(primarykey.Trim()))
            {
                return 0;
            }
            return db.Update<T>("SET " + column + "=@0 WHERE " + primarykey + "=@1", value, id);
        }
        /// <summary>
        /// 更新满足where条件的字段column的值为value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="column">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public long UpdateField<T>(string where, string column, object value)
        {
            if (string.IsNullOrEmpty(column.Trim()))
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(where.Trim()))
            {
                return db.Update<T>("SET " + column + "=@0 WHERE " + where, value);
            }
            else
            {
                return db.Update<T>("SET " + column + "=@0 ", value);
            }
        }
        /// <summary>
        /// 更新实体，值非null的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public long UpdateFields<T>(T entity) where T : new()
        {
            Type type = typeof(T);
            string primarykey = "";
            object[] keyattrs = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            PrimaryKeyAttribute keyattr = null;
            if (keyattrs.Length == 1)
            {
                keyattr = (PrimaryKeyAttribute)keyattrs[0];
                primarykey = keyattr.Value;
            }
            if (string.IsNullOrEmpty(primarykey))
            {
                return 0;
            }
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo p in ps)
            {
                object value = p.GetValue(entity, null);
                if (p.Name == primarykey)
                {
                    if (value == null)
                    {
                        return 0;
                    }
                    else if (value.ToString().Trim() == string.Empty)
                    {
                        return 0;
                    }
                    else if (Convert.ToInt64(value) == 0)
                    {
                        return 0;
                    }
                }
                if (value != null && p.Name != primarykey)
                {
                    sb.Append(p.Name);
                    sb.Append(",");
                }
            }
            string[] columns = sb.ToString().TrimEnd(',').Split(',');
            return db.Update(entity, columns);
        }
        /// <summary>
        /// 更新树表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">数据实体</param>
        /// <param name="nodefield">节点字段</param>
        /// <param name="layerfield">节点深度字段</param>
        /// <param name="parentfield">父几点关联字段</param>
        /// <returns></returns>
        public long UpdateTree<T>(T entity, string nodefield, string layerfield, string parentfield) where T : new()
        {
            try
            {
                Type type = typeof(T);
                string primarykey = "";
                object[] keyattrs = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                PrimaryKeyAttribute keyattr = null;
                if (keyattrs.Length == 1)
                {
                    keyattr = (PrimaryKeyAttribute)keyattrs[0];
                    primarykey = keyattr.Value;
                }
                //原数据
                int oid = Convert.ToInt32(getProperty<T>(entity, primarykey));
                T oldt = db.SingleOrDefault<T>(oid);
                //旧的父栏目ID
                int olsparentid = Convert.ToInt32(getProperty<T>(oldt, parentfield));
                //新的父栏目ID
                int parentid = Convert.ToInt32(getProperty<T>(entity, parentfield));

                string nodefieldvalue = "";
                int layerfieldvalue = 0;
                //先判断选中的父节点是否被包含
                if (IsContainNode<T>(primarykey, nodefield, oid, parentid))
                {
                    //查找旧父节点数据
                    string class_list = "," + parentid + ",";
                    int class_layer = 1;
                    if (olsparentid > 0)
                    {

                        var oldParentModel = db.SingleOrDefault<T>(parentid);
                        class_list = getProperty<T>(oldParentModel, nodefield).ToString() + parentid + ",";
                        class_layer = Convert.ToInt32(getProperty<T>(oldParentModel, layerfield)) + 1;
                    }
                    //先提升选中的父节点
                    db.Update<T>("SET " + nodefield + "=@0," + layerfield + "=@1," + parentfield + "=@2 WHERE " + primarykey + "=@3", class_list, class_layer, olsparentid, parentid);
                    this.UpdateChilds<T>(primarykey, nodefield, layerfield, parentfield, parentid);
                }
                //更新子节点
                if (parentid > 0)
                {
                    var model2 = db.SingleOrDefault<T>(parentid);
                    nodefieldvalue = getProperty<T>(model2, nodefield).ToString() + oid + ",";
                    layerfieldvalue = Convert.ToInt32(getProperty<T>(model2, layerfield)) + 1;
                }
                else
                {
                    nodefieldvalue = "," + oid + ",";
                    layerfieldvalue = 1;
                }
                setProperty<T>(ref entity, nodefield, nodefieldvalue);
                setProperty<T>(ref entity, layerfield, layerfieldvalue);
                db.Update(entity);
                this.UpdateChilds<T>(primarykey, nodefield, layerfield, parentfield, oid); //更新子节点
                return oid;
            }
            catch { return 0; }
           
        }
        #endregion 更新方法

        #region 删除方法
        /// <summary>
        /// 删除主键对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public long Delete<T>(object id)where T:new()
        {
            return db.Delete<T>(id);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Delete<T>(T entity)where T:new()
        {
            return  db.Delete<T>(entity); ;
        }
        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public long Delete<T>(string where) where T : new()
        {
            Type t = typeof(T);
            string tablename = "";
            object[] nameattrs = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            TableNameAttribute nameattr = null;
            if (nameattrs.Length == 1)
            {
                nameattr = (TableNameAttribute)nameattrs[0];
                tablename = nameattr.Value;
            }
            string sql="";
            if (!string.IsNullOrEmpty(where.Trim()))
                sql = string.Format("DELETE FROM {0} WHERE {1}", tablename, where);
            else
                //sql = string.Format("DELETE FROM {0}", tablename);
                return 0;
            return db.Execute(sql, null);
        }
        /// <summary>
        /// 树表删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">主键值</param>
        /// <param name="nodefield">节点字段</param>
        /// <returns></returns>
        public long DeleteTree<T>(object id,string nodefield) where T : new()
        {
            Type t = typeof(T);
            string tablename = "";
          
            object[] nameattrs = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            TableNameAttribute nameattr = null;
            if (nameattrs.Length == 1)
            {
                nameattr = (TableNameAttribute)nameattrs[0];
                tablename = nameattr.Value;
            }
            string sql = string.Format("DELETE FROM {0} WHERE {1}", tablename, nodefield+" LIKE '%,"+id+",%'"); ;
          
            return db.Execute(sql, null);
        }
        #endregion 删除方法

        #region 私有方法
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="propertyName"></param>
        private object getProperty<T>(T objClass, string propertyName) where T : new()
        {
            if (objClass != null)
            {
                PropertyInfo[] infos = objClass.GetType().GetProperties();
                foreach (PropertyInfo info in infos)
                {
                    if (info.Name == propertyName && info.CanRead)
                    {
                        return info.GetValue(objClass, null);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private void setProperty<T>(ref T objClass, string propertyName, object value) where T : new()
        {
            if (objClass != null)
            {
                PropertyInfo[] infos = objClass.GetType().GetProperties();
                foreach (PropertyInfo info in infos)
                {
                    if (info.Name == propertyName && info.CanWrite)
                    {
                        info.SetValue(objClass, value, null);
                    }
                }
            }
        }
       /// <summary>
       /// 验证节点是否被包含
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="primarykey">主键字段</param>
       /// <param name="nodefield">节点字段</param>
        /// <param name="id">待查询的节点</param>
        /// <param name="parentid">父节点</param>
       /// <returns></returns>
        private bool IsContainNode<T>(string primarykey,string nodefield,int id, int parentid)where T:new()
        {
           return  db.Exists<T>(nodefield + " LIKE '%," + id + ",%' AND " + primarykey + "=" + parentid);
        }
        /// <summary>
        /// 修改子节点的ID列表及深度（自身迭代）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primarykey">主键字段</param>
        /// <param name="nodefield">节点段</param>
        /// <param name="layerfield">节点深度字段</param>
        /// <param name="parentfield">父级关联字段</param>
        /// <param name="parentid"></param>
        private void UpdateChilds<T>(string primarykey, string nodefield, string layerfield, string parentfield, int parentid) where T : new()
        {
            //查找父节点信息
            T model = db.SingleOrDefault<T>(parentid);
            if (model != null)
            {
                //查找子节点
                List<T> childllist = db.Fetch<T>("WHERE " + parentfield + "=" + parentid);
                foreach (var t in childllist)
                {
                    //修改子节点的ID列表及深度
                    int id = Convert.ToInt32(getProperty<T>(t, primarykey)); ;
                    string class_list = getProperty<T>(model, nodefield).ToString() + id + ",";
                    int class_layer = Convert.ToInt32(getProperty<T>(model, layerfield)) + 1;
                    db.Update<T>("SET " + nodefield + "=@0," + layerfield + "=@1 WHERE " + primarykey + "=@2", class_list,class_layer, id);
                    //调用自身迭代
                    this.UpdateChilds<T>(primarykey, nodefield, layerfield, parentfield, id); 
                }
            }
        }
        #endregion 私有方法

    }
}
