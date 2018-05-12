using Microsoft.Win32;
using NeuralNetworks;
using NeuralNetworks.Data;
using NeuralNetworks.DataGenerators;
using NeuralNetworks.Learning;
using NeuralNetworks.Learning.MLP;
using NeuralNetworks.Trainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NNApp
{
    /// <summary>
    /// Interaction logic for PictureCompressionWindow.xaml
    /// </summary>
    public partial class PictureCompressionWindow : Window
    {
        public MainWindow MainWindow { get; set; } = ((MainWindow)Application.Current.MainWindow);

        NeuralNetwork currentNetwork;

        private string bmpFileName;
        private string generatedFileName;
        private string dataFileName;
        private string codeBookFileName;
        private string deCompressedBmpFileName;
        private string compressedDataFileName;
        private string compressedCodeBookFileName;

        public PictureCompressionWindow()
        {
            InitializeComponent();
        }

        private void BmpFileButton_Click(object sender, RoutedEventArgs e)
        {
            var sonParameters = MainWindow.SonParameters;
            bmpFileName = ShowFileWindow("Learn File", "Bitmap", "*.bmp");
            var gen = new DataToCompressGenerator();
            // var genFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "\\Files\\generatedData.txt");
            var genFile = Directory.GetCurrentDirectory() + "\\Files\\generatedData.txt";
            gen.GenerateData(bmpFileName, genFile);
            MainWindow.DataProvider = new PointsDataProvider(genFile, CompressionConstants.neuronsInFrame);
            MainWindow.Trainer = new SONTrainer
               (
                (IDataProvider)MainWindow.DataProvider,
                MainWindow.CurrentNetwork,
                (SONLearningAlgorithm)MainWindow.LearningAlgorithm,
                new SONLearningRateHandler(sonParameters.StartingLearningRate, sonParameters.MinimumLearningRate, sonParameters.MaxIterations),
                sonParameters.LengthCalculator,
                new ConscienceWithPotential(sonParameters.ConscienceMinPotential, sonParameters.NeuronsCounter, ((IDataProvider)MainWindow.DataProvider).Points.Length * 2)
               );

        }

        private void LearnButton_Click(object sender, RoutedEventArgs e)
        {
            currentNetwork = MainWindow.CurrentNetwork;
            int dataCount = Convert.ToInt32(DataCountBox.Text);
            MainWindow.Trainer.TrainNetwork(ref currentNetwork, dataCount);
        }

        private void DataButton_Click(object sender, RoutedEventArgs e)
        {
            dataFileName = ShowFileWindow("Data File", "Text File", "*.txt");
        }

        private void CodeBookButton_Click(object sender, RoutedEventArgs e)
        {
            codeBookFileName = ShowFileWindow("Code Book File", "Text File", "*.txt");
        }

        private void GeneratedFileButton_Click(object sender, RoutedEventArgs e)
        {
            generatedFileName = ShowFileWindow("Generated File", "Text File", "*.txt");
        }

        private void CompressButton_Click(object sender, RoutedEventArgs e)
        {
            dataFileName = Directory.GetCurrentDirectory() + "\\Files\\" + System.IO.Path.ChangeExtension(DataOutputBox.Text, ".txt");
            codeBookFileName = Directory.GetCurrentDirectory() + "\\Files\\" + System.IO.Path.ChangeExtension(CodeBookOutputBox.Text, ".txt");
            var compressor = new DataCompressor(MainWindow.SonParameters.LengthCalculator);
            compressor.CompressData(
                currentNetwork,
                generatedFileName,
                dataFileName,
                codeBookFileName
                );
        }

        private void DeCompressedBmpFileButton_Click(object sender, RoutedEventArgs e)
        {
            deCompressedBmpFileName = ShowFileWindow("Decompress File", "Bitmap", "*.bmp");
        }

        private void CompressedDataButton_Click(object sender, RoutedEventArgs e)
        {
            compressedDataFileName = ShowFileWindow("Data File", "Text File", "*.txt");
        }

        private void CompressedCodeBookButton_Click(object sender, RoutedEventArgs e)
        {
            compressedCodeBookFileName = ShowFileWindow("Code Book File", "Text File", "*.txt");
        }

        private void DeCompressButton_Click(object sender, RoutedEventArgs e)
        {
            deCompressedBmpFileName = Directory.GetCurrentDirectory() + "\\Files\\" + System.IO.Path.ChangeExtension(BmpOutputBox.Text, ".txt");
            var decompressor = new CompressedImageReader();
            decompressor.ReadCompressedImage(
                compressedDataFileName,
                compressedCodeBookFileName,
                deCompressedBmpFileName
                );
        }

        private string ShowFileWindow(string WindowName, string ExtensionName, string Extension)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ExtensionName + " (" + Extension + ")" + "|" + Extension + "|All files (*.*)|*.*"; //"Text files (*.txt)|*.txt" "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = WindowName;
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else
                return "";
        }
        private void TopBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            MainWindow.WindowState = WindowState.Minimized;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
