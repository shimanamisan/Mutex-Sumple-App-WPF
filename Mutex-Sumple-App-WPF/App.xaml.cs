using System;
using System.Threading;
using System.Windows;

namespace Mutex_Sumple_App_WPF
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    // Mutex名（アプリケーション固有の名前. 他のアプリケーションと被らない用に注意）
    private const string _mutexName = "MutexTest";

    // Mutexオブジェクトを作成
    private Mutex _mutex = new Mutex(false, _mutexName);

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public App()
    {

      // 起動時のイベントにメソッドを登録
      Startup += Application_Startup;

      // アプリケーション終了時のイベントにメソッドを登録
      Exit += Application_Exit;
    }

    /// <summary>
    /// スタートアップ時のイベントに読み込ませるメソッド
    /// </summary>
    /// <param name="sender">イベントを送信したオブジェクト</param>
    /// <param name="e">イベント引数</param>
    private void Application_Startup(object sender, StartupEventArgs e)
    {

      // ミューテックスの所有権を要求
      if (!_mutex.WaitOne(0, false))
      {
        // 既に起動しているため終了させる
        MessageBox.Show("ソフトウェアは既に起動しています。",
                        "二重起動防止",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);

        // インスタンスが所有しているリソースを開放する
        _mutex.Close();

        // null を代入
        _mutex = null;

        // アプリケーションを終了させる
        Environment.Exit(0);
      }
    }

    /// <summary>
    /// アプリケーション終了時のイベントに登録するメソッド
    /// </summary>
    /// <param name="sender">イベントを送信したオブジェクト</param>
    /// <param name="e">イベント引数</param>
    private void Application_Exit(object sender, ExitEventArgs e)
    {

      if (_mutex != null)
      {
        // 共有リソースである mutex を開放
        _mutex.ReleaseMutex();

        // インスタンスが所有しているリソースを開放する
        _mutex.Close();
      }
    }
  }
}
