using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class ClsDataAccsess
    {
        public static readonly string connations = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
    }
}