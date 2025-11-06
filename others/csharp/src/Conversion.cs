// CSS Color Module Level 4 - C# Implementation
// Converted from JavaScript reference implementation
// W3C Candidate Recommendation Draft, 24 April 2025

using System;
using System.Linq;

namespace CssColor4
{
    public static class Conversion
    {
        // Standard white points, defined by 4-figure CIE x,y chromaticities
        public static readonly double[] D50 = new double[] {
            0.3457 / 0.3585,
            1.00000,
            (1.0 - 0.3457 - 0.3585) / 0.3585
        };

        public static readonly double[] D65 = new double[] {
            0.3127 / 0.3290,
            1.00000,
            (1.0 - 0.3127 - 0.3290) / 0.3290
        };

        // sRGB-related functions

        /// <summary>
        /// Convert an array of sRGB values where in-gamut values are in the range [0 - 1]
        /// to linear light (un-companded) form.
        /// Extended transfer function for negative values.
        /// </summary>
        public static double[] LinSrgb(double[] rgb)
        {
            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);

                if (abs <= 0.04045)
                {
                    return val / 12.92;
                }

                return sign * Math.Pow((abs + 0.055) / 1.055, 2.4);
            }).ToArray();
        }

        /// <summary>
        /// Convert an array of linear-light sRGB values in the range 0.0-1.0
        /// to gamma corrected form.
        /// </summary>
        public static double[] GamSrgb(double[] rgb)
        {
            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);

                if (abs > 0.0031308)
                {
                    return sign * (1.055 * Math.Pow(abs, 1.0 / 2.4) - 0.055);
                }

                return 12.92 * val;
            }).ToArray();
        }

        /// <summary>
        /// Convert linear-light sRGB to CIE XYZ using D65 white point
        /// </summary>
        public static double[] LinSrgbToXyz(double[] rgb)
        {
            double[][] m = new double[][]
            {
                new double[] { 506752.0 / 1228815.0,  87881.0 / 245763.0,   12673.0 /   70218.0 },
                new double[] {  87098.0 /  409605.0, 175762.0 / 245763.0,   12673.0 /  175545.0 },
                new double[] {   7918.0 /  409605.0,  87881.0 / 737289.0, 1001167.0 / 1053270.0 }
            };
            return MultiplyMatrices(m, rgb);
        }

        /// <summary>
        /// Convert XYZ to linear-light sRGB
        /// </summary>
        public static double[] XyzToLinSrgb(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] {   12831.0 /   3959.0,    -329.0 /    214.0, -1974.0 /   3959.0 },
                new double[] { -851781.0 / 878810.0, 1648619.0 / 878810.0, 36519.0 / 878810.0 },
                new double[] {     705.0 /  12673.0,   -2585.0 /  12673.0,   705.0 /    667.0 }
            };
            return MultiplyMatrices(m, xyz);
        }

        // Display P3 functions

        public static double[] LinP3(double[] rgb) => LinSrgb(rgb);
        public static double[] GamP3(double[] rgb) => GamSrgb(rgb);

        public static double[] LinP3ToXyz(double[] rgb)
        {
            double[][] m = new double[][]
            {
                new double[] { 608311.0 / 1250200.0, 189793.0 / 714400.0,  198249.0 / 1000160.0 },
                new double[] {  35783.0 /  156275.0, 247089.0 / 357200.0,  198249.0 / 2500400.0 },
                new double[] {      0.0 /       1.0,  32229.0 / 714400.0, 5220557.0 / 5000800.0 }
            };
            return MultiplyMatrices(m, rgb);
        }

        public static double[] XyzToLinP3(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] { 446124.0 / 178915.0, -333277.0 / 357830.0, -72051.0 / 178915.0 },
                new double[] { -14852.0 /  17905.0,   63121.0 /  35810.0,    423.0 /  17905.0 },
                new double[] {  11844.0 / 330415.0,  -50337.0 / 660830.0, 316169.0 / 330415.0 }
            };
            return MultiplyMatrices(m, xyz);
        }

        // ProPhoto RGB functions

        public static double[] LinProPhoto(double[] rgb)
        {
            double et2 = 16.0 / 512.0;
            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);

                if (abs <= et2)
                {
                    return val / 16.0;
                }

                return sign * Math.Pow(abs, 1.8);
            }).ToArray();
        }

        public static double[] GamProPhoto(double[] rgb)
        {
            double et = 1.0 / 512.0;
            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);

                if (abs >= et)
                {
                    return sign * Math.Pow(abs, 1.0 / 1.8);
                }

                return 16.0 * val;
            }).ToArray();
        }

        public static double[] LinProPhotoToXyz(double[] rgb)
        {
            double[][] m = new double[][]
            {
                new double[] { 0.79776664490064230,  0.13518129740053308,  0.03134773412839220 },
                new double[] { 0.28807482881940130,  0.71183523424187300,  0.00008993693872564 },
                new double[] { 0.00000000000000000,  0.00000000000000000,  0.82510460251046020 }
            };
            return MultiplyMatrices(m, rgb);
        }

        public static double[] XyzToLinProPhoto(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] {  1.34578688164715830, -0.25557208737979464, -0.05110186497554526 },
                new double[] { -0.54463070512490190,  1.50824774284514680,  0.02052744743642139 },
                new double[] {  0.00000000000000000,  0.00000000000000000,  1.21196754563894520 }
            };
            return MultiplyMatrices(m, xyz);
        }

        // Adobe RGB (a98-rgb) functions

        public static double[] LinA98rgb(double[] rgb)
        {
            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);
                return sign * Math.Pow(abs, 563.0 / 256.0);
            }).ToArray();
        }

        public static double[] GamA98rgb(double[] rgb)
        {
            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);
                return sign * Math.Pow(abs, 256.0 / 563.0);
            }).ToArray();
        }

        public static double[] LinA98rgbToXyz(double[] rgb)
        {
            double[][] m = new double[][]
            {
                new double[] { 573536.0 /  994567.0,  263643.0 / 1420810.0,  187206.0 /  994567.0 },
                new double[] { 591459.0 / 1989134.0, 6239551.0 / 9945670.0,  374412.0 / 4972835.0 },
                new double[] {  53769.0 / 1989134.0,  351524.0 / 4972835.0, 4929758.0 / 4972835.0 }
            };
            return MultiplyMatrices(m, rgb);
        }

        public static double[] XyzToLinA98rgb(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] { 1829569.0 /  896150.0, -506331.0 /  896150.0, -308931.0 /  896150.0 },
                new double[] { -851781.0 /  878810.0, 1648619.0 /  878810.0,   36519.0 /  878810.0 },
                new double[] {   16779.0 / 1248040.0, -147721.0 / 1248040.0, 1266979.0 / 1248040.0 }
            };
            return MultiplyMatrices(m, xyz);
        }

        // Rec. 2020 functions

        public static double[] Lin2020(double[] rgb)
        {
            double alpha = 1.09929682680944;
            double beta = 0.018053968510807;

            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);

                if (abs < beta * 4.5)
                {
                    return val / 4.5;
                }

                return sign * Math.Pow((abs + alpha - 1.0) / alpha, 1.0 / 0.45);
            }).ToArray();
        }

        public static double[] Gam2020(double[] rgb)
        {
            double alpha = 1.09929682680944;
            double beta = 0.018053968510807;

            return rgb.Select(val =>
            {
                double sign = val < 0 ? -1 : 1;
                double abs = Math.Abs(val);

                if (abs > beta)
                {
                    return sign * (alpha * Math.Pow(abs, 0.45) - (alpha - 1.0));
                }

                return 4.5 * val;
            }).ToArray();
        }

        public static double[] Lin2020ToXyz(double[] rgb)
        {
            double[][] m = new double[][]
            {
                new double[] { 63426534.0 / 99577255.0,  20160776.0 / 139408157.0,  47086771.0 / 278816314.0 },
                new double[] { 26158966.0 / 99577255.0, 472592308.0 / 697040785.0,   8267143.0 / 139408157.0 },
                new double[] {        0.0 /        1.0,  19567812.0 / 697040785.0, 295819943.0 / 278816314.0 }
            };
            return MultiplyMatrices(m, rgb);
        }

        public static double[] XyzToLin2020(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] {  30757411.0 / 17917100.0, -6372589.0 / 17917100.0, -4539589.0 / 17917100.0 },
                new double[] { -19765991.0 / 29648200.0, 47925759.0 / 29648200.0,   467509.0 / 29648200.0 },
                new double[] {    792561.0 / 44930125.0, -1921689.0 / 44930125.0, 42328811.0 / 44930125.0 }
            };
            return MultiplyMatrices(m, xyz);
        }

        // Chromatic adaptation

        public static double[] D65ToD50(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] {  1.0479297925449969,    0.022946870601609652,  -0.05019226628920524  },
                new double[] {  0.02962780877005599,   0.9904344267538799,    -0.017073799063418826 },
                new double[] { -0.009243040646204504,  0.015055191490298152,   0.7518742814281371   }
            };
            return MultiplyMatrices(m, xyz);
        }

        public static double[] D50ToD65(double[] xyz)
        {
            double[][] m = new double[][]
            {
                new double[] {  0.955473421488075,    -0.02309845494876471,   0.06325924320057072  },
                new double[] { -0.0283697093338637,    1.0099953980813041,    0.021041441191917323 },
                new double[] {  0.012314014864481998, -0.020507649298898964,  1.330365926242124    }
            };
            return MultiplyMatrices(m, xyz);
        }

        // CIE Lab and LCH

        public static double[] XyzToLab(double[] xyz)
        {
            double epsilon = 216.0 / 24389.0;  // 6^3/29^3
            double kappa = 24389.0 / 27.0;     // 29^3/3^3

            // Compute xyz scaled relative to reference white
            double[] scaledXyz = new double[]
            {
                xyz[0] / D50[0],
                xyz[1] / D50[1],
                xyz[2] / D50[2]
            };

            // Compute f
            double[] f = scaledXyz.Select(value =>
                value > epsilon ? Math.Pow(value, 1.0 / 3.0) : (kappa * value + 16.0) / 116.0
            ).ToArray();

            return new double[]
            {
                (116.0 * f[1]) - 16.0,  // L
                500.0 * (f[0] - f[1]),  // a
                200.0 * (f[1] - f[2])   // b
            };
        }

        public static double[] LabToXyz(double[] lab)
        {
            double kappa = 24389.0 / 27.0;
            double epsilon = 216.0 / 24389.0;

            // Compute f, starting with luminance-related term
            double f1 = (lab[0] + 16.0) / 116.0;
            double f0 = lab[1] / 500.0 + f1;
            double f2 = f1 - lab[2] / 200.0;

            // Compute xyz
            double[] xyz = new double[]
            {
                Math.Pow(f0, 3.0) > epsilon ? Math.Pow(f0, 3.0) : (116.0 * f0 - 16.0) / kappa,
                lab[0] > kappa * epsilon ? Math.Pow((lab[0] + 16.0) / 116.0, 3.0) : lab[0] / kappa,
                Math.Pow(f2, 3.0) > epsilon ? Math.Pow(f2, 3.0) : (116.0 * f2 - 16.0) / kappa
            };

            // Scale by reference white
            return new double[]
            {
                xyz[0] * D50[0],
                xyz[1] * D50[1],
                xyz[2] * D50[2]
            };
        }

        public static double[] LabToLch(double[] lab)
        {
            double epsilon = 0.0015;
            double chroma = Math.Sqrt(Math.Pow(lab[1], 2.0) + Math.Pow(lab[2], 2.0));
            double hue = Math.Atan2(lab[2], lab[1]) * 180.0 / Math.PI;
            if (hue < 0)
            {
                hue = hue + 360.0;
            }
            if (chroma <= epsilon)
            {
                hue = double.NaN;
            }
            return new double[] { lab[0], chroma, hue };
        }

        public static double[] LchToLab(double[] lch)
        {
            return new double[]
            {
                lch[0],  // L is still L
                lch[1] * Math.Cos(lch[2] * Math.PI / 180.0),  // a
                lch[1] * Math.Sin(lch[2] * Math.PI / 180.0)   // b
            };
        }

        // OKLab and OKLCH

        public static double[] XyzToOklab(double[] xyz)
        {
            double[][] xyzToLms = new double[][]
            {
                new double[] { 0.8190224379967030, 0.3619062600528904, -0.1288737815209879 },
                new double[] { 0.0329836539323885, 0.9292868615863434,  0.0361446663506424 },
                new double[] { 0.0481771893596242, 0.2642395317527308,  0.6335478284694309 }
            };
            double[][] lmsToOklab = new double[][]
            {
                new double[] { 0.2104542683093140,  0.7936177747023054, -0.0040720430116193 },
                new double[] { 1.9779985324311684, -2.4285922420485799,  0.4505937096174110 },
                new double[] { 0.0259040424655478,  0.7827717124575296, -0.8086757549230774 }
            };

            double[] lms = MultiplyMatrices(xyzToLms, xyz);
            // TODO Deal with negative values for `c` here? Dotnet Math.Pow gives NaN in such cases.
            double[] lmsCbrt = lms.Select(c => Math.Pow(c, 1.0 / 3.0)).ToArray();
            return MultiplyMatrices(lmsToOklab, lmsCbrt);
        }

        public static double[] OklabToXyz(double[] oklab)
        {
            double[][] lmsToXyz = new double[][]
            {
                new double[] {  1.2268798758459243, -0.5578149944602171,  0.2813910456659647 },
                new double[] { -0.0405757452148008,  1.1122868032803170, -0.0717110580655164 },
                new double[] { -0.0763729366746601, -0.4214933324022432,  1.5869240198367816 }
            };
            double[][] oklabToLms = new double[][]
            {
                new double[] { 1.0000000000000000,  0.3963377773761749,  0.2158037573099136 },
                new double[] { 1.0000000000000000, -0.1055613458156586, -0.0638541728258133 },
                new double[] { 1.0000000000000000, -0.0894841775298119, -1.2914855480194092 }
            };

            double[] lmsNonlinear = MultiplyMatrices(oklabToLms, oklab);
            double[] lms = lmsNonlinear.Select(c => Math.Pow(c, 3.0)).ToArray();
            return MultiplyMatrices(lmsToXyz, lms);
        }

        public static double[] OklabToOklch(double[] oklab)
        {
            double epsilon = 0.000004;
            double hue = Math.Atan2(oklab[2], oklab[1]) * 180.0 / Math.PI;
            double chroma = Math.Sqrt(Math.Pow(oklab[1], 2.0) + Math.Pow(oklab[2], 2.0));
            if (hue < 0)
            {
                hue = hue + 360.0;
            }
            if (chroma <= epsilon)
            {
                hue = double.NaN;
            }
            return new double[] { oklab[0], chroma, hue };
        }

        public static double[] OklchToOklab(double[] oklch)
        {
            return new double[]
            {
                oklch[0],  // L is still L
                oklch[1] * Math.Cos(oklch[2] * Math.PI / 180.0),  // a
                oklch[1] * Math.Sin(oklch[2] * Math.PI / 180.0)   // b
            };
        }

        // Premultiplied alpha conversions

        public static double[] RectangularPremultiply(double[] color, double alpha)
        {
            return color.Select(c => c * alpha).ToArray();
        }

        public static double[] RectangularUnPremultiply(double[] color, double alpha)
        {
            if (alpha == 0.0)
            {
                return color;  // Avoid divide by zero
            }
            return color.Select(c => c / alpha).ToArray();
        }

        public static double[] PolarPremultiply(double[] color, double alpha, int hueIndex)
        {
            return color.Select((c, i) => c * (i == hueIndex ? 1.0 : alpha)).ToArray();
        }

        public static double[] PolarUnPremultiply(double[] color, double alpha, int hueIndex)
        {
            if (alpha == 0.0)
            {
                return color;
            }
            return color.Select((c, i) => c / (i == hueIndex ? 1.0 : alpha)).ToArray();
        }

        public static double[] HslPremultiply(double[] color, double alpha)
        {
            return PolarPremultiply(color, alpha, 0);
        }

        // Matrix multiplication utility

        /// <summary>
        /// Simple matrix (and vector) multiplication.
        /// A is m x n. B is n x p. Product is m x p.
        /// </summary>
        public static double[] MultiplyMatrices(double[][] a, double[] b)
        {
            int m = a.Length;
            double[] product = new double[m];

            for (int i = 0; i < m; i++)
            {
                double sum = 0;
                for (int j = 0; j < b.Length; j++)
                {
                    sum += a[i][j] * b[j];
                }
                product[i] = sum;
            }

            return product;
        }
    }
}
