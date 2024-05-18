using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordCracker
{
public static class AesEncryption
{
    // 指定されたプレーンテキストとSaltを使用してAES暗号化を行う関数
    public static string Encrypt(string plainText, string salt)
    {
        using (Aes aes = Aes.Create())
        {
            // Saltからキーを生成
            aes.Key = GenerateKeyFromSalt(salt);
/* 1.Aes.Create()メソッドを使用して、AES暗号化オブジェクト（aes）を作成します。
2.aes.Keyプロパティに、指定されたSaltから生成されたキーを設定します。
3.aes.IVプロパティに、初期化ベクトル（IV）を設定します。このコードでは、単純化のためにゼロのIVが使用されています。
4.aes.CreateEncryptor(aes.Key, aes.IV)メソッドを使用して、暗号化器（encryptor）を作成します。
5.MemoryStreamオブジェクト（ms）を作成します。これは、暗号化されたデータを一時的に格納するためのメモリストリームです。
6.CryptoStreamオブジェクト（cs）を作成します。これは、暗号化ストリームを作成するためのクリプトストリームです。暗号化器（encryptor）とメモリストリーム（ms）をパラメータとして渡します。
7.StreamWriterオブジェクト（sw）を作成します。これは、ストリームにテキストを書き込むためのストリームライターです。クリプトストリーム（cs）をパラメータとして渡します。
8.sw.Write(plainText)を使用して、プレーンテキストをストリームに書き込みます。
9.sw.Close()を使用して、ストリームライターを閉じます。
10.ms.ToArray()を使用して、メモリストリームの内容をバイト配列に変換します。
11.Convert.ToBase64String()を使用して、バイト配列をBase64エンコードして暗号化された結果を文字列として返します。*/

            // 初期化ベクトル（IV）を設定
            aes.IV = new byte[16]; // Zero IV for simplicity

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))// 暗号化器を作成
            using (var ms = new MemoryStream())// メモリストリームを作成
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))//  暗号化ストリームを作成
            using (var sw = new StreamWriter(cs))// ストリームライターを作成
            {
                // プレーンテキストを書き込む
                sw.Write(plainText);
                sw.Close();

                // 暗号化された結果をBase64エンコードして返す
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    // Saltからキーを生成する関数
    private static byte[] GenerateKeyFromSalt(string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            // SaltをSHA-256でハッシュ化
            byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(salt));

            // キーのサイズを256ビット（32バイト）に調整
            Array.Resize(ref key, 32);
            return key;
        }
    }
}
}


