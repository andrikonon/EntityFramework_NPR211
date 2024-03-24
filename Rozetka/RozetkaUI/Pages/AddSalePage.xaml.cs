﻿using BAL.DTO.Models;
using BAL.Interfaces;
using BAL.Services;
using BAL.Utilities;
using DAL.Data.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace RozetkaUI.Pages
{
    /// <summary>
    /// Interaction logic for AddSalePage.xaml
    /// </summary>
    public partial class AddSalePage : Page
    {
        private SaleEntityDTO _sale;
        private Page _prevPage;
        public AddSalePage(Page prevPage, SaleEntityDTO sale = null)
        {
            InitializeComponent();
            _sale = sale;
            _prevPage = prevPage;

            if (_sale != null)
            {
                submit.Content = "Відредагувати";

                saleNameTextBox.Text = _sale.SaleName;
                saleDescriptionTextBox.Text = _sale.SaleDescription;
                decreasePercentTextBox.Text = _sale.DecreasePercent.ToString();
                expireTimeDatePicker.SelectedDate = _sale.ExpireTime;

                if (!String.IsNullOrEmpty(_sale.ImagePath))
                {
                    photosDockPanel.Children.Insert(photosDockPanel.Children.Count - 1, CreatePhoto(_sale.ImagePath));
                    photosDockPanel.Children[1].Visibility = Visibility.Hidden;
                }
            }
        }

        private Border CreatePhoto(string filePath)
        {
            /*
             XAML CODE
             <Border Style="{StaticResource PhotoCard}"
                            Drop="Image_Drop"
                            MouseMove="Image_MouseMove"
                            Name="image1">
                        <Border.Background>
                            <ImageBrush Stretch="UniformToFill" ImageSource="https://storage.ws.pho.to/s2/ba5069b25867b2305fe566efdffa8813bdee34c5_m.jpeg"/>
                        </Border.Background>
                        <Border CornerRadius="7">
                            <Border.Style>
                                <Style>
                                    <Setter Property="Border.Opacity" Value="1"/>
                                    <Setter Property="Border.Visibility" Value="Collapsed"></Setter>
                                    <Style.Triggers>
                                        <DataTrigger Binding="{Binding ElementName=image1, Path=IsMouseOver}" Value="true">
                                            <Setter Property="Border.Visibility" Value="Visible"></Setter>
                                            <Setter Property="Border.Background">
                                                <Setter.Value>
                                                    <SolidColorBrush Color="Black" Opacity="0.6"/>
                                                </Setter.Value>
                                            </Setter>
                                        </DataTrigger>
                                    </Style.Triggers>
                                </Style>
                            </Border.Style>
                            <Grid HorizontalAlignment="Stretch"
                                  VerticalAlignment="Stretch">
                                <Button Style="{StaticResource CardButton}"
                                    VerticalAlignment="Bottom" 
                                    Margin="31,0,67,10">
                                    <Path Data="{StaticResource EditImage}"
                                      Stretch="Uniform" 
                                      Margin="-1 0 0 -2"
                                      Fill="{StaticResource PrimaryBackgroundColor}" 
                                      Width="17" Height="17"
                                      HorizontalAlignment="Center">
                                        <Path.LayoutTransform>
                                            <RotateTransform CenterX="0" CenterY="0" Angle="180" />
                                        </Path.LayoutTransform>
                                    </Path>
                                </Button>
                                <Button Style="{StaticResource CardButton}"
                                    VerticalAlignment="Bottom"
                                    Margin="67,0,31,10">
                                    <Path Data="{StaticResource Delete}"
                                      Stretch="Uniform" 
                                      Fill="{StaticResource PrimaryBackgroundColor}" 
                                      Width="17" Height="17"
                                      HorizontalAlignment="Center">
                                        <Path.LayoutTransform>
                                            <RotateTransform CenterX="0" CenterY="0" Angle="180" />
                                        </Path.LayoutTransform>
                                    </Path>
                                </Button>
                            </Grid>
                        </Border>
                    </Border>
             */

            var main = new Border();
            main.Style = this.Resources["PhotoCard"] as Style;
            main.Name = $"image1";
            var image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri(filePath));
            image.Stretch = Stretch.UniformToFill;
            main.Background = image;

            var child = new Border();
            child.CornerRadius = new CornerRadius(7);
            var childStyle = new Style();

            /*
             <Border.Style>
                 <Style>
                   <Setter Property="Border.Opacity" Value="1"/>
                   <Setter Property="Border.Visibility" Value="Collapsed"></Setter>
                   <Style.Triggers>
                        <DataTrigger Binding="{Binding ElementName=image1, Path=IsMouseOver}" Value="true">
                              <Setter Property="Border.Visibility" Value="Visible"></Setter>
                              <Setter Property="Border.Background">
                                  <Setter.Value>
                                       SolidColorBrush Color="Black" Opacity="0.6"/>
                                  </Setter.Value>
                              </Setter>
                          </DataTrigger>
                      </Style.Triggers>
                  </Style>
             </Border.Style>               
             */

            childStyle.Setters.Add(new Setter(Border.OpacityProperty, 1.0));
            childStyle.Setters.Add(new Setter(Border.VisibilityProperty, Visibility.Collapsed));
            var dataTrigger = new DataTrigger()
            {
                Binding = new Binding()
                {
                    Path = new PropertyPath("IsMouseOver"),
                    Source = main
                },
                Value = Boolean.TrueString
            };
            dataTrigger.Setters.Add(new Setter(Border.VisibilityProperty, Visibility.Visible));
            dataTrigger.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush() { Color = Color.FromRgb(0, 0, 0), Opacity = 0.6 }));
            childStyle.Triggers.Add(dataTrigger);

            child.Style = childStyle;

            var grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;

            var edit = new Button()
            {
                Style = this.Resources["CardButton"] as Style,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(31, 0, 67, 10),
                Content = new System.Windows.Shapes.Path()
                {
                    Data = this.FindResource("EditImage") as PathGeometry,
                    Stretch = Stretch.Uniform,
                    Margin = new Thickness(-1, 0, 0, -2),
                    Fill = this.FindResource("PrimaryBackgroundColor") as SolidColorBrush,
                    Width = 17,
                    Height = 17,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LayoutTransform = new RotateTransform()
                    {
                        CenterX = 0,
                        CenterY = 0,
                        Angle = 180,
                    }
                },
                Name = $"{main.Name}Edit"
            };
            edit.Click += ChangePhoto;
            grid.Children.Add(edit);
            var delete = new Button()
            {
                Style = this.Resources["CardButton"] as Style,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(67, 0, 31, 10),
                Content = new System.Windows.Shapes.Path()
                {
                    Data = this.FindResource("Delete") as PathGeometry,
                    Stretch = Stretch.Uniform,
                    Fill = this.FindResource("PrimaryBackgroundColor") as SolidColorBrush,
                    Width = 17,
                    Height = 17,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LayoutTransform = new RotateTransform()
                    {
                        CenterX = 0,
                        CenterY = 0,
                        Angle = 180,
                    }
                },
                Name = $"{main.Name}Delete"
            };
            delete.Click += DeletePhoto;
            grid.Children.Add(delete);
            child.Child = grid;

            child.Child = grid;

            main.Child = child;

            return main;
        }

        private void DeletePhoto(object sender, RoutedEventArgs e)
        {
            var mainName = (sender as Button).Name.Substring(0, (sender as Button).Name.IndexOf("Delete"));
            foreach (Border photo in photosDockPanel.Children)
            {
                if (photo.Name == mainName)
                {
                    photosDockPanel.Children[1].Visibility = Visibility.Visible;
                    photosDockPanel.Children.Remove(photo);
                    break;
                }
            }
        }
        private void ChangePhoto(object sender, RoutedEventArgs e)
        {
            var mainName = (sender as Button).Name.Substring(0, (sender as Button).Name.IndexOf("Edit"));
            int index = 0;
            Border realPhoto = null;
            foreach (Border photo in photosDockPanel.Children)
            {
                if (photo.Name == mainName)
                {
                    index = photosDockPanel.Children.IndexOf(photo);
                    realPhoto = photo;
                    break;
                }
            }

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() != false)
            {
                photosDockPanel.Children.Remove(realPhoto);
                photosDockPanel.Children.Insert(index, CreatePhoto(fileDialog.FileName));
                photosDockPanel.Children[1].Visibility = Visibility.Hidden;
            }
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            if (e.Effects != DragDropEffects.None)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                photosDockPanel.Children.Insert(photosDockPanel.Children.Count - 1, CreatePhoto(files[0]));
            }
        }
        public readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
        private void Button_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }



            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 1)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            if (ImageExtensions.Contains(System.IO.Path.GetExtension(files[0]).ToUpperInvariant()))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
        }

        private void LoadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() != false)
            {
                photosDockPanel.Children.Insert(photosDockPanel.Children.Count - 1, CreatePhoto(fileDialog.FileName));
                photosDockPanel.Children[1].Visibility = Visibility.Hidden;
            }
        }

        private void ReturnBackClick(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).pageFrame.Navigate(_prevPage);
        }

        private async void add_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            returnBack.IsEnabled = false;

            var name = saleNameTextBox.Text;
            var description = saleDescriptionTextBox.Text;
            int decreasePercent;
            try
            {
                decreasePercent = int.Parse(decreasePercentTextBox.Text);
                if (decreasePercent < 0)
                    throw new Exception();
            }
            catch (Exception)
            {
                decreasePercentTextBox.BorderBrush = Brushes.Red;
                decreasePercentTextBox.Focus();
                return;
            }

            var expireTime = expireTimeDatePicker.SelectedDate;

            string photo = "";
            try
            {
                photo = ((photosDockPanel.Children[0] as Border).Background as ImageBrush).ImageSource.ToString();
                photo = photo.Substring(8);
            }
            catch
            {

            }

            if (name.Length == 0)
            {
                saleNameTextBox.BorderBrush = Brushes.Red;
                saleNameTextBox.Focus();
                return;
            }

            SaleService saleService = new SaleService();
            if (_sale == null)
            {
                if (photo != null)
                    photo = PhotoSaver.UploadImage(File.ReadAllBytes(photo));

                var sale = new SaleEntityDTO()
                {
                    SaleName = name,
                    SaleDescription = description,
                    ImagePath = photo,
                    DateCreated = DateTime.Now,
                    DecreasePercent= decreasePercent,
                    ExpireTime = expireTime.Value,
                    Sales_Products = new List<Sales_ProductEntityDTO>()
                };
                await saleService.CreateSale(sale);

                if (_prevPage != null)
                {
                    if (_prevPage.GetType() == typeof(AdminPanelPage))
                    {
                        (_prevPage as AdminPanelPage).Sales.Add(sale);
                        CollectionViewSource.GetDefaultView((_prevPage as AdminPanelPage).Sales).Refresh();
                    }
                }

                saleNameTextBox.Text = "";
                saleDescriptionTextBox.Text = "";

                photosDockPanel.Children.RemoveRange(0, photosDockPanel.Children.Count - 1);
                photosDockPanel.Children[0].Visibility = Visibility.Visible;

                decreasePercentTextBox.Text = "";
                expireTimeDatePicker.SelectedDate = null;

                var timer = new System.Timers.Timer();
                (sender as Button).Content = "Успішно добавлено";
                timer.Interval = 5000;
                timer.Elapsed += (s, e) =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        (sender as Button).Content = "Добавити акцію";
                        timer.Stop();
                    });
                };
                timer.Start();
            }
            else
            {
                if (photo != null)
                {
                    if (!photo.Contains(@"http://") && !photo.Contains(@"localhost:5006") && !photo.Contains(@"rozetka.com") && !photo.Contains(@"media.istockphoto.com"))
                        photo = PhotoSaver.UploadImage(File.ReadAllBytes(photo));
                    else
                        photo = "https://" + photo;
                }

                _sale.ImagePath = photo;
                _sale.SaleName = name;
                _sale.DecreasePercent = decreasePercent;
                _sale.ExpireTime = expireTime.Value;
                _sale.SaleDescription = description;

                await saleService.EditSale(_sale);

                if (_prevPage != null)
                {
                    if (_prevPage.GetType() == typeof(AdminPanelPage))
                    {
                        var cat = (_prevPage as AdminPanelPage).Sales.First(c => c.Id == _sale.Id);
                        cat.SaleName = _sale.SaleName;
                        cat.SaleDescription = _sale.SaleDescription;
                        cat.DecreasePercent = _sale.DecreasePercent;
                        cat.ExpireTime = _sale.ExpireTime;
                        cat.ImagePath = _sale.ImagePath;
                        CollectionViewSource.GetDefaultView((_prevPage as AdminPanelPage).Sales).Refresh();
                    }
                }

                var timer = new System.Timers.Timer();
                (sender as Button).Content = "Успішно відредаговано";
                timer.Interval = 5000;
                timer.Elapsed += (s, e) =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        (sender as Button).Content = "Відредагувати";
                        timer.Stop();
                    });
                };
                timer.Start();
            }

            (sender as Button).IsEnabled = true;
            returnBack.IsEnabled = true;
        }
    }
}
