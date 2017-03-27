using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public abstract class PasswordStrategyBase : IPasswordStrategy
    {
        #region Static
        public static readonly PasswordStrategy MD5PasswordStrategy = new PasswordStrategy();
        public static readonly SymmetricPasswordStrategy SymmetricPasswordStrategy = new SymmetricPasswordStrategy();
        #endregion

        #region Const
        public const string digitDescription = "digit";
        public const string lowercaseDescription = "lowercase English letter";
        public const string uppercaseDescription = "uppercase English letter";
        public const string specialDescription = "character neither digit nor English letter";
        #endregion

        #region public property

        /// <summary>
        /// User can customize validation rule with the following format:
        /// (?= some_expression)
        /// For example:
        /// ValidationRule.Add(@"(?=.*[0-9])");
        /// ValidationRule.Add(@"(?=.*[a-z])");
        /// And "()" must be provided, otherwise the rule will cause validation exception
        /// </summary>
        public List<KeyValuePair<string, string>> ValidateionRuleDescriptionPair
        {
            get;
            protected set;
        }

        /// <summary>
        /// User can customize minimum password length, if this value is not customized,
        /// the default length "8" will be used
        /// </summary>
        public int MinLength
        {
            get;
            protected set;
        }

        /// <summary>
        /// User can customize password strength level, if this value is not customized,
        /// the highest strength level will be used
        /// </summary>
        public PasswordScore Score
        {
            get;
            protected set;
        }

        #endregion

        #region protected method
        /// <summary>
        /// Initialize validation rule
        /// </summary>
        /// <param name="minLength">user customized minimum password length</param>
        /// <param name="validationRule">user customized validation rule, if it is null or empty, default validation rule will be used</param>
        /// <param name="score">user customized password strength level</param>
        protected virtual void InitializeValidationRule(int minLength, List<KeyValuePair<string, string>> validateionRuleDescriptionPair, PasswordScore score)
        {
            if (validateionRuleDescriptionPair == null || validateionRuleDescriptionPair.Count == 0)
            {
                ////ValidationRule = new List<string>();
                ////ValidationRule.Add(@"(?=.*[0-9])");
                ////ValidationRule.Add(@"(?=.*[a-z])");
                ////ValidationRule.Add(@"(?=.*[A-Z])");
                ////ValidationRule.Add(@"(?=.*[\W])");

                this.ValidateionRuleDescriptionPair = new List<KeyValuePair<string, string>>();
                this.ValidateionRuleDescriptionPair.Add(new KeyValuePair<string, string>(@"(?=.*[0-9])", digitDescription));
                this.ValidateionRuleDescriptionPair.Add(new KeyValuePair<string, string>(@"(?=.*[a-z])", lowercaseDescription));
                this.ValidateionRuleDescriptionPair.Add(new KeyValuePair<string, string>(@"(?=.*[A-Z])", uppercaseDescription));
                this.ValidateionRuleDescriptionPair.Add(new KeyValuePair<string, string>(@"(?=.*[\W])", specialDescription));
            }
            else
            {
                this.ValidateionRuleDescriptionPair = validateionRuleDescriptionPair;
                ////ValidationRule = validationRule;
            }

            this.MinLength = minLength > 0 ? minLength : 8;
            this.Score = score;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Default .ctor for use Encrypt() only
        /// </summary>
        public PasswordStrategyBase()
            : this(8, PasswordScore.VeryStrong)
        {
        }

        /// <summary>
        /// Use default validation rule
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="score"></param>
        public PasswordStrategyBase(int minLength, PasswordScore score)
            : this(minLength, null, score)
        {
        }

        /// <summary>
        /// Use customized validation rule
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="validationRule"></param>
        /// <param name="score"></param>
        public PasswordStrategyBase(int minLength, List<KeyValuePair<string, string>> validateionRuleDescriptionPair, PasswordScore score)
        {
            this.InitializeValidationRule(minLength, validateionRuleDescriptionPair, score);
        }
        #endregion

        #region public method

        /// <summary>
        /// Check password strength
        /// Return "true" means the password strength is enough
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool CheckPasswordStrength(string password, out string errorMessage)
        {
            bool isStrongEnough = false;

            PasswordScore score = this.GetPasswordStrength(password);
            switch (score)
            {
                case PasswordScore.Blank:
                    isStrongEnough = (int)this.Score <= (int)score ? true : false;
                    break;
                case PasswordScore.VeryWeak:
                    isStrongEnough = (int)this.Score <= (int)score ? true : false;
                    break;
                case PasswordScore.Weak:
                    isStrongEnough = (int)this.Score <= (int)score ? true : false;
                    break;
                case PasswordScore.Medium:
                    isStrongEnough = (int)this.Score <= (int)score ? true : false;
                    break;
                case PasswordScore.Strong:
                    isStrongEnough = (int)this.Score <= (int)score ? true : false;
                    break;
                case PasswordScore.VeryStrong:
                    isStrongEnough = (int)this.Score <= (int)score ? true : false;
                    break;
            }
            this.BuildValidateRuleDescripitionByScore();

            errorMessage = isStrongEnough ? string.Empty : string.Format("Your password is not stong enough. It needs include: {0}, and mininum length is {1}.", this.BuildValidateRuleDescripitionByScore(), this.MinLength);
            return isStrongEnough;
        }

        /// <summary>
        /// Build validate rule descripition by Score
        /// </summary>
        /// <returns></returns>
        private string BuildValidateRuleDescripitionByScore()
        {
            StringBuilder sb = new StringBuilder();
            int ruleIndex = ((int)this.Score - 1) > this.ValidateionRuleDescriptionPair.Count ? this.ValidateionRuleDescriptionPair.Count : ((int)this.Score - 1);
            if (ruleIndex <= 0)
                sb.Append(", any character");
            else
                for (int i = 0; i < ruleIndex; i++)
                {
                    sb.Append(", ").Append(this.ValidateionRuleDescriptionPair[i].Value);
                }
            return sb.Remove(0, 1).ToString();
        }

        /// <summary>
        /// Get password strength
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual PasswordScore GetPasswordStrength(string password)
        {
            int score = 1;

            if (string.IsNullOrEmpty(password))
                return PasswordScore.Blank;

            if (password.Length >= this.MinLength)
            {
                for (int i = 0; i < this.ValidateionRuleDescriptionPair.Count; i++)
                {
                    string rule = this.ValidateionRuleDescriptionPair[i].Key;
                    try
                    {
                        if (Regex.Match(password, rule, RegexOptions.ECMAScript).Success)
                            score++;
                    }
                    catch (ArgumentException ex)
                    {
                        throw ex;
                    }
                    continue;
                }
            }

            score = (int)(score > (int)PasswordScore.VeryStrong ? (int)PasswordScore.VeryStrong : score);


            return (PasswordScore)score;
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// Password encrypt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public abstract string Encrypt(string password);
        #endregion
    }
}
