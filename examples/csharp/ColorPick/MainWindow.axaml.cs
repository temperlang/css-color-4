using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;

namespace ColorPick;

public partial class MainWindow : Window
{
    private Image _image;
    private TextBlock _rgbText;
    private WriteableBitmap _bitmap;

    public MainWindow()
    {
        Width = 800;
        Height = 600;
        Title = "Image RGB Picker";

        var dock = new DockPanel();

        // Load button
        var btn = new Button { Content = "Load Image", Margin = new Thickness(10) };
        btn.Click += async (s, e) =>
        {
            var dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter { Name = "Images", Extensions = { "png", "jpg", "jpeg", "bmp" } });
            var result = await dialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                LoadImage(result[0]);
            }
        };
        DockPanel.SetDock(btn, Dock.Top);

        // RGB text
        _rgbText = new TextBlock { Margin = new Thickness(10), FontSize = 16 };
        DockPanel.SetDock(_rgbText, Dock.Bottom);

        // Image
        _image = new Image { Stretch = Avalonia.Media.Stretch.Uniform };
        _image.PointerPressed += OnImageClick;

        dock.Children.Add(btn);
        dock.Children.Add(_rgbText);
        dock.Children.Add(_image);

        Content = dock;
    }

    private void LoadImage(string path)
    {
        using (var stream = File.OpenRead(path))
        {
            var bmp = new Bitmap(stream);
            _bitmap = new WriteableBitmap(bmp.PixelSize, bmp.Dpi, Avalonia.Platform.PixelFormat.Bgra8888, AlphaFormat.Premul);
            using (var ctx = _bitmap.Lock())
            {
                bmp.CopyPixels(new PixelRect(bmp.PixelSize), ctx.Address, ctx.RowBytes * _bitmap.PixelSize.Height, ctx.RowBytes);
            }
            _image.Source = _bitmap;
        }
    }

    private void OnImageClick(object sender, PointerPressedEventArgs e)
    {
        if (_bitmap == null) return;

        var pos = e.GetPosition(_image);
        var scaleX = _bitmap.PixelSize.Width / _image.Bounds.Width;
        var scaleY = _bitmap.PixelSize.Height / _image.Bounds.Height;

        int x = (int)(pos.X * scaleX);
        int y = (int)(pos.Y * scaleY);

        if (x < 0 || x >= _bitmap.PixelSize.Width || y < 0 || y >= _bitmap.PixelSize.Height)
            return;

        unsafe
        {
            using (var fb = _bitmap.Lock())
            {
                byte* ptr = (byte*)fb.Address;
                int offset = y * fb.RowBytes + x * 4;
                byte b = ptr[offset];
                byte g = ptr[offset + 1];
                byte r = ptr[offset + 2];

                _rgbText.Text = $"RGB: ({r}, {g}, {b}) at ({x}, {y})";
            }
        }
    }
}
