using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Mail;
using System.IO;
using System.Text;
/// <summary>
/// Summary description for EncryptJKS
/// </summary>
public class EncryptJKS
{
    public EncryptJKS()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //Encryptpwd & Decryptpwd Added by Kuntalkarar on 28thMay2016-------------------------------------------
    public string RijndaelEncription(string strNewPassword)
    {

        string plainText = strNewPassword; // original plaintext

        string passPhrase = "qS@RHPss@p";// can be any string
        string saltValue = ConfigurationManager.AppSettings["SaltingKey"].Trim().ToString(); //"s@1tValue";        // can be any string
        string hashAlgorithm = "SHA1";             // can be "MD5"
        int passwordIterations = 2;                // can be any number
        string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        int keySize = 256;                // can be 192 or 128

        // string CurrentcipherText = objEncrypt.Encryptpwd(strCurrentPassword, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        string NewcipherText = Encryptpwd(plainText, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        /*================================================================*/

        /*Added by kuntalkarar for RAJNDAEL encryption on 28thMay2016===================*/
        //iReturnVal = objUsers.ChangePassword(iUserID, strUserName,CurrentcipherText,NewcipherText);// Encrypt.EncryptData(cipherText));
        return NewcipherText;
    }

    public string RijndaelDecryption(string strNewPassword)
    {

        string plainText = strNewPassword; // original plaintext

        string passPhrase = "qS@RHPss@p";// can be any string
        string saltValue = ConfigurationManager.AppSettings["SaltingKey"].Trim().ToString(); //"s@1tValue";        // can be any string
        string hashAlgorithm = "SHA1";             // can be "MD5"
        int passwordIterations = 2;                // can be any number
        string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        int keySize = 256;                // can be 192 or 128

        // string CurrentcipherText = objEncrypt.Encryptpwd(strCurrentPassword, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        string NewcipherText = Decryptpwd(plainText, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        /*================================================================*/

        /*Added by kuntalkarar for RAJNDAEL encryption on 28thMay2016===================*/
        //iReturnVal = objUsers.ChangePassword(iUserID, strUserName,CurrentcipherText,NewcipherText);// Encrypt.EncryptData(cipherText));
        return NewcipherText;
    }
















    public string Encryptpwd
        (
        string plainText,
        string passPhrase,
        string saltValue,
        string hashAlgorithm,
        int passwordIterations,
        string initVector,
        int keySize
        )
    {
        // Convert strings into byte arrays.
        // Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
        // encoding.
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

        // Convert our plaintext into a byte array.
        // Let us assume that plaintext contains UTF8-encoded characters.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        // First, we must create a password, from which the key will be derived.
        // This password will be generated from the specified passphrase and 
        // salt value. The password will be created using the specified hash 
        // algorithm. Password creation can be done in several iterations.
        PasswordDeriveBytes password = new PasswordDeriveBytes
            (
            passPhrase,
            saltValueBytes,
            hashAlgorithm,
            passwordIterations
            );

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = password.GetBytes(keySize / 8);

        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = new RijndaelManaged();

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;

        // Generate encryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (
            keyBytes,
            initVectorBytes
            );

        // Define memory stream which will be used to hold encrypted data.
        MemoryStream memoryStream = new MemoryStream();

        // Define cryptographic stream (always use Write mode for encryption).
        CryptoStream cryptoStream = new CryptoStream
            (
            memoryStream,
            encryptor,
            CryptoStreamMode.Write
            );

        // Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

        // Finish encrypting.
        cryptoStream.FlushFinalBlock();

        // Convert our encrypted data from a memory stream into a byte array.
        byte[] cipherTextBytes = memoryStream.ToArray();

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert encrypted data into a base64-encoded string.
        string cipherText = Convert.ToBase64String(cipherTextBytes);

        // Return encrypted string.
        return cipherText;
    }
    public string Decryptpwd
        (
        string cipherText,
        string passPhrase,
        string saltValue,
        string hashAlgorithm,
        int passwordIterations,
        string initVector,
        int keySize
        )
    {
        // Convert strings defining encryption key characteristics into byte
        // arrays. Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
        // encoding.
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

        // Convert our ciphertext into a byte array.
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        // First, we must create a password, from which the key will be 
        // derived. This password will be generated from the specified 
        // passphrase and salt value. The password will be created using
        // the specified hash algorithm. Password creation can be done in
        // several iterations.
        PasswordDeriveBytes password = new PasswordDeriveBytes
            (
            passPhrase,
            saltValueBytes,
            hashAlgorithm,
            passwordIterations
            );

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = password.GetBytes(keySize / 8);

        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = new RijndaelManaged();

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;

        // Generate decryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        ICryptoTransform decryptor = symmetricKey.CreateDecryptor
            (
            keyBytes,
            initVectorBytes
            );

        // Define memory stream which will be used to hold encrypted data.
        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

        // Define cryptographic stream (always use Read mode for encryption).
        CryptoStream cryptoStream = new CryptoStream
            (
            memoryStream,
            decryptor,
            CryptoStreamMode.Read
            );

        // Since at this point we don't know what the size of decrypted data
        // will be, allocate the buffer long enough to hold ciphertext;
        // plaintext is never longer than ciphertext.
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        // Start decrypting.
        int decryptedByteCount = cryptoStream.Read
            (
            plainTextBytes,
            0,
            plainTextBytes.Length
            );

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert decrypted data into a string. 
        // Let us assume that the original plaintext string was UTF8-encoded.
        string plainText = Encoding.UTF8.GetString
            (
            plainTextBytes,
            0,
            decryptedByteCount
            );

        // Return decrypted string.   
        return plainText;
    }
    //ENDS ===Encryptpwd & Decryptpwd Added by Kuntalkarar on 28thMay2016-------------------------------------------
    public static string EncryptData(string Message)
    {
        byte[] Results;
        string passphrase = "Password";
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        byte[] DataToEncrypt = UTF8.GetBytes(Message);
        try
        {
            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return Convert.ToBase64String(Results);
    }
    public static string DecryptString(string Message)
    {
        byte[] Results;
        string passphrase = "Password";
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;
        byte[] DataToDecrypt = Convert.FromBase64String(Message);
        try
        {
            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        }
        finally
        {
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }
        return UTF8.GetString(Results);
    }
    public static int UpdateEncrypt(int UserID, int CompanyID, string Password)
    {
        int iReturnValue = 0;
        SqlCommand sqlCmd = null;
        SqlParameter sqlReturnParam = null;
        string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        SqlConnection sqlConn = new SqlConnection(ConsString);
        sqlCmd = new SqlCommand("Sp_SaveEncryptPassword_ETH", sqlConn);
        sqlCmd.CommandType = CommandType.StoredProcedure;
        sqlCmd.Parameters.Add("@UserID", UserID);
        sqlCmd.Parameters.Add("@Password", Password);
        sqlReturnParam = sqlCmd.Parameters.Add("ReturnValue", SqlDbType.Int);
        sqlReturnParam.Direction = ParameterDirection.ReturnValue;
        try
        {
            sqlConn.Open();
            sqlCmd.ExecuteNonQuery();
            iReturnValue = Convert.ToInt32(sqlReturnParam.Value);
        }
        catch (Exception ex)
        {
            string strExceptionMessage = ex.Message.Trim();
        }
        finally
        {
            sqlReturnParam = null;
            if (sqlCmd != null)
                sqlCmd.Dispose();
            if (sqlConn != null)
                sqlConn.Close();
        }

        return (iReturnValue);
    }
    public static string DecryptPassword(int UserID)
    {
        string LabelValue = "";
        string ConsString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        SqlConnection sqlConn = new SqlConnection(ConsString);
        SqlDataAdapter sqlDA = new SqlDataAdapter("Sp_SearchPasswordByUserID", sqlConn);
        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
        sqlDA.SelectCommand.Parameters.Add("@UserID", UserID);
        DataSet ds = new DataSet();
        try
        {
            sqlConn.Open();
            sqlDA.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    LabelValue = LabelValue + "UserName :" + ds.Tables[0].Rows[i][0].ToString() + "<br />";
                    LabelValue = LabelValue + "CompanyName :" + ds.Tables[0].Rows[i][1].ToString() + "<br />";
                    if (Convert.ToString(ds.Tables[0].Rows[i][2]) == "")
                        LabelValue = LabelValue + "Decrypt Password : NULL";
                    else
                        LabelValue = LabelValue + "Decrypt Password :" + CBSolutions.ETH.Web.Encrypt.DecryptString(ds.Tables[0].Rows[i][2].ToString()) + "    <br />";

                }
            }
        }
        catch (Exception ex)
        {
            LabelValue = "Error <br />" + ex.Message.ToString();
        }
        finally
        {
            sqlConn.Close();
            sqlDA.Dispose();
            ds = null;
        }
        return LabelValue.ToString();
    }
    #region SendEmail
    public static void SendEmail(string strSubject, string strBody)
    {
        try
        {
            MailMessage _mailMSG = new MailMessage();
            _mailMSG.From = "support@p2dgroup.com";
            _mailMSG.To = "rjaiswal@vnsinfo.com.au";
            _mailMSG.Cc = "errorvns@gmail.com";
            _mailMSG.Subject = strSubject;
            _mailMSG.Body = strBody;
            _mailMSG.Priority = MailPriority.High;
            _mailMSG.BodyFormat = MailFormat.Html;
            SmtpMail.SmtpServer = "";
            SmtpMail.Send(_mailMSG);
        }
        catch (Exception ex)
        {
            string ss = ex.Message.Trim();
        }
    }
    #endregion
}