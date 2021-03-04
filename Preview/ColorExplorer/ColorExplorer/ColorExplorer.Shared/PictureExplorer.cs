using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views.UWP;
using Windows.Graphics.Imaging;
using Windows.Storage;
using SkiaSharp;

using System.IO;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ColorExplorer
{
    public partial class PictureExplorer: SKXamlCanvas
    {
        public PictureExplorer()
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.PointerMoved += PictureExplorer_PointerMoved;
        }

        private double _renderedWidth = 0.0;
        private double _renderedHeight = 0.0;
        private double _scaleX = 1.0;
        private double _scaleY = 1.0;

        private void PictureExplorer_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(this);
            var pos = p.Position;

            var scaledX = pos.X * _scaleX;
            var scaledY = pos.Y * _scaleY;

            if (_bitmap != null && _renderedWidth > 0 && _renderedHeight > 0)
            {

                var scale = Math.Min((double)_renderedWidth / _bitmap.Width,
                               (double)_renderedHeight / _bitmap.Height);

                var left = (_renderedWidth - scale * _bitmap.Width) / 2;
                var top = (_renderedHeight - scale * _bitmap.Height) / 2;
                var right = left + scale * _bitmap.Width;
                var bottom = top + scale * _bitmap.Height;
                SKRect rect = new SKRect((float)left, (float)top, (float)right, (float)bottom);

                if (scaledX >= left && scaledX <= right &&
                    scaledY >= top && scaledY <= bottom)
                {
                    var u = (scaledX - left) / (right - left);
                    var v = (scaledY - top) / (bottom - top);

                    var px = u * _bitmap.Width;
                    var py = v * _bitmap.Height;

                    if (px >= 0 && px < _bitmap.Width &&
                        py >=0 && py < _bitmap.Height)
                    {
                        var pix = _bitmap.GetPixel((int)px, (int)py);
                        if (this.PixelHovered != null)
                        {
                            this.PixelHovered(this, new PixelHoveredEventArgs()
                            {
                                Color = Windows.UI.Color.FromArgb(pix.Alpha, pix.Red, pix.Green, pix.Blue)
                            });
                        }
                    }
                }
            }
        }

        public event PixelHoveredEventHandler PixelHovered;

        private Uri _pictureUri;
        public Uri PictureUri { 
            get
            {
                return _pictureUri;
            }
            set
            {
                _pictureUri = value;
                OnPictureUriChanged();
            }
        }

        private SKBitmap _bitmap;

        private async void OnPictureUriChanged()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(PictureUri);
            Stream imagestream = await file.OpenStreamForReadAsync();

            using (MemoryStream memStream = new MemoryStream())
            {
                await imagestream.CopyToAsync(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                _bitmap = SKBitmap.Decode(memStream);
                this.Invalidate();
            };
        }

        

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var surface = e.Surface;
            var surfaceWidth = e.Info.Width;
            var surfaceHeight = e.Info.Height;

            var canvas = surface.Canvas;

            _scaleX = (double)e.Info.Width / ActualWidth;
            _scaleY = (double)e.Info.Height / ActualHeight;
            _renderedWidth = e.Info.Width;
            _renderedHeight = e.Info.Height;

            if (_bitmap != null)
            {

                var scale = Math.Min((double)e.Info.Width / _bitmap.Width,
                               (double)e.Info.Height / _bitmap.Height);

                var left = (e.Info.Width - scale * _bitmap.Width) / 2;
                var top = (e.Info.Height - scale * _bitmap.Height) / 2;
                var right = left + scale * _bitmap.Width;
                var bottom = top + scale * _bitmap.Height;
                SKRect rect = new SKRect((float)left, (float)top, (float)right, (float)bottom);

                canvas.DrawBitmap(_bitmap, rect);
            }

            canvas.Flush();
        }
    }

    public delegate void PixelHoveredEventHandler(object sender, PixelHoveredEventArgs args);

    public class PixelHoveredEventArgs
    {
        public Windows.UI.Color Color { get; internal set;  }
    }
}
