using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Security;

namespace MyToDos.ViewModel
{
    public class SQL
    {
        private string _dataSourceAddress;
        //public string Password
        //{
        //    set
        //    {
        //        CONNECTION_STRING = "Data Source=" + _dataSourceAddress + ";Version=3;Password=" + value + ";";
        //    }
        //}
        private SQLiteConnection _sQLite;
        private DataSet _dataSet = new DataSet();
        private DataTable _dataTable = new DataTable();
        //private SQLiteDataAdapter _dataAdapter;
        public SQL(string dataSourceAddress, string password)
        {
            _dataSourceAddress = dataSourceAddress;
            _sQLite = new SQLiteConnection("Data Source = " + _dataSourceAddress + "; Version = 3;");
            
        }

    }
}
