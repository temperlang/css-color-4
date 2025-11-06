// Unit tests for CSS Color Module Level 4 conversion functions in C#
// Using xUnit testing framework

using System;
using System.Linq;
using Xunit;
using CssColor4;

namespace CssColor4.Tests
{
    public class ConversionTests
    {
        private const double Epsilon = 0.0001; // Tolerance for floating point comparisons

        private void AssertArraysClose(double[] arr1, double[] arr2, double epsilon = Epsilon)
        {
            Assert.Equal(arr1.Length, arr2.Length);
            for (int i = 0; i < arr1.Length; i++)
            {
                Assert.True(Math.Abs(arr1[i] - arr2[i]) < epsilon,
                    $"Arrays differ at index {i}: {arr1[i]} vs {arr2[i]}");
            }
        }

        [Fact]
        public void LinSrgb_Black_RemainsBlack()
        {
            var result = Conversion.LinSrgb(new double[] { 0, 0, 0 });
            AssertArraysClose(result, new double[] { 0, 0, 0 });
        }

        [Fact]
        public void LinSrgb_White_RemainsWhite()
        {
            var result = Conversion.LinSrgb(new double[] { 1, 1, 1 });
            AssertArraysClose(result, new double[] { 1, 1, 1 });
        }

        [Fact]
        public void LinSrgb_MidGray_ConvertsCorrectly()
        {
            var result = Conversion.LinSrgb(new double[] { 0.5, 0.5, 0.5 });
            AssertArraysClose(result, new double[] { 0.2140, 0.2140, 0.2140 });
        }

        [Fact]
        public void Srgb_Roundtrip_PreservesValues()
        {
            var srgbTest = new double[] { 0.25, 0.5, 0.75 };
            var result = Conversion.GamSrgb(Conversion.LinSrgb(srgbTest));
            AssertArraysClose(result, srgbTest);
        }

        [Fact]
        public void LinSrgb_NegativeValues_HandledCorrectly()
        {
            var result = Conversion.LinSrgb(new double[] { -0.5, 0, 0.5 });
            AssertArraysClose(result, new double[] { -0.2140, 0, 0.2140 });
        }

        [Fact]
        public void SrgbWhite_ToXyz_CorrectD65()
        {
            var whiteSrgb = new double[] { 1, 1, 1 };
            var whiteXyz = Conversion.LinSrgbToXyz(Conversion.LinSrgb(whiteSrgb));
            AssertArraysClose(whiteXyz, new double[] { 0.9505, 1.0000, 1.0890 });
        }

        [Fact]
        public void SrgbToXyzToSrgb_Roundtrip_PreservesValues()
        {
            var rgbTest = new double[] { 0.5, 0.3, 0.8 };
            var linear = Conversion.LinSrgb(rgbTest);
            var xyz = Conversion.LinSrgbToXyz(linear);
            var result = Conversion.XyzToLinSrgb(xyz);
            AssertArraysClose(result, linear);
        }

        [Fact]
        public void LinP3_UsesSameGammaAsSrgb()
        {
            var result = Conversion.LinP3(new double[] { 0.5, 0.5, 0.5 });
            AssertArraysClose(result, new double[] { 0.2140, 0.2140, 0.2140 });
        }

        [Fact]
        public void P3ToXyzToP3_Roundtrip_PreservesValues()
        {
            var p3Test = new double[] { 0.6, 0.4, 0.7 };
            var linear = Conversion.LinP3(p3Test);
            var xyz = Conversion.LinP3ToXyz(linear);
            var result = Conversion.XyzToLinP3(xyz);
            AssertArraysClose(result, linear);
        }

        [Fact]
        public void LinProPhoto_Black_RemainsBlack()
        {
            var result = Conversion.LinProPhoto(new double[] { 0, 0, 0 });
            AssertArraysClose(result, new double[] { 0, 0, 0 });
        }

        [Fact]
        public void ProPhoto_Roundtrip_PreservesValues()
        {
            var prophotoTest = new double[] { 0.5, 0.6, 0.7 };
            var result = Conversion.GamProPhoto(Conversion.LinProPhoto(prophotoTest));
            AssertArraysClose(result, prophotoTest);
        }

        [Fact]
        public void ProPhotoToXyzToProPhoto_Roundtrip_PreservesValues()
        {
            var prophotoTest = new double[] { 0.5, 0.6, 0.7 };
            var linear = Conversion.LinProPhoto(prophotoTest);
            var xyz = Conversion.LinProPhotoToXyz(linear);
            var result = Conversion.XyzToLinProPhoto(xyz);
            AssertArraysClose(result, linear);
        }

        [Fact]
        public void AdobeRgb_Roundtrip_PreservesValues()
        {
            var a98Test = new double[] { 0.4, 0.5, 0.6 };
            var result = Conversion.GamA98rgb(Conversion.LinA98rgb(a98Test));
            AssertArraysClose(result, a98Test);
        }

        [Fact]
        public void AdobeRgbToXyzToAdobeRgb_Roundtrip_PreservesValues()
        {
            var a98Test = new double[] { 0.4, 0.5, 0.6 };
            var linear = Conversion.LinA98rgb(a98Test);
            var xyz = Conversion.LinA98rgbToXyz(linear);
            var result = Conversion.XyzToLinA98rgb(xyz);
            AssertArraysClose(result, linear);
        }

        [Fact]
        public void Rec2020_Roundtrip_PreservesValues()
        {
            var rec2020Test = new double[] { 0.3, 0.5, 0.7 };
            var result = Conversion.Gam2020(Conversion.Lin2020(rec2020Test));
            AssertArraysClose(result, rec2020Test);
        }

        [Fact]
        public void Rec2020ToXyzToRec2020_Roundtrip_PreservesValues()
        {
            var rec2020Test = new double[] { 0.3, 0.5, 0.7 };
            var linear = Conversion.Lin2020(rec2020Test);
            var xyz = Conversion.Lin2020ToXyz(linear);
            var result = Conversion.XyzToLin2020(xyz);
            AssertArraysClose(result, linear);
        }

        [Fact]
        public void D65ToD50ToD65_Roundtrip_PreservesValues()
        {
            var xyzD65 = new double[] { 0.5, 0.6, 0.7 };
            var result = Conversion.D50ToD65(Conversion.D65ToD50(xyzD65));
            AssertArraysClose(result, xyzD65, 0.001);
        }

        [Fact]
        public void XyzToLab_LInRange()
        {
            var xyzD50 = new double[] { 0.5, 0.6, 0.7 };
            var lab = Conversion.XyzToLab(xyzD50);
            Assert.True(lab[0] >= 0 && lab[0] <= 100, $"Lab L* should be in [0,100], got {lab[0]}");
        }

        [Fact]
        public void XyzToLabToXyz_Roundtrip_PreservesValues()
        {
            var xyzD50 = new double[] { 0.5, 0.6, 0.7 };
            var result = Conversion.LabToXyz(Conversion.XyzToLab(xyzD50));
            AssertArraysClose(result, xyzD50);
        }

        [Fact]
        public void LabToLch_LPreserved()
        {
            var labTest = new double[] { 50, 25, -50 };
            var lch = Conversion.LabToLch(labTest);
            Assert.Equal(50, lch[0]);
        }

        [Fact]
        public void LabToLch_ChromaNonNegative()
        {
            var labTest = new double[] { 50, 25, -50 };
            var lch = Conversion.LabToLch(labTest);
            Assert.True(lch[1] >= 0, $"Chroma should be non-negative, got {lch[1]}");
        }

        [Fact]
        public void LabToLch_HueInRange()
        {
            var labTest = new double[] { 50, 25, -50 };
            var lch = Conversion.LabToLch(labTest);
            Assert.True(lch[2] >= 0 && lch[2] < 360, $"Hue should be in [0, 360), got {lch[2]}");
        }

        [Fact]
        public void LabToLchToLab_Roundtrip_PreservesValues()
        {
            var labTest = new double[] { 50, 25, -50 };
            var result = Conversion.LchToLab(Conversion.LabToLch(labTest));
            AssertArraysClose(result, labTest);
        }

        [Fact]
        public void LabToLch_AchromaticColor_HasNaNHue()
        {
            var grayLab = new double[] { 50, 0, 0 };
            var grayLch = Conversion.LabToLch(grayLab);
            Assert.True(double.IsNaN(grayLch[2]), "Achromatic color should have NaN hue");
        }

        [Fact]
        public void XyzToOklab_LInRange()
        {
            var xyzTest = new double[] { 0.4, 0.5, 0.6 };
            var oklab = Conversion.XyzToOklab(xyzTest);
            Assert.True(oklab[0] >= 0 && oklab[0] <= 1, $"OKLab L should be in [0,1], got {oklab[0]}");
        }

        [Fact]
        public void XyzToOklabToXyz_Roundtrip_PreservesValues()
        {
            var xyzTest = new double[] { 0.4, 0.5, 0.6 };
            var result = Conversion.OklabToXyz(Conversion.XyzToOklab(xyzTest));
            AssertArraysClose(result, xyzTest);
        }

        [Fact]
        public void OklabToOklch_LPreserved()
        {
            var oklabTest = new double[] { 0.5, 0.1, -0.05 };
            var oklch = Conversion.OklabToOklch(oklabTest);
            Assert.Equal(0.5, oklch[0]);
        }

        [Fact]
        public void OklabToOklch_ChromaNonNegative()
        {
            var oklabTest = new double[] { 0.5, 0.1, -0.05 };
            var oklch = Conversion.OklabToOklch(oklabTest);
            Assert.True(oklch[1] >= 0, $"Chroma should be non-negative, got {oklch[1]}");
        }

        [Fact]
        public void OklabToOklchToOklab_Roundtrip_PreservesValues()
        {
            var oklabTest = new double[] { 0.5, 0.1, -0.05 };
            var result = Conversion.OklchToOklab(Conversion.OklabToOklch(oklabTest));
            AssertArraysClose(result, oklabTest);
        }

        [Fact]
        public void RectangularPremultiply_CalculatesCorrectly()
        {
            var colorRect = new double[] { 0.5, 0.3, 0.8 };
            double alpha = 0.7;
            var premul = Conversion.RectangularPremultiply(colorRect, alpha);
            AssertArraysClose(premul, new double[] { 0.35, 0.21, 0.56 });
        }

        [Fact]
        public void RectangularUnPremultiply_ReversesOperation()
        {
            var colorRect = new double[] { 0.5, 0.3, 0.8 };
            double alpha = 0.7;
            var premul = Conversion.RectangularPremultiply(colorRect, alpha);
            var result = Conversion.RectangularUnPremultiply(premul, alpha);
            AssertArraysClose(result, colorRect);
        }

        [Fact]
        public void RectangularUnPremultiply_ZeroAlpha_ReturnsInput()
        {
            var result = Conversion.RectangularUnPremultiply(new double[] { 0, 0, 0 }, 0);
            AssertArraysClose(result, new double[] { 0, 0, 0 });
        }

        [Fact]
        public void PolarPremultiply_PreservesHue()
        {
            var colorPolar = new double[] { 50, 30, 180 }; // L, C, H
            double alpha = 0.7;
            var premulPolar = Conversion.PolarPremultiply(colorPolar, alpha, 2);
            AssertArraysClose(premulPolar, new double[] { 35, 21, 180 });
        }

        [Fact]
        public void PolarUnPremultiply_ReversesOperation()
        {
            var colorPolar = new double[] { 50, 30, 180 };
            double alpha = 0.7;
            var premulPolar = Conversion.PolarPremultiply(colorPolar, alpha, 2);
            var result = Conversion.PolarUnPremultiply(premulPolar, alpha, 2);
            AssertArraysClose(result, colorPolar);
        }

        [Fact]
        public void HslPremultiply_PreservesHueAtIndexZero()
        {
            var result = Conversion.HslPremultiply(new double[] { 180, 50, 30 }, 0.7);
            AssertArraysClose(result, new double[] { 180, 35, 21 });
        }

        [Fact]
        public void Srgb_VerySmallValues_Roundtrip()
        {
            var result = Conversion.GamSrgb(Conversion.LinSrgb(new double[] { 0.001, 0.001, 0.001 }));
            AssertArraysClose(result, new double[] { 0.001, 0.001, 0.001 });
        }

        [Fact]
        public void Srgb_NearWhiteValues_Roundtrip()
        {
            var result = Conversion.GamSrgb(Conversion.LinSrgb(new double[] { 0.999, 0.999, 0.999 }));
            AssertArraysClose(result, new double[] { 0.999, 0.999, 0.999 }, 0.001);
        }
    }
}