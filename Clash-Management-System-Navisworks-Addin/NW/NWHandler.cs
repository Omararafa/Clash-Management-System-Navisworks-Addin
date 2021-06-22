using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.NW
{

    //All "GET" below means get from NW
    //All "Create" below means create object and may/may not create into NW

    public static class NWHandler
    {
        #region ClashMatrixMethods

        static ClashMatrix CreateClashMatrix(List<object>clashMatrixData)
        {
            throw new Exception("Method CreateClashMatrix: Work in progress");
            return null;
        }


        #endregion



        #region ClashTestMethods

        static ClashTest CreateClashTest(List<object> clashTestData)
        {
            throw new Exception("Method CreateClashTest: Work in progress");
            return null;
        }

        static List<ClashTest> CreateClashTest(List<List<object>> clashTestDatas)
        {
            foreach (var clashTestData in clashTestDatas)
            {
                CreateClashTest(clashTestData);
            }
            throw new Exception("Method CreateClashTest: Work in progress");
            return null;
        }


        static ClashTest GetClashTest(string clashTestName)
        {
            throw new Exception("Method GetClashTest: Work in progress");
            return null;
        }


        #endregion


        #region SearchSetMethods

        static SearchSet CreateSearchSet()
        {
            throw new Exception("Method CreateSearchSet: Work in progress");
            return null;
        }

        static SearchSet GetSearchSet()
        {
            throw new Exception("Method GetSearchSet: Work in progress");
            return null;
        }


        #endregion

        #region ClashResultMethods

        static ClashTestResult GetClashTestResult()
        {
            throw new Exception("Method GetClashTestResult: Work in progress");
            return null;
        }



        #endregion
    }
}
