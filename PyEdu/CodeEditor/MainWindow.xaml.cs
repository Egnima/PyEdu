using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ExplorerControl.ViewModels;
using FileSystemModels;

namespace CodeEditor
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        private string currentFileName;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;

            IHighlightingDefinition pythonHighlighting;
            using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("CodeEditor.Resources.Python.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("하이라이팅 리소스 파일을 찾을 수 없습니다.");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    pythonHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }

            HighlightingManager.Instance.RegisterHighlighting("Python Highlighting",
                    new string[] { ".cool" }, pythonHighlighting);

            codeEditor.SyntaxHighlighting = pythonHighlighting;
            codeEditor.PreviewKeyDown += new KeyEventHandler(codeEditor_PreviewKeyDown);

            var appVM = new ApplicationViewModel();
            this.DataContext = appVM;

            var newPath = PathFactory.Create(Environment.CurrentDirectory, FileSystemModels.Models.FSItems.Base.FSItemType.File);
            appVM.InitializeViewModel(newPath);
        }

        private void codeEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                // 파이썬 실행
            }
        }

        private void openFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { CheckFileExists = true,
                DefaultExt = ".py",
                Filter = "파이썬 파일|*.py|일반 텍스트|*.txt|모든 파일|*.*",
                FilterIndex = 0 };
            if (openFileDialog.ShowDialog() ?? false)
            {
                currentFileName = openFileDialog.FileName;
                codeEditor.Load(currentFileName);
                //codeEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
            }
        }

        private void saveFileClick(object sender, EventArgs e)
        {
            if (currentFileName == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { DefaultExt = ".py",
                    Filter = "파이썬 파일|*.py|일반 텍스트|*.txt|모든 파일|*.*",
                    FilterIndex = 0 };
                if (saveFileDialog.ShowDialog() ?? false)
                {
                    currentFileName = saveFileDialog.FileName;
                }
                else { return; }
            }

            codeEditor.Save(currentFileName);
        }

        private void runClick(object sender, EventArgs e)
        {
            MessageBox.Show("Run Button Clicked!");
            Dispatcher.Invoke(new Action(() =>
            {
                btnRun.IsEnabled = false;
                btnStop.IsEnabled = true;
            }));
        }

        private void stopClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Stop Button Clicked!");
            Dispatcher.Invoke(new Action(() =>
            {
                btnRun.IsEnabled = true;
                btnStop.IsEnabled = false;
            }));
        }
    }
}
