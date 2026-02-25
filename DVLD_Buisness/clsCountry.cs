using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = string.Empty;
        }
        private clsCountry(int countryID, string countryName)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
        }
        public static clsCountry Find(int ID)
        {
            string CountryName = "";

            if (clsCountryData.GetCountryInfoByID(ID, ref CountryName))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }

        public static clsCountry Find(string CountryName)
        {

            int ID = -1;

            if (clsCountryData.GetCountryInfoByName(CountryName, ref ID))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }
        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
    }
}
