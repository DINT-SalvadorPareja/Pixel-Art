using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pixel_Art
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int dimensions = 8;
        private int posicionColumna = 0;
        private int posicionCelda = 0;
        private SolidColorBrush pincelColor;
        public MainWindow()
        {
            
            InitializeComponent();
            GeneraGrid();
            AsignaTagBoton();
            AsignaTagColor();
            
            



        }

        

        public void AsignaTagBoton()
        {
            TamPequenoButton.Tag = 1;
            TamMedianoButton.Tag = 2;
            TamGrandeButton.Tag = 3;

            ExportToPNG.Tag = 1;
            ExportToTIFF.Tag = 2;
        }

        public void AsignaTagColor()
        {
            ColorNegro.Tag = 1;
            ColorBlanco.Tag = 2;
            ColoRojo.Tag = 3;
            ColoVerde.Tag = 4;
            ColoAzul.Tag = 5;

            ColoAmarillo.Tag = 6;
            ColoNaranja.Tag = 7;
            ColoRosa.Tag = 8;
            
            ColorCustom.Tag = 9;
            

        }

        public void GeneraGrid()
        {
            GridDibujo.ColumnDefinitions.Clear();
            GridDibujo.RowDefinitions.Clear();

            for (int i = 0; i < dimensions; i++)
            {
                ColumnDefinition c1 = new ColumnDefinition();
                GridDibujo.ColumnDefinitions.Add(c1);

            }

            for (int i = 0; i < dimensions; i++)
            {
                RowDefinition r1 = new RowDefinition();
                GridDibujo.RowDefinitions.Add(r1);

            }



            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    Border b1 = new Border();
                    b1.BorderThickness = new Thickness(1);
                    b1.Background = new SolidColorBrush(Colors.White);
                    b1.BorderBrush = new SolidColorBrush(Colors.Gray);

                    Grid.SetColumn(b1, j);
                    Grid.SetRow(b1, i);

                    GridDibujo.Children.Add(b1);

                }


            }
            
            
            

        }

       
       

        private void GridDibujoMouseMove(object sender, MouseEventArgs e)
        {
            var posicion = (UIElement)e.Source;

            posicionColumna = Grid.GetColumn(posicion);
            posicionCelda = Grid.GetRow(posicion);
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Border fondo = new Border();
                fondo.Background = pincelColor;
                Grid.SetColumn(fondo, posicionColumna);
                Grid.SetRow(fondo, posicionCelda);
                GridDibujo.Children.Add(fondo);

            }
            else if(e.RightButton == MouseButtonState.Pressed)
            {
                Border b1 = new Border();
                b1.BorderThickness = new Thickness(1);
                b1.Background = new SolidColorBrush(Colors.White);
                b1.BorderBrush = new SolidColorBrush(Colors.Gray);
                Grid.SetColumn(b1, posicionColumna);
                Grid.SetRow(b1, posicionCelda);
                GridDibujo.Children.Add(b1);
            }


        }

        private void GridDibujoMouseClick(object sender, MouseButtonEventArgs e)
        {
            
            Border fondo = new Border();
            fondo.Background = pincelColor;
            Grid.SetColumn(fondo, posicionColumna);
            Grid.SetRow(fondo, posicionCelda);
            GridDibujo.Children.Add(fondo);

            
            

            
        }

        private void SelectorGridClick(object sender, RoutedEventArgs e)
        {
            Button tam = (Button)sender;
            var result = MessageBox.Show("Se va a resetear la cuadricula","PXiFY", MessageBoxButton.YesNo, MessageBoxImage.Information);
            
            switch(result)
            {
                case MessageBoxResult.Yes:
                    switch (tam.Tag)
                    {
                        case 1:
                            dimensions = 8;
                            break;
                        case 2:
                            dimensions = 16;
                            break;
                        case 3:
                            dimensions = 32;
                            break;

                    }
                    GeneraGrid();
                    break;
                case MessageBoxResult.No:
                    break;
            }

            
        }

        

        

        private void GridDibujoMouseClickDerecho(object sender, MouseButtonEventArgs e)
        {
            Border b1 = new Border();
            b1.BorderThickness = new Thickness(1);
            b1.Background = new SolidColorBrush(Colors.White);
            b1.BorderBrush = new SolidColorBrush(Colors.Gray);
            Grid.SetColumn(b1, posicionColumna);
            Grid.SetRow(b1, posicionCelda);
            GridDibujo.Children.Add(b1);
        }

        private void ExportToPNGClick(object sender, RoutedEventArgs e)
        {
            string path = @"./Out/";
            Button botonExport = (Button)sender;
            try
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)GridDibujo.ActualWidth, (int)GridDibujo.ActualHeight, 0, 0, PixelFormats.Pbgra32);
                bitmap.Render(GridDibujo);
                
                switch (botonExport.Tag)
                {
                    case 1:
                        FileStream stream = File.Create("./Out/prueba.png");
                        
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        
                        encoder.Frames.Add(BitmapFrame.Create(bitmap));
                        encoder.Save(stream);


                        stream.Close();
                        break;
                        
                    case 2:

                        FileStream stream2 = File.Create("./Out/prueba.tiff");
                        TiffBitmapEncoder encoder2 = new TiffBitmapEncoder();
                        encoder2.Frames.Add(BitmapFrame.Create(bitmap));
                        encoder2.Save(stream2);
                        stream2.Close();
                        break;

                }
                
                    MessageBox.Show("Se exporto correctamente");
                

            }
            catch (Exception m)
            {

                MessageBox.Show("Hubo un error al exportar el archivo: " + m.Message);
            }
            
        }

        public void ColorPersonalizado()
        {
            if(ColorCustomTexto.Text == string.Empty || ColorCustomTexto.Text.Length < 5)
            {
                pincelColor = new SolidColorBrush(ColorPicker.SelectedColor);
            }
            else
            {
                try
                {
                    String[] temp = ColorCustomTexto.Text.Split(' ');
                    int[] RGB = new int[3];
                    for (int i = 0; i < RGB.Length; i++)
                    {

                        RGB[i] = int.Parse(temp[i]);

                    }

                    pincelColor = new SolidColorBrush(Color.FromRgb((byte)RGB[0], (byte)RGB[1], (byte)RGB[2]));



                }
                catch (Exception e)
                {

                    MessageBox.Show("No se ha podido asignar el color correctamente, use un formato RGB (255 44 0)" + e.Message);
                }

            }
            

        }

        private void ColorButtonChecked(object sender, RoutedEventArgs e)
        {
            
            RadioButton selectColor = (RadioButton)sender;

            switch (selectColor.Tag)
            {
                case 1:
                
                    pincelColor = new SolidColorBrush(Colors.Black);
                    break;
                case 2:

                    pincelColor = new SolidColorBrush(Colors.White);
                    
                    break;
                case 3:
                    pincelColor = new SolidColorBrush(Colors.Red);
                    break;
                case 4:
                    pincelColor = new SolidColorBrush(Colors.Green);
                    break;
                case 5:
                    pincelColor = new SolidColorBrush(Colors.Blue);
                    break;
                case 6:
                    pincelColor = new SolidColorBrush(Colors.Yellow);
                    break;
                case 7:
                    pincelColor = new SolidColorBrush(Colors.Orange);
                    break;
                case 8:
                    pincelColor = new SolidColorBrush(Colors.Pink);
                    break;
                case 9:
                    ColorPersonalizado();
                    break;
                default:
                    pincelColor = new SolidColorBrush(Colors.Black);
                    break;



            }

        }

        private void RellenarButtonClicked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {

                    Border b1 = new Border();
                    
                    
                    b1.Background = pincelColor;

                    Grid.SetColumn(b1, j);
                    Grid.SetRow(b1, i);

                    GridDibujo.Children.Add(b1);


                }

            }
        }

        private void OpenURL(object sender, MouseButtonEventArgs e)
        {
            
            System.Diagnostics.Process.Start("https://github.com/GyroZeppeli1995");

        }

        

        

        private void ColorPickerColorChanged(object sender, RoutedEventArgs e)
        {
            if(ColorCustom.IsChecked == true)
            {
                pincelColor = new SolidColorBrush(ColorPicker.SelectedColor);

            }
            
            
            
        }

        private void ColorCustomTextoChanged(object sender, TextChangedEventArgs e)
        {
            if(ColorCustom.IsChecked == true && ColorCustomTexto.Text.Length >= 5)
            {
                ColorPersonalizado();

            }
        }
    }
}
