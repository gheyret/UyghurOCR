using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UyghurOCR
{

    public class AnnotationRectangle
    {
        public Rectangle ImageRect { get; set; }
        public Rectangle CanvasRect { get; set; }
        public Pen Pen { get; set; }
        public bool IsSelected { get; set; }

        public AnnotationRectangle(Rectangle imageRect, Rectangle canvasRect, Color color, float width)
        {
            ImageRect = imageRect;
            CanvasRect = canvasRect;
            Pen = new Pen(color, width);
        }

        public void UpdatePen(Color color, float width)
        {
            Pen.Dispose();
            Pen = new Pen(color, width);
        }
    }

    public class ResimRamka2 : UserControl, IDisposable
    {
        public delegate void dlSeletedImage(Bitmap org);
        public delegate void dlSelectedArea(int x, int y, int w, int h);
        public delegate void dlMousePoint(int x, int y);

        private Image _originalImage;
        private Image _displayImage;
        private Rectangle _imageBounds;
        private List<AnnotationRectangle> _rectangles = new List<AnnotationRectangle>();
        private AnnotationRectangle _selectedRectangle;
        private Rectangle? _currentRect;
        private Point _startPoint;
        private string _resizeMode;
        private List<Rectangle> _resizeHandles = new List<Rectangle>();
        private readonly Pen _currentRectPen = new Pen(Color.Red, 2);
        private const int HandleSize = 6;

        public dlSelectedArea SelectedArea = null;

        public ResimRamka2()
        {
            DoubleBuffered = true;
            BackColor = Color.Gray;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
            Paint += OnPaint;
            Resize += OnResize;
        }

        public Bitmap Image
        {
            get { return (Bitmap)_originalImage; }
            set
            {
                _originalImage?.Dispose();
                _originalImage = value;
                ResizeImage();
                _rectangles.Clear();
                _selectedRectangle = null;
                _currentRect = null;
                _resizeHandles.Clear();
                Invalidate();
            }
        }
        public void CopytoclipBorad()
        {
            Clipboard.SetImage(this.Image);
        }

        public void SetImage(Image newImage)
        {
            _originalImage?.Dispose();
            _originalImage = newImage;
            ResizeImage();
            _rectangles.Clear();
            _selectedRectangle = null;
            _currentRect = null;
            _resizeHandles.Clear();
            Invalidate();
        }

        public Rectangle getRoi()
        {
            if (_rectangles.Count == 0)
            {
                return new Rectangle(0, 0, _originalImage.Width, _originalImage.Height);
            }
            else 
            { 
                return _rectangles[0].ImageRect;
            }
        }

        private void ResizeImage()
        {
            if (_originalImage == null) return;

            int canvasWidth = Width;
            int canvasHeight = Height;
            float ratio = Math.Min((float)canvasWidth / _originalImage.Width, (float)canvasHeight / _originalImage.Height);
            int newWidth = (int)(_originalImage.Width * ratio);
            int newHeight = (int)(_originalImage.Height * ratio);
            if (newHeight > 0 && newWidth > 0)
            {
                _displayImage?.Dispose();
                _displayImage = new Bitmap(_originalImage, newWidth, newHeight);
                int xOffset = (canvasWidth - newWidth) / 2;
                int yOffset = (canvasHeight - newHeight) / 2;
                _imageBounds = new Rectangle(xOffset, yOffset, newWidth, newHeight);

                UpdateCanvasRectangles();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (_displayImage != null)
                g.DrawImage(_displayImage, _imageBounds);

            foreach (var rect in _rectangles)
                g.DrawRectangle(rect.Pen, rect.CanvasRect);

            if (_currentRect.HasValue)
                g.DrawRectangle(_currentRectPen, _currentRect.Value);

            if (_selectedRectangle != null)
                foreach (var handle in _resizeHandles)
                {
                    g.FillRectangle(Brushes.Blue, handle);
                    g.DrawRectangle(Pens.White, handle);
                }
        }

        private void OnResize(object sender, EventArgs e)
        {
            ResizeImage();
            Invalidate();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!IsInsideImage(e.Location)) return;

            if (_selectedRectangle != null && (_resizeMode = CheckResizeHandle(e.Location)) != null)
            {
                _startPoint = e.Location;
                return;
            }

            var rect = FindRectangle(e.Location);
            if (rect != null)
            {
                SelectRectangle(rect);
                _startPoint = e.Location;
                return;
            }

            DeselectRectangle();
            _startPoint = e.Location;
            _currentRect = new Rectangle(e.X, e.Y, 0, 0);

            Invalidate();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_resizeMode != null && _selectedRectangle != null)
            {
                ResizeRectangle(e.Location);
                return;
            }

            if (_selectedRectangle != null && e.Button == MouseButtons.Left)
            {
                MoveRectangle(e.Location);
                return;
            }

            if (_currentRect.HasValue)
            {
                //Eger birla daire tallash zorur bolsa bu yerni echiweting
                if (_currentRect.Value.Width > 5 && _currentRect.Value.Height > 5)
                {
                    _rectangles.Clear();
                }

                Point constrained = ConstrainToImage(e.Location);
                _currentRect = new Rectangle(
                    Math.Min(_startPoint.X, constrained.X),
                    Math.Min(_startPoint.Y, constrained.Y),
                    Math.Abs(constrained.X - _startPoint.X),
                    Math.Abs(constrained.Y - _startPoint.Y));
                Invalidate();
            }

            UpdateCursor(e.Location);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_currentRect.HasValue && _currentRect.Value.Width > 5 && _currentRect.Value.Height > 5)
            {
                var imgRect = CanvasToImageCoords(_currentRect.Value);
                var newRect = new AnnotationRectangle(imgRect, _currentRect.Value, Color.Red, 2);

                _rectangles.Add(newRect);
                SelectRectangle(newRect);
                if (SelectedArea != null)
                {
                    SelectedArea(newRect.ImageRect.X, newRect.ImageRect.Y, newRect.ImageRect.Width, newRect.ImageRect.Height);
                }
            }
            _currentRect = null;
            _resizeMode = null;
            Invalidate();
        }

        private bool IsInsideImage(Point p)
        {
            return _imageBounds.Contains(p);
        }

        private Point ConstrainToImage(Point p)
        {
            return new Point(
                Math.Max(_imageBounds.Left, Math.Min(p.X, _imageBounds.Right)),
                Math.Max(_imageBounds.Top, Math.Min(p.Y, _imageBounds.Bottom)));
        }

        private AnnotationRectangle FindRectangle(Point p)
        {
            foreach (var rect in _rectangles)
                if (rect.CanvasRect.Contains(p))
                    return rect;
            return null;
        }

        private void SelectRectangle(AnnotationRectangle rect)
        {
            DeselectRectangle();
            _selectedRectangle = rect;
            rect.IsSelected = true;
            rect.UpdatePen(Color.Blue, 2);
            DrawResizeHandles(rect.CanvasRect);
            Invalidate();
        }

        private void DeselectRectangle()
        {
            if (_selectedRectangle != null)
            {
                _selectedRectangle.IsSelected = false;
                _selectedRectangle.UpdatePen(Color.Red, 2);
                _selectedRectangle = null;
                _resizeHandles.Clear();
            }
            Invalidate();
        }

        private void DrawResizeHandles(Rectangle rect)
        {
            _resizeHandles.Clear();
            int x1 = rect.X, y1 = rect.Y, x2 = rect.Right, y2 = rect.Bottom;
            _resizeHandles.AddRange(new[]
            {
                new Rectangle(x1 - HandleSize / 2, y1 - HandleSize / 2, HandleSize, HandleSize), // nw
                new Rectangle((x1 + x2) / 2 - HandleSize / 2, y1 - HandleSize / 2, HandleSize, HandleSize), // n
                new Rectangle(x2 - HandleSize / 2, y1 - HandleSize / 2, HandleSize, HandleSize), // ne
                new Rectangle(x2 - HandleSize / 2, (y1 + y2) / 2 - HandleSize / 2, HandleSize, HandleSize), // e
                new Rectangle(x2 - HandleSize / 2, y2 - HandleSize / 2, HandleSize, HandleSize), // se
                new Rectangle((x1 + x2) / 2 - HandleSize / 2, y2 - HandleSize / 2, HandleSize, HandleSize), // s
                new Rectangle(x1 - HandleSize / 2, y2 - HandleSize / 2, HandleSize, HandleSize), // sw
                new Rectangle(x1 - HandleSize / 2, (y1 + y2) / 2 - HandleSize / 2, HandleSize, HandleSize) // w
            });
        }

        private string CheckResizeHandle(Point p)
        {
            string[] modes = { "nw", "n", "ne", "e", "se", "s", "sw", "w" };
            for (int i = 0; i < _resizeHandles.Count; i++)
                if (_resizeHandles[i].Contains(p))
                    return modes[i];
            return null;
        }

        private void ResizeRectangle(Point p)
        {
            if (_selectedRectangle == null) return;
            Point constrained = ConstrainToImage(p);
            Rectangle rect = _selectedRectangle.CanvasRect;
            int x1 = rect.X, y1 = rect.Y, x2 = rect.Right, y2 = rect.Bottom;

            switch (_resizeMode)
            {
                case "nw": x1 = constrained.X; y1 = constrained.Y; break;
                case "n": y1 = constrained.Y; break;
                case "ne": x2 = constrained.X; y1 = constrained.Y; break;
                case "e": x2 = constrained.X; break;
                case "se": x2 = constrained.X; y2 = constrained.Y; break;
                case "s": y2 = constrained.Y; break;
                case "sw": x1 = constrained.X; y2 = constrained.Y; break;
                case "w": x1 = constrained.X; break;
            }

            rect = new Rectangle(Math.Min(x1, x2), Math.Min(y1, y2), Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            _selectedRectangle.CanvasRect = rect;
            _selectedRectangle.ImageRect = CanvasToImageCoords(rect);
            DrawResizeHandles(rect);
            Invalidate();
        }

        private void MoveRectangle(Point p)
        {
            if (_selectedRectangle == null) return;
            int dx = p.X - _startPoint.X;
            int dy = p.Y - _startPoint.Y;
            Rectangle rect = _selectedRectangle.CanvasRect;
            Rectangle newRect = new Rectangle(rect.X + dx, rect.Y + dy, rect.Width, rect.Height);

            if (_imageBounds.Contains(newRect))
            {
                _selectedRectangle.CanvasRect = newRect;
                _selectedRectangle.ImageRect = CanvasToImageCoords(newRect);
                DrawResizeHandles(newRect);
                _startPoint = p;
                Invalidate();
            }
        }

        // マウスカーソルを更新（通常のswitch文で実装）
        private void UpdateCursor(Point p)
        {
            if (_selectedRectangle != null)
            {
                string handle = CheckResizeHandle(p);
                if (handle != null)
                {
                    switch (handle)
                    {
                        case "nw":
                            Cursor = Cursors.SizeNWSE;
                            break;
                        case "n":
                            Cursor = Cursors.SizeNS;
                            break;
                        case "ne":
                            Cursor = Cursors.SizeNESW;
                            break;
                        case "e":
                            Cursor = Cursors.SizeWE;
                            break;
                        case "se":
                            Cursor = Cursors.SizeNWSE;
                            break;
                        case "s":
                            Cursor = Cursors.SizeNS;
                            break;
                        case "sw":
                            Cursor = Cursors.SizeNESW;
                            break;
                        case "w":
                            Cursor = Cursors.SizeWE;
                            break;
                        default:
                            Cursor = Cursors.Default;
                            break;
                    }
                    return;
                }

                if (_selectedRectangle.CanvasRect.Contains(p))
                {
                    Cursor = Cursors.SizeAll;
                    return;
                }
            }
            Cursor = Cursors.Default;
        }

        private void UpdateCanvasRectangles()
        {
            if (_originalImage == null) return;
            float ratioX = (float)_imageBounds.Width / _originalImage.Width;
            float ratioY = (float)_imageBounds.Height / _originalImage.Height;

            foreach (var rect in _rectangles)
            {
                rect.CanvasRect = ImageToCanvasCoords(rect.ImageRect, ratioX, ratioY);
                if (rect.IsSelected)
                    DrawResizeHandles(rect.CanvasRect);
            }
        }

        private Rectangle CanvasToImageCoords(Rectangle canvasRect)
        {
            if (_originalImage == null) return canvasRect;
            float ratioX = (float)_imageBounds.Width / _originalImage.Width;
            float ratioY = (float)_imageBounds.Height / _originalImage.Height;
            int x = (int)((canvasRect.X - _imageBounds.X) / ratioX);
            int y = (int)((canvasRect.Y - _imageBounds.Y) / ratioY);
            int width = (int)(canvasRect.Width / ratioX);
            int height = (int)(canvasRect.Height / ratioY);
            return new Rectangle(x, y, width, height);
        }

        private Rectangle ImageToCanvasCoords(Rectangle imgRect, float ratioX, float ratioY)
        {
            int x = _imageBounds.X + (int)(imgRect.X * ratioX);
            int y = _imageBounds.Y + (int)(imgRect.Y * ratioY);
            int width = (int)(imgRect.Width * ratioX);
            int height = (int)(imgRect.Height * ratioY);
            return new Rectangle(x, y, width, height);
        }

        public new void Dispose()
        {
            _originalImage?.Dispose();
            _displayImage?.Dispose();
            _currentRectPen.Dispose();
            foreach (var rect in _rectangles)
                rect.Pen.Dispose();
            base.Dispose();
        }
        public void DeleteSelectedRectangle()
        {
            if (_selectedRectangle != null)
            {
                _rectangles.Remove(_selectedRectangle);
                _selectedRectangle = null;
                _resizeHandles.Clear();
                Invalidate();
            }
        }

        public void AddRectangle(int x, int y, int width, int height, Color? color = null)
        {
            // 画像サイズをチェック
            if (_originalImage == null)
                return;

            // 画像の範囲内であることを確認
            x = Math.Max(0, Math.Min(x, _originalImage.Width - width));
            y = Math.Max(0, Math.Min(y, _originalImage.Height - height));

            // 画像座標での矩形
            Rectangle imageRect = new Rectangle(x, y, width, height);

            // キャンバス座標に変換
            float ratioX = (float)_imageBounds.Width / _originalImage.Width;
            float ratioY = (float)_imageBounds.Height / _originalImage.Height;
            Rectangle canvasRect = ImageToCanvasCoords(imageRect, ratioX, ratioY);

            // 色の指定がない場合はデフォルトの赤
            Color rectColor = color ?? Color.Red;

            // 新しい矩形を作成して追加
            var newRect = new AnnotationRectangle(imageRect, canvasRect, rectColor, 2);
            _rectangles.Add(newRect);

            // コントロールを再描画
            Invalidate();

            // SelectedAreaイベントを呼び出し（オプション）
            if (SelectedArea != null)
            {
                SelectedArea(x, y, width, height);
            }
        }

        // オーバーロードメソッド：Rectangleオブジェクトから追加
        public void AddRectangle(Rectangle rect, Color? color = null)
        {
            AddRectangle(rect.X, rect.Y, rect.Width, rect.Height, color);
        }

        // 複数の矩形を一度に追加するメソッド
        public void AddRectangles(List<Rectangle> rectangles, Color? color = null)
        {
            foreach (var rect in rectangles)
            {
                AddRectangle(rect, color);
            }
        }
    }
}
