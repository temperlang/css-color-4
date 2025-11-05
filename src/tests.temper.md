    // Unit tests for CSS Color Module Level 4 conversion functions in Temper

    let epsilon: Float64 = 0.0001; // Tolerance for floating point comparisons

    @extension("assertArraysClose")
    let assertArraysClose(test: Test, arr1: List<Float64>, arr2: List<Float64>, message: String): Void {
      assert(
        arr1.length == arr2.length && do {
          var result = true;
          for (var i = 0; i < arr1.length; i += 1) {
            if (!arr1[i].near(arr2[i], null, epsilon)) {
              result = false;
              break;
            }
          }
          result
        }
      ) {
        "${message}: ${vecToString(arr1)} ${vecToString(arr2)}"
      }
    }

    let vecToString(vec: List<Float64>): String {
      "[${vec.join(", ") { x => "${x}" }}]"
    }

    test("sRGB Gamma Conversion Tests") { test =>
      test.assertArraysClose(
        linSrgb([0.0, 0.0, 0.0]),
        [0.0, 0.0, 0.0],
        "linSrgb: Black remains black",
      );

      test.assertArraysClose(
        linSrgb([1.0, 1.0, 1.0]),
        [1.0, 1.0, 1.0],
        "linSrgb: White remains white",
      );

      test.assertArraysClose(
        linSrgb([0.5, 0.5, 0.5]),
        [0.2140, 0.2140, 0.2140],
        "linSrgb: Mid-gray conversion",
      );

      // Test roundtrip sRGB
      let srgbTest = [0.25, 0.5, 0.75];
      test.assertArraysClose(
        gamSrgb(linSrgb(srgbTest)),
        srgbTest,
        "sRGB roundtrip",
      );

      // Test negative values
      test.assertArraysClose(
        linSrgb([-0.5, 0.0, 0.5]),
        [-0.2140, 0.0, 0.2140],
        "linSrgb: Negative values",
      );
    }

    test("sRGB <-> XYZ Conversion Tests") { test =>
      let whiteSrgb = [1.0, 1.0, 1.0];
      let whiteXyz = linSrgbToXyz(linSrgb(whiteSrgb));
      test.assertArraysClose(
        whiteXyz,
        [0.9505, 1.0000, 1.0890],
        "sRGB white to XYZ (D65)",
      );

      // Test XYZ roundtrip
      let rgbTest = [0.5, 0.3, 0.8];
      test.assertArraysClose(
        xyzToLinSrgb(linSrgbToXyz(linSrgb(rgbTest))),
        linSrgb(rgbTest),
        "sRGB -> XYZ -> sRGB roundtrip",
      );
    }

    test("Display P3 Tests") { test =>
      test.assertArraysClose(
        linP3([0.5, 0.5, 0.5]),
        [0.2140, 0.2140, 0.2140],
        "linP3: Uses same gamma as sRGB",
      );

      let p3Test = [0.6, 0.4, 0.7];
      test.assertArraysClose(
        xyzToLinP3(linP3ToXyz(linP3(p3Test))),
        linP3(p3Test),
        "P3 -> XYZ -> P3 roundtrip",
      );
    }

    test("ProPhoto RGB Tests") { test =>
      test.assertArraysClose(
        linProPhoto([0.0, 0.0, 0.0]),
        [0.0, 0.0, 0.0],
        "linProPhoto: Black",
      );

      let prophotoTest = [0.5, 0.6, 0.7];
      test.assertArraysClose(
        gamProPhoto(linProPhoto(prophotoTest)),
        prophotoTest,
        "ProPhoto roundtrip",
      );

      test.assertArraysClose(
        xyzToLinProPhoto(linProPhotoToXyz(linProPhoto(prophotoTest))),
        linProPhoto(prophotoTest),
        "ProPhoto -> XYZ -> ProPhoto roundtrip",
      );
    }

    test("Adobe RGB Tests") { test =>
      let a98Test = [0.4, 0.5, 0.6];
      test.assertArraysClose(
        gamA98rgb(linA98rgb(a98Test)),
        a98Test,
        "Adobe RGB roundtrip",
      );

      test.assertArraysClose(
        xyzToLinA98rgb(linA98rgbToXyz(linA98rgb(a98Test))),
        linA98rgb(a98Test),
        "Adobe RGB -> XYZ -> Adobe RGB roundtrip",
      );
    }

    test("Rec. 2020 Tests") { test =>
      let rec2020Test = [0.3, 0.5, 0.7];
      test.assertArraysClose(
        gam2020(lin2020(rec2020Test)),
        rec2020Test,
        "Rec. 2020 roundtrip",
      );

      test.assertArraysClose(
        xyzToLin2020(lin2020ToXyz(lin2020(rec2020Test))),
        lin2020(rec2020Test),
        "Rec. 2020 -> XYZ -> Rec. 2020 roundtrip",
      );
    }

    test("Chromatic Adaptation Tests") { test =>
      let xyzD65 = [0.5, 0.6, 0.7];
      test.assertArraysClose(
        d50ToD65(d65ToD50(xyzD65)),
        xyzD65,
        "D65 -> D50 -> D65 roundtrip",
      );
    }

    test("CIE Lab Tests") { test =>
      let xyzD50 = [0.5, 0.6, 0.7];
      let lab = xyzToLab(xyzD50);
      assert(lab[0] >= 0.0 && lab[0] <= 100.0) {
        "Lab L* in range [0,100]: ${lab[0]}"
      }

      test.assertArraysClose(
        labToXyz(xyzToLab(xyzD50)),
        xyzD50,
        "XYZ -> Lab -> XYZ roundtrip",
      );
    }

    test("Lab <-> LCH Tests") { test =>
      let labTest = [50.0, 25.0, -50.0];
      let lch = labToLch(labTest);
      assert(lch[0] == 50.0) {
        "LCH: L preserved (${lch[0]})"
      }
      assert(lch[1] >= 0.0) {
        "LCH: Chroma non-negative (${lch[1]})"
      }
      assert(lch[2] >= 0.0 && lch[2] < 360.0) {
        "LCH: Hue in [0, 360) (${lch[2]})"
      }

      test.assertArraysClose(
        lchToLab(labToLch(labTest)),
        labTest,
        "Lab -> LCH -> Lab roundtrip",
      );
    }

    test("OKLab Tests") { test =>
      let xyzTest = [0.4, 0.5, 0.6];
      let oklab = xyzToOklab(xyzTest);
      assert(oklab[0] >= 0.0 && oklab[0] <= 1.0) {
        "OKLab L in range [0,1]: ${oklab[0]}"
      }

      test.assertArraysClose(
        oklabToXyz(xyzToOklab(xyzTest)),
        xyzTest,
        "XYZ -> OKLab -> XYZ roundtrip",
      );
    }

    test("OKLab <-> OKLCH Tests") { test =>
      let oklabTest = [0.5, 0.1, -0.05];
      let oklch = oklabToOklch(oklabTest);
      assert(oklch[0] == 0.5) {
        "OKLCH: L preserved (${oklch[0]})"
      }
      assert(oklch[1] >= 0.0) {
        "OKLCH: Chroma non-negative (${oklch[1]})"
      }

      test.assertArraysClose(
        oklchToOklab(oklabToOklch(oklabTest)),
        oklabTest,
        "OKLab -> OKLCH -> OKLab roundtrip",
      );
    }

    test("Premultiplication Tests") { test =>
      let colorRect = [0.5, 0.3, 0.8];
      let alpha = 0.7;

      let premul = rectangularPremultiply(colorRect, alpha);
      test.assertArraysClose(
        premul,
        [0.35, 0.21, 0.56],
        "Rectangular premultiply",
      );

      test.assertArraysClose(
        rectangularUnPremultiply(premul, alpha),
        colorRect,
        "Rectangular un-premultiply",
      );

      // Test zero alpha edge case
      test.assertArraysClose(
        rectangularUnPremultiply([0.0, 0.0, 0.0], 0.0),
        [0.0, 0.0, 0.0],
        "Rectangular un-premultiply with alpha=0",
      );

      // Test polar premultiplication
      let colorPolar = [50.0, 30.0, 180.0]; // L, C, H
      let premulPolar = polarPremultiply(colorPolar, alpha, 2);
      test.assertArraysClose(
        premulPolar,
        [35.0, 21.0, 180.0],
        "Polar premultiply (hue preserved)",
      );

      test.assertArraysClose(
        polarUnPremultiply(premulPolar, alpha, 2),
        colorPolar,
        "Polar un-premultiply",
      );

      // Test HSL convenience function
      test.assertArraysClose(
        hslPremultiply([180.0, 50.0, 30.0], alpha),
        [180.0, 35.0, 21.0],
        "HSL premultiply convenience function",
      );
    }

    test("Matrix Multiplication Tests") { test =>
      const matrix2x2 = [[1.0, 2.0], [3.0, 4.0]];
      const vector2 = [5.0, 6.0];
      test.assertArraysClose(
        multiplyMatrixVector(matrix2x2, vector2),
        [17.0, 39.0],
        'Matrix * vector multiplication'
      );

      const matrix2x3 = [[1.0, 2.0, 3.0], [4.0, 5.0, 6.0]];
      const vector3 = [1.0, 2.0, 3.0];
      test.assertArraysClose(
        multiplyMatrixVector(matrix2x3, vector3),
        [14.0, 32.0],
        'Matrix * vector (2x3 * 3x1)'
      );

      // Test identity matrix
      const identity = [[1.0, 0.0, 0.0], [0.0, 1.0, 0.0], [0.0, 0.0, 1.0]];
      test.assertArraysClose(
        multiplyMatrixVector(identity, vector3),
        vector3,
        'Identity matrix multiplication'
      );
    }

    test("Edge Cases") { test =>
      // Very small values
      test.assertArraysClose(
        gamSrgb(linSrgb([0.001, 0.001, 0.001])),
        [0.001, 0.001, 0.001],
        "sRGB: Very small values roundtrip",
      );

      // Near-white values
      test.assertArraysClose(
        gamSrgb(linSrgb([0.999, 0.999, 0.999])),
        [0.999, 0.999, 0.999],
        "sRGB: Near-white values roundtrip",
      );
    }
