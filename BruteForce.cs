using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordCracker
{
    public static class BruteForce
    {
        // ブルートフォース攻撃で使用する文字の集合
        private static readonly char[] CharactersToTest = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!$#@-".ToCharArray();

        // パスワードが見つかったかどうかを示すフラグ
        private static bool isMatched = false;

        // 見つかったパスワードを格納する変数
        private static string result;

        // 暗号化されたパスワードを復号化する関数
        public static string Decrypt(string encryptedPassword, string salt, int maxThreads)
        {
            int estimatedPasswordLength = 0;
            while (!isMatched)
            {
                estimatedPasswordLength++;

                // マルチスレッドを使用してブルートフォース攻撃を実行
                ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };
                Parallel.For(0, CharactersToTest.Length, parallelOptions, (i, state) =>
                {
                    // パスワードが見つかった場合、他のスレッドを停止
                    if (isMatched) state.Stop();
                    StartBruteForce(estimatedPasswordLength, encryptedPassword, salt, i);
                });
            }
            return result;
        }

        // ブルートフォース攻撃を開始する関数
        private static void StartBruteForce(int keyLength, string encryptedPassword, string salt, int startIndex)
        {
            var keyChars = new char[keyLength];
            CreateNewKey(0, keyChars, keyLength, keyLength - 1, encryptedPassword, salt, startIndex);
        }

        // 新しいキー（パスワード候補）を生成する再帰関数
        private static void CreateNewKey(int currentCharPosition, char[] keyChars, int keyLength, int indexOfLastChar, string encryptedPassword, string salt, int startIndex)
        {
            for (int i = startIndex; i < CharactersToTest.Length; i++)
            {
                keyChars[currentCharPosition] = CharactersToTest[i];
                if (currentCharPosition < indexOfLastChar)
                {
                    CreateNewKey(currentCharPosition + 1, keyChars, keyLength, indexOfLastChar, encryptedPassword, salt, 0);
                }
                else
                {
                    // キー（パスワード候補）から文字列を生成
                    string testPassword = new string(keyChars);

                    // 生成したパスワード候補を使って暗号化し、元の暗号化されたパスワードと比較
                    string testEncryptedPassword = AesEncryption.Encrypt(testPassword, salt);
                    if (testEncryptedPassword == encryptedPassword)
                    {
                        isMatched = true;
                        result = testPassword;
                        return;
                    }
                }
            }
        }
    }
}