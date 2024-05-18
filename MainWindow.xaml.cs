using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordCracker
{
    public partial class MainWindow : Window
    {
        // 静的なSaltを定義
        private static readonly string StaticSalt = "staticSalt";

        // 暗号化されたパスワードを格納する変数
        private string encryptedPassword;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 暗号化ボタンがクリックされたときのイベントハンドラ
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            // テキストボックスから入力されたパスワードを取得
            string password = PasswordTextBox.Text;

            // パスワードを暗号化し、結果を表示
            encryptedPassword = AesEncryption.Encrypt(password, StaticSalt);
            EncryptedPasswordTextBlock.Text = $"Encrypted Password: {encryptedPassword}";
        }

        // ファイル保存ボタンがクリックされたときのイベントハンドラ
        private void SaveToFileButton_Click(object sender, RoutedEventArgs e)
        {
            // 暗号化されたパスワードをファイルに保存
            FileManager.SaveToFile("encryptedPassword.txt", encryptedPassword);
            MessageBox.Show("Encrypted password saved to file.");
        }

        // ブルートフォース攻撃開始ボタンがクリックされたときのイベントハンドラ
        private async void StartBruteForceButton_Click(object sender, RoutedEventArgs e)
        {
            // 入力された最大スレッド数が有効かどうかをチェック
            if (!int.TryParse(MaxThreadsTextBox.Text, out int maxThreads))
            {
                MessageBox.Show("Please enter a valid number for max threads.");
                return;
            }

            // ブルートフォース攻撃の実行時間を計測
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // ブルートフォース攻撃を非同期で実行し、復号化されたパスワードを取得
            string decryptedPassword = await Task.Run(() => BruteForce.Decrypt(encryptedPassword, StaticSalt, maxThreads));

            stopwatch.Stop();

            // 復号化されたパスワードと実行時間を表示
            BruteForceResultTextBlock.Text = $"Decrypted Password: {decryptedPassword}";
            TimeElapsedTextBlock.Text = $"Time Elapsed: {stopwatch.Elapsed.TotalSeconds} seconds";
        }
    }
}