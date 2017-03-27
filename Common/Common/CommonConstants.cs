using System;
using System.Configuration;

namespace Common
{
    public class CommonConstants
    {
        public const string AssemblyVersionNumberFormat = "6.0.*";
        public const string BackendUserLoginKey = "BackendUserLogin";
        public const string InstallUserKey = "InstallUser";
        public const string PasswordMaskText = "******";

        #region App Setting Keys
        public const string AppSettingKey_ResourceTypeName = "ResourceTypeName";
        public const string AppSettingKey_EnableEntityFrameworkWrapper = "EnableEntityFrameworkWrapper";
        public const string AppSettingKey_EntityFrameworkCache_Type = "EntityFrameworkCache.Type";
        public const string AppSettingKey_EntityFrameworkCachePolicy_Type = "EntityFrameworkCachePolicy.Type";
        public const string AppSettingKey_EntityFrameworkCachePolicy_MaxCacheableRows = "EntityFrameworkCachePolicy.DefaultMaxCacheableRows";
        public const string AppSettingKey_EntityFrameworkCachePolicy_SlidingExpiration = "EntityFrameworkCachePolicy.SlidingExpiration";
        #endregion

        #region Regex
        public const string EmailRegularExpr = RegexLibrary.Email;
        #endregion

        #region StringComparision
        public static readonly StringComparison StringComparision = StringComparison.InvariantCulture;
        public static readonly StringComparison StringComparisionIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
        public static readonly StringComparer StringComparer = StringComparer.InvariantCulture;
        public static readonly StringComparer StringComparerIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
        #endregion

        #region Rout Extension Related
        public const string RouteValueKey_Controller = "controller";
        public const string RouteValueKey_Action = "action";
        public const string RouteValueKey_Area = "area";
        public const string DataTokensKey_Namespaces = "namespaces";
        public const string DataTokensKey_Area = "area";
        public const string DataTokensKey_ParentActionViewContext = "ParentActionViewContext";
        public const string DataTokensKey_ComponentName = "componentName";
        #endregion


        public const string ReturnUrlQueryStringKey = "ReturnUrl";

        public const string MessageTempDataKey = "message";

        public const string BackUrlQueryStringKey = "BackUrl";


        public static readonly string DefaultPassword = "PASS@word1";
        public static string TestTel = "13500000000";

        #region Cache

        public static int DataCacheDuration
        {
            get
            {
                int r = 0;
                return int.TryParse(ConfigurationManager.AppSettings["DataCacheDuration"].ToString(), out r) ? r : 300;
            }
        }
        public static bool CacheEnabled = ConfigurationManager.AppSettings["DataCacheEnabled"].ToLower().Equals("true");
        #endregion

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        };

        public enum NewsType
        {
            NewsforTraining = 1,
            NewsforClass = 2
        }

        #region DB Records Status
        public enum DbRowStatus
        {
            Enabled = 1,
            Disabled = 2
        }
        #endregion

        public static DateTime DBDateTimeMin = DateTime.Parse("1753-01-01");

    }
}
