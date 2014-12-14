using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.PSD;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PuzzleSprite.Parsers.PSD {
	internal class PSDParser : IPSDParser {

		public Bitmap PsdToBitmap(string filePath) {

			PsdFile psd = new PsdFile();
			psd.Load(filePath);

			Bitmap bitmap = new System.Drawing.Bitmap(psd.Columns, psd.Rows);
			using(Graphics g = Graphics.FromImage(bitmap)) {
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.CompositingMode = CompositingMode.SourceOver;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;

				g.DrawImage(DecodeImage(psd), new Point(0, 0));
			}

			return bitmap;

		}

		#region Image methods

		public static Bitmap DecodeImage(PsdFile psdFile) {
			Bitmap bitmap = new Bitmap(psdFile.Columns, psdFile.Rows, PixelFormat.Format32bppArgb);

			Parallel.For(0, psdFile.Rows, y => {
				Int32 rowIndex = y * psdFile.Columns;

				for(Int32 x = 0; x < psdFile.Columns; x++) {
					Int32 pos = rowIndex + x;

					Color pixelColor = GetColor(psdFile, pos);

					lock(bitmap) {
						bitmap.SetPixel(x, y, pixelColor);
					}
				}
			});

			return bitmap;
		}

		#endregion Image methods

		#region Colors methods

		private static Color GetColor(PsdFile psdFile, Int32 pos) {
			Color color = Color.Transparent;

			var r = psdFile.ImageData[0][pos];
			var g = psdFile.ImageData[1][pos];
			var b = psdFile.ImageData[2][pos];
			var a = psdFile.ImageData[3][pos];

			switch(psdFile.ColorMode) {
				case PsdFile.ColorModes.RGB:
					color = Color.FromArgb(a, r, g, b);
					break;
				case PsdFile.ColorModes.CMYK:
					color = CMYKToRGB(r, g, b, a);
					break;
				case PsdFile.ColorModes.Multichannel:
					color = CMYKToRGB(r, g, b, 0);
					break;
				case PsdFile.ColorModes.Grayscale:
				case PsdFile.ColorModes.Duotone:
					color = Color.FromArgb(r, r, r);
					break;
				case PsdFile.ColorModes.Indexed: {
						Int32 index = r;
						color = Color.FromArgb(psdFile.ColorModeData[index], psdFile.ColorModeData[index + 256], psdFile.ColorModeData[index + 2 * 256]);
					}
					break;
				case PsdFile.ColorModes.Lab:
					color = LabToRGB(r, g, b);
					break;
			}

			return color;
		}

		private static Color LabToRGB(Byte lb, Byte ab, Byte bb) {
			Double exL = lb;
			Double exA = ab;
			Double exB = bb;

			const Double lCoef = 256.0 / 100.0;
			const Double aCoef = 256.0 / 256.0;
			const Double bCoef = 256.0 / 256.0;

			Int32 l = (Int32)(exL / lCoef);
			Int32 a = (Int32)(exA / aCoef - 128.0);
			Int32 b = (Int32)(exB / bCoef - 128.0);

			// For the conversion we first convert values to XYZ and then to RGB
			// Standards used Observer = 2, Illuminant = D65

			const Double refX = 95.047;
			const Double refY = 100.000;
			const Double refZ = 108.883;

			Double varY = (l + 16.0) / 116.0;
			Double varX = a / 500.0 + varY;
			Double varZ = varY - b / 200.0;

			varY = Math.Pow(varY, 3) > 0.008856 ? Math.Pow(varY, 3) : (varY - 16 / 116) / 7.787;
			varX = Math.Pow(varX, 3) > 0.008856 ? Math.Pow(varX, 3) : (varX - 16 / 116) / 7.787;
			varZ = Math.Pow(varZ, 3) > 0.008856 ? Math.Pow(varZ, 3) : (varZ - 16 / 116) / 7.787;

			Double x = refX * varX;
			Double y = refY * varY;
			Double z = refZ * varZ;

			return XYZToRGB(x, y, z);
		}
		private static Color XYZToRGB(Double x, Double y, Double z) {

			Double varX = x / 100.0;
			Double varY = y / 100.0;
			Double varZ = z / 100.0;

			Double varR = varX * 3.2406 + varY * (-1.5372) + varZ * (-0.4986);
			Double varG = varX * (-0.9689) + varY * 1.8758 + varZ * 0.0415;
			Double varB = varX * 0.0557 + varY * (-0.2040) + varZ * 1.0570;

			varR = varR > 0.0031308 ? 1.055 * (Math.Pow(varR, 1 / 2.4)) - 0.055 : 12.92 * varR;
			varG = varG > 0.0031308 ? 1.055 * (Math.Pow(varG, 1 / 2.4)) - 0.055 : 12.92 * varG;
			varB = varB > 0.0031308 ? 1.055 * (Math.Pow(varB, 1 / 2.4)) - 0.055 : 12.92 * varB;

			Int32 nRed = (Int32)(varR * 256.0);
			Int32 nGreen = (Int32)(varG * 256.0);
			Int32 nBlue = (Int32)(varB * 256.0);

			nRed = nRed > 0 ? nRed : 0;
			nRed = nRed < 255 ? nRed : 255;

			nGreen = nGreen > 0 ? nGreen : 0;
			nGreen = nGreen < 255 ? nGreen : 255;

			nBlue = nBlue > 0 ? nBlue : 0;
			nBlue = nBlue < 255 ? nBlue : 255;

			return Color.FromArgb(nRed, nGreen, nBlue);
		}

		///////////////////////////////////////////////////////////////////////////////
		//
		// The algorithms for these routines were taken from:
		//     http://www.neuro.sfc.keio.ac.jp/~aly/polygon/info/color-space-faq.html
		//
		// RGB --> CMYK                              CMYK --> RGB
		// ---------------------------------------   --------------------------------------------
		// Black   = minimum(1-Red,1-Green,1-Blue)   Red   = 1-minimum(1,Cyan*(1-Black)+Black)
		// Cyan    = (1-Red-Black)/(1-Black)         Green = 1-minimum(1,Magenta*(1-Black)+Black)
		// Magenta = (1-Green-Black)/(1-Black)       Blue  = 1-minimum(1,Yellow*(1-Black)+Black)
		// Yellow  = (1-Blue-Black)/(1-Black)
		//

		private static Color CMYKToRGB(Byte c, Byte m, Byte y, Byte k) {
			Double dMaxColours = Math.Pow(2, 8);

			Double exC = c;
			Double exM = m;
			Double exY = y;
			Double exK = k;

			Double C = (1.0 - exC / dMaxColours);
			Double M = (1.0 - exM / dMaxColours);
			Double Y = (1.0 - exY / dMaxColours);
			Double K = (1.0 - exK / dMaxColours);

			Int32 nRed = (Int32)((1.0 - (C * (1 - K) + K)) * 255);
			Int32 nGreen = (Int32)((1.0 - (M * (1 - K) + K)) * 255);
			Int32 nBlue = (Int32)((1.0 - (Y * (1 - K) + K)) * 255);

			nRed = nRed > 0 ? nRed : 0;
			nRed = nRed < 255 ? nRed : 255;

			nGreen = nGreen > 0 ? nGreen : 0;
			nGreen = nGreen < 255 ? nGreen : 255;

			nBlue = nBlue > 0 ? nBlue : 0;
			nBlue = nBlue < 255 ? nBlue : 255;

			return Color.FromArgb(nRed, nGreen, nBlue);
		}


		#endregion Colors methods

	}



}
