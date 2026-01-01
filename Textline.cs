using OpenCvSharp;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Clipper2Lib;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace UyghurOCR
{
    public class TextDetector
    {
        private readonly int minSize = 2;
        private readonly float thresh = 0.2f;
        private readonly float boxThresh = 0.5f;
        private readonly int shortestSize = 1280;
        private readonly int limitSize = 1600;
        private readonly float unclipRatio = 5.0f;
        private readonly int maxCandidates = 1500;
        private readonly float[] rgb = { 0.485f, 0.456f, 0.406f };
        private readonly float[] std = { 0.229f, 0.224f, 0.225f };
        private readonly InferenceSession sess = null;
        private readonly string inputName;
        public TextDetector()
        {
            string modelPath = Path.Combine(Application.StartupPath,"model.onnx");
            sess = new InferenceSession(modelPath);
            inputName = sess.InputMetadata.Keys.First();
        }

        public Mat ResizeShortestEdge(Mat img)
        {
            int h = img.Height;
            int w = img.Width;
            float scale = (float)shortestSize / Math.Min(h, w);
            int newH, newW;

            if (h < w)
            {
                newH = shortestSize;
                newW = (int)(w * scale);
            }
            else
            {
                newH = (int)(h * scale);
                newW = shortestSize;
            }

            if (Math.Max(newH, newW) > limitSize)
            {
                scale = (float)limitSize / Math.Max(newH, newW);
                newH = (int)(newH * scale);
                newW = (int)(newW * scale);
            }

            newW = Math.Max((newW / 32) * 32, 32);
            newH = Math.Max((newH / 32) * 32, 32);

            return img.Resize(new OpenCvSharp.Size(newW, newH), 0, 0, InterpolationFlags.Area).CvtColor(ColorConversionCodes.BGR2RGB);
        }
        public DenseTensor<float> Preprocess(Mat img)
        {
            var imgCopy = img.Clone();
            var resized = ResizeShortestEdge(imgCopy);
            int h = resized.Height;
            int w = resized.Width;
            var tensor = new DenseTensor<float>(new[] { 1, 3, h, w });
            // 画像データをテンソルに変換
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Vec3b pixel = resized.At<Vec3b>(y, x);
                    tensor[0, 0, y, x] = (pixel[0] / 255.0f - rgb[0]) / std[0];
                    tensor[0, 1, y, x] = (pixel[1] / 255.0f - rgb[1]) / std[1];
                    tensor[0, 2, y, x] = (pixel[2] / 255.0f - rgb[2]) / std[2];
                }
            }
            imgCopy.Dispose();
            resized.Dispose();
            return tensor;
        }
        public (List<OpenCvSharp.Point[]>, float[]) Detect(Mat img)
        {
            List<OpenCvSharp.Point[]> boxes;
            float[] scores;

            int oriH = img.Height;
            int oriW = img.Width;
            var inputTensor = Preprocess(img);

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(inputName, inputTensor) };
            var results = sess.Run(inputs);
            inputs.Clear();

            var pred = results.First().AsTensor<float>();

            // Reshape pred to (H, W)
            int predH = pred.Dimensions[2];
            int predW = pred.Dimensions[3];
            var pred2D = new float[predH, predW];
            var binary = new Mat(predH, predW, MatType.CV_8UC1);
            for (int i = 0; i < predH; i++)
            {
                for (int j = 0; j < predW; j++)
                {
                    float vall = pred[0, 0, i, j];
                    pred2D[i, j] = vall;
                    binary.At<byte>(i, j) = (byte)(vall>thresh ? 255 : 0);
                }
            }
            (boxes,scores) = BoxesFromBitmap(pred2D, binary, oriW, oriH);
            binary.Dispose();
            return (boxes,scores);
        }

        private (List<OpenCvSharp.Point[]>, float[]) BoxesFromBitmap(float[,] pred, Mat binary, int destWidth, int destHeight)
        {
            int height = binary.Height;
            int width = binary.Width;

            var contours = Cv2.FindContoursAsArray(binary, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
            int numContours = Math.Min(contours.Length, maxCandidates);

            var boxes = new List<OpenCvSharp.Point[]>();
            var scores = new List<float>();

            for (int i = 0; i < numContours; i++)
            {
                var contour = contours[i].Select(p => new OpenCvSharp.Point(p.X, p.Y)).ToArray();
                var (points, sside) = GetMiniBoxes(contour);

                if (sside < minSize)
                    continue;

                float score = BoxScoreFast(pred, contour);
                if (boxThresh > score)
                    continue;

                var box = Unclip(points, unclipRatio).SelectMany(arr => arr).ToArray();
                var (newBox, newSside) = GetMiniBoxes(box);
                if (newSside < minSize + 2)
                    continue;

                // Scale to original image size
                var scaledBox = newBox.Select(p => new OpenCvSharp.Point(
                    Clamp((int)Math.Round(p.X * destWidth / (float)width), 0, destWidth),
                    Clamp((int)Math.Round(p.Y * destHeight / (float)height), 0, destHeight)
                )).ToArray();

                boxes.Add(scaledBox);
                scores.Add(score);
            }
            
            return (boxes, scores.ToArray());
        }

        private OpenCvSharp.Point[][] Unclip(OpenCvSharp.Point[] box, float unclipRatio)
        {
            // Calculate polygon area and perimeter
            float area = 0;
            float perimeter = 0;
            for (int i = 0; i < box.Length; i++)
            {
                int j = (i + 1) % box.Length;
                area += box[i].X * box[j].Y - box[j].X * box[i].Y;
                perimeter += (float)Math.Sqrt(Math.Pow(box[j].X - box[i].X, 2) + Math.Pow(box[j].Y - box[i].Y, 2));
            }
            area = Math.Abs(area / 2.0f);

            float width = box.Max(p => p.X) - box.Min(p => p.X);
            float height = box.Max(p => p.Y) - box.Min(p => p.Y);
            float boxDist = Math.Min(width, height);
            float ratio = unclipRatio / (float)Math.Sqrt(boxDist);
            float distance = area * ratio / perimeter;

            // Use Clipper2 for offset
            var path = new Path64(box.Select(p => new Point64(p.X, p.Y)));
            var clipper = new ClipperOffset();
            clipper.AddPath(path, JoinType.Round, EndType.Polygon);
            var solution = new Paths64();
            clipper.Execute(distance, solution);

            return solution.Select(p => p.Select(dp => new OpenCvSharp.Point((int)dp.X, (int)dp.Y)).ToArray()).ToArray();
        }

        private (OpenCvSharp.Point[], float) GetMiniBoxes(OpenCvSharp.Point[] contour)
        {
            var rect = Cv2.MinAreaRect(contour.Select(p => new OpenCvSharp.Point2f(p.X, p.Y)).ToArray());
            var points = Cv2.BoxPoints(rect).Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y)).ToList();

            points.Sort((a, b) => a.X.CompareTo(b.X));
            int index1 = points[1].Y > points[0].Y ? 0 : 1;
            int index4 = index1 == 0 ? 1 : 0;
            int index2 = points[3].Y > points[2].Y ? 2 : 3;
            int index3 = index2 == 2 ? 3 : 2;

            var box = new[] { points[index1], points[index2], points[index3], points[index4] };
            return (box, Math.Min(rect.Size.Width, rect.Size.Height));
        }

        private float BoxScoreFast(float[,] bitmap, OpenCvSharp.Point[] box)
        {
            int h = bitmap.GetLength(0);
            int w = bitmap.GetLength(1);
            int xmin = Clamp((int)Math.Floor((double)box.Min(p => p.X)), 0, w - 1);
            int xmax = Clamp((int)Math.Ceiling((double)box.Max(p => p.X)), 0, w - 1);
            int ymin = Clamp((int)Math.Floor((double)box.Min(p => p.Y)), 0, h - 1);
            int ymax = Clamp((int)Math.Ceiling((double)box.Max(p => p.Y)), 0, h - 1);

            var mask = new Mat(ymax - ymin + 1, xmax - xmin + 1, MatType.CV_8UC1, Scalar.Black);
            var shiftedBox = box.Select(p => new OpenCvSharp.Point(p.X - xmin, p.Y - ymin)).ToArray();
            Cv2.FillPoly(mask, new[] { shiftedBox.Select(p => new OpenCvSharp.Point(p.X, p.Y)) }, Scalar.White);

            float sum = 0;
            int count = 0;
            for (int i = ymin; i <= ymax; i++)
            {
                for (int j = xmin; j <= xmax; j++)
                {
                    if (mask.At<byte>(i - ymin, j - xmin) > 0)
                    {
                        sum += bitmap[i, j];
                        count++;
                    }
                }
            }
            mask.Dispose();
            return count > 0 ? sum / count : 0;
        }
        int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
        public void Dispose()
        {
            if (sess != null)
            {
                sess.Dispose();
            }
        }
    }
}
