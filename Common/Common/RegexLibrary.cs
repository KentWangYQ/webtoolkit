namespace Common
{
    public static class RegexLibrary
    {
        public const string Email = @"^[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+$";
        public const string Phone = @"^\(\d{3}\)\s\d{3}-\d{4}$";
        public const string PhoneExt = @"^(\d{0,5})$";
        public const string Fax = @"^\(\d{3}\)\s\d{3}-\d{4}$";
        public const string FaxExt = @"^(\d{0,5})$";
        public const string DomainList = @"[^\s,;]+";
        public const string CSharpName = "^[a-zA-Z_][0-9a-zA-Z_]*$";
        public const string UserLoginName = @"^[a-zA-Z0-9_]{4,20}$";
        public const string MemberLoginName = @"^[a-zA-Z0-9_]{4,20}$";
        public const string ExtractHrefUrl = "href\\s*=\\s*((?<1>https?://[^\\s*|\"|>]+)|'(?<1>https?://[^\"|\\s]*)'|\"(?<1>https?://[^\"|\\s]*))\"";
        public const string Url = @"^http:\/\/$|^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";
        public const string MutipleEmailsSeparateWithComma = @"^[^,;@]+@[^\.][^,;@]*\.[\w-]+([,][^,;@]+@[^\.][^,;@]*\.[\w-]+)*$";
        public const string PositiveNumber = @"^[1-9](\d)*$";
        public const string MutipleEmailsSeparateWithSemicolons = @"^[^,;@]+@[^\.][^,;@]*\.[\w-]+([;][^,;@]+@[^\.][^,;@]*\.[\w-]+)*$";

        public const string InternalLinksWithHref = "(href\\s*=\\s*[\"'])(\\s*\\?\\s*navid\\s*=.*?)([\"'])"; ////use "?navid=" to identity internal link?
        public const string ShowDocumentLinksWithHref = "(href\\s*=\\s*[\"'])[^\"']*?home/showdocument\\s*(\\?.*?[\"'])";
        public const string ShowImageLinksWithSrc = "(src\\s*=\\s*[\"'])[^\"']*?home/showimage\\s*(\\?.*?[\"'])";

        public const string MailTo = "(\\\"mailto:){1,1}([^>]+)(\\\"){1,1}[ |>]{1,1}";
    }

    public static class InputMaskedLibrary
    {
        public const string Phone = "(999) 999-9999";
        public const string Fax = "(999) 999-9999";
    }
}
