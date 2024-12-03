//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Configuration;
//using System.Data;
//using System.Windows.Forms;
//using System.Data.SqlClient;
//using System.IO;
//using System.Collections;
//using System.Drawing;


//namespace Algorithm
//{
//    public class Common
//    {
//        public static string ConnectionStrings = "";  

//        #region GetDataDictionary(string dicName)
//        /// <summary>
//        /// 得到数据字典
//        /// </summary>
//        /// <param name="dicName"></param>
//        /// <returns></returns>
//        public static DataSet GetDataDictionary(string dicName)
//        {
//            DataSet ds;
//            string sql = "Select DictionaryKey,DictionaryValue From DataDictionary Where DictionaryName='" + dicName + "'";
//            try
//            {
//                ds = Common.GetDataSet(sql);
//                return ds;
//            }
//            catch 
//            {
//                MessageBox.Show("读取数据错误");
//                return null;
//            }
//        }
//        #endregion

//        #region GetDataDictionaryDataTable(String astg_DicName,Boolean abol_AllowEmpty)
//        public static DataTable GetDataDictionaryDataTable(String astg_DicName,Boolean abol_AllowEmpty)
//        {
//            String lstg_Sql = "";
//            if (abol_AllowEmpty)
//            {
//                lstg_Sql = @"Select '' as DictionaryKey,'' as DictionaryValue 
//                             Union 
//                             Select DictionaryKey,DictionaryValue 
//                               From DataDictionary 
//                              Where DictionaryName='" + astg_DicName + "';";
//            }
//            else
//            {
//                lstg_Sql = @"Select DictionaryKey,DictionaryValue From DataDictionary Where DictionaryName='" + astg_DicName + "';";
//            }          

//            return Common.GetDataTable(lstg_Sql);
//        }
//        #endregion
                

//        #region GetDataTable(string Sqlsource)
//        //用于传入一个SQL语句，返回一个dataset对象  Blate Cheng 
//        public static DataTable GetDataTable(string Sqlsource)
//        {         
//            try
//            {
                
//                SqlConnection sqlconn = new SqlConnection(ConnectionStrings);
                
//                sqlconn.Open();
                

//                DataTable dt_table = new DataTable();
//                SqlDataAdapter ldapt1 = new SqlDataAdapter(Sqlsource, sqlconn);
//                ldapt1.SelectCommand.CommandTimeout = 180;
                
//                ldapt1.Fill(dt_table);

//                return dt_table;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("数据库错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return null;
//            }

//        }
//        #endregion               


//        #region GetDataSet(string Sqlsource)
//        //用于传入一个SQL语句，返回一个dataset对象  Blate Cheng 
//        public static DataSet GetDataSet(string Sqlsource)
//        {
//            try
//            {
//                SqlConnection sqlconn = new SqlConnection(ConnectionStrings);
//                sqlconn.Open();

//                DataSet ds = new DataSet();
//                SqlDataAdapter ldapt1 = new SqlDataAdapter(Sqlsource, sqlconn);
//                ldapt1.Fill(ds);

//                return ds;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("数据库错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return null;
//            }
//        }
//        #endregion

//        #region  GetConnection()
//        //用于返回一个数据库连接  Blate Cheng 
//        public static SqlConnection GetConnection()
//        {
//            try
//            {
//                SqlConnection sqlconn = new SqlConnection(ConnectionStrings);
//                sqlconn.Open();

//                return sqlconn;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("数据库连接错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return null;
//            }
//        }
//        #endregion
       
//        #region ExecuteNonQuery(string Sqlsource)
//        //用于传入一个SQL语句，返回一个dataset对象  Blate Cheng 
//        public static int ExecuteNonQuery(string SqlExecute )
//        {
//            try
//            {        
//                SqlCommand sqlcomm = new SqlCommand(SqlExecute, GetConnection());

//                return sqlcomm.ExecuteNonQuery();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("SQL执行错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//                return -1;
//            }
//        }
//        #endregion            

//        #region GetSqlDataStr(string Sqlsource)
//        //用于传入一个SQL语句,SQL中取一个字段的值，返回一个String字符串  Blate Cheng 
//        public static String GetSqlDataStr(string Sqlsource)
//        {           
//            try
//            {
//                SqlConnection sqlconn = new SqlConnection(ConnectionStrings);
//                sqlconn.Open();

//                DataTable dt_table = new DataTable();
//                SqlDataAdapter ldapt1 = new SqlDataAdapter(Sqlsource, sqlconn);
//                ldapt1.Fill(dt_table);

//                if (dt_table.Rows.Count > 0)
//                    return dt_table.Rows[0][0].ToString();
//                else
//                    return "";
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("数据库错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return "";
//            }

//        }
//        #endregion

//        #region GetSqlDataInt(string Sqlsource)
//        //用于传入一个SQL语句,SQL中取一个字段的值，返回一个Int  Blate Cheng 
//        public static int GetSqlDataInt(string Sqlsource)
//        {
//            try
//            {
//                SqlConnection sqlconn = new SqlConnection(ConnectionStrings);
//                sqlconn.Open();

//                DataTable dt_table = new DataTable();
//                SqlDataAdapter ldapt1 = new SqlDataAdapter(Sqlsource, sqlconn);
//                ldapt1.Fill(dt_table);

//                if (dt_table.Rows.Count > 0)
//                    return int.Parse(dt_table.Rows[0][0].ToString());
//                else
//                    return 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("数据库错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return 0;
//            }
//        }
//        #endregion

//        #region FillDataTable(string Sqlsource,DataTable dt )
//        //用于传入一个SQL语句和一个表,取数据后Fill到datatabel  Blate Cheng 
//        public static int FillDataTable(string Sqlsource,DataTable dt_table )
//        {
//            try
//            {
//                SqlConnection sqlconn = new SqlConnection(ConnectionStrings);
//                sqlconn.Open();

//                dt_table.Clear();
//                SqlDataAdapter ldapt1 = new SqlDataAdapter(Sqlsource, sqlconn);
//                ldapt1.Fill(dt_table);

//                return dt_table.Rows.Count;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("数据库错误" + ex.ToString(), "震坤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return 0;
//            }
//        }
//        #endregion

//    }
//}
