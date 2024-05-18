using System.IO;

namespace PasswordCracker
{
    public static class FileManager
    {
        // 指定されたファイル名とコンテンツをファイルに保存する関数
        public static void SaveToFile(string fileName, string content)
        {
            // 保存先のフルパスを構築
            string filePath = Path.Combine(@"C:\Users\annon\source\repos\PasswordCracker\PasswordStore", fileName);

            // ファイルに内容を書き込む
            File.WriteAllText(filePath, content);
        }
    }
}