﻿using System.Windows;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = new PianoGridGenerator(WhiteKeysGrid, BlackKeysGrid, 28);
        }
    }
}
