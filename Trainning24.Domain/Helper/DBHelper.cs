using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Trainning24.Domain.Helper

{
    public class DBHelper
    {
        private int retryAttempts = 0;
        private MySqlConnection _connection;

        public DBHelper(string connectionString)
        {
            Initialize(connectionString);
        }

        private void Initialize(string connectionString)
        {
            try
            {
                _connection = new MySqlConnection(connectionString);

            }
            catch (Exception)
            {

                throw;
            }

            //_connection.ConnectionString = String.Format("server={0};database={1};uid={2};password={3}", server, database, uid, password);
            //_connection.ConnectionString = connectionString;// new SqlConnectionStringBuilder(connectionString).ToString();
            //_connection.ConnectionString =  new SqlConnectionStringBuilder(connectionString).ToString();
        }
        public void Open()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
            }
            catch (MySqlException e)
            {
                _connection.Close();
                if (e.Number == 0 && retryAttempts < 4)
                {
                    retryAttempts++;
                    _connection.Close();

                    Open();
                }
                throw  e;
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
        }

        #region Closing active connection
        public void Close()
        {
            try
            {
                // if(_connection..) // check if opened and close..
                _connection.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Cannot disconnect from server.", ex);
            }
        }
        #endregion

        #region Disposing connection
        public void Dispose()
        {
            Close();
        }
        #endregion

        #region int ExecuteStoredProcedure Excecute storedprocedure with parameters and return  as count 
        //for select result and return as Dataset 
        public int ExecuteStoredProcedure(string name, Dictionary<string, object> parameters)
        {
            using (var cmd = new MySqlCommand() { Connection = _connection })
            {
                cmd.CommandText = name;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                var count = cmd.ExecuteNonQuery();
                return count;
            }

        }

        public DataSet SelectList(String SP_NAME, SortedDictionary<string, string> sd)
        {
            try
            {

                return SP_DataTable_return(SP_NAME, GetSdParameter(sd));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MySqlParameter[] GetSdParameter(SortedDictionary<string, string> sortedDictionary)
        {
            MySqlParameter[] paramArray = new MySqlParameter[] { };

            foreach (string key in sortedDictionary.Keys)
            {
                // AddParameter(ref paramArray, new MySqlParameter(key, sortedDictionary[key]));
            }

            return paramArray;
        }

        public DataSet SP_DataTable_return(String ProcName, params MySqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            //open connection

            //for Select Query               

            MySqlCommand cmdSel = new MySqlCommand(ProcName, _connection);
            cmdSel.CommandType = CommandType.StoredProcedure;
            // Assign the provided values to these parameters based on parameter order
            AssignParameterValues(commandParameters, commandParameters);
            AttachParameters(cmdSel, commandParameters);
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(ds);
            //close connection

            return ds;
        }

        private static void AttachParameters(MySqlCommand command, MySqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (MySqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void AssignParameterValues(MySqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }


        public DataTable ExecuteStoredProcedureAndReturnData(string name, Dictionary<string, object> parameters)
        {
            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand() { Connection = _connection })
            {
                cmd.CommandText = name;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(rdr);
            }
            return dt;
        }

        #endregion

        #region ExecuteNonQuery  for insert / Update and Delete
        //For Insert/Update/Delete  
        public int ExecuteNonQuery_IUD(String Querys)
        {
            int result = 0;

            //create command and assign the query and connection from the constructor  
            MySqlCommand cmd = new MySqlCommand(Querys, _connection);
            //Execute command  
            result = cmd.ExecuteNonQuery();
            return result;
        }
        #endregion

        #region Dataset for select result and return as Dataset 
        //for select result and return as Dataset 
        public DataSet ExcecuteQueryDS(String Querys)
        {

            DataSet ds = new DataSet();
            //for Select Query              
            MySqlCommand cmdSel = new MySqlCommand(Querys, _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(ds);

            return ds;
        }
        #endregion

        #region DataTable for select result and return as DataTable 
        //for select result and return as DataTable 
        public DataTable ExcecuteQueryDT(String Querys)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmdSel = new MySqlCommand(Querys, _connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);

            return dt;
        }
        #endregion
    }
}
