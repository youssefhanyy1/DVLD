using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enTestType {All=0, VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public enTestType ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }

        public clsTestType()
        {
            this.ID = enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;
        }

        public clsTestType(enTestType id, string title, string description, float fees)
        {
            this.ID = id;
            this.Title = title;
            this.Description = description;
            this.Fees = fees;
            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            int NewID = clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);

            this.ID = (enTestType)NewID;

            return (NewID != -1);
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }

        public static clsTestType Find(enTestType TestTypeID)
        {
            string Title = "", Description = "";
            float Fees = 0;

            if (clsTestTypeData.GetTestTypeByID((int)TestTypeID, ref Title, ref Description, ref Fees))
                return new clsTestType(TestTypeID, Title, Description, Fees);
            else
                return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestType();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTestType();
            }

            return false;
        }
    }
}