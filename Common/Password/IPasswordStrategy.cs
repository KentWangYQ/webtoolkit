namespace Common
{
    public interface IPasswordStrategy
    {
        string Encrypt(string password);
        bool CheckPasswordStrength(string password, out string errorMessage);
        PasswordScore GetPasswordStrength(string password);
    }

    public interface ISymmetricPasswordStrategy : IPasswordStrategy
    {
        string Decrypt(string encryptPassword);
    }
}
