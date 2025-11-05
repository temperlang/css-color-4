// Unit tests for CSS Color Module Level 4 conversion functions
// Using a simple test framework pattern

import {
  // Constants
  D50, D65,
  
  // sRGB functions
  lin_sRGB, gam_sRGB, lin_sRGB_to_XYZ, XYZ_to_lin_sRGB,
  
  // Display P3 functions
  lin_P3, gam_P3, lin_P3_to_XYZ, XYZ_to_lin_P3,
  
  // ProPhoto RGB functions
  lin_ProPhoto, gam_ProPhoto, lin_ProPhoto_to_XYZ, XYZ_to_lin_ProPhoto,
  
  // Adobe RGB (a98-rgb) functions
  lin_a98rgb, gam_a98rgb, lin_a98rgb_to_XYZ, XYZ_to_lin_a98rgb,
  
  // Rec. 2020 functions
  lin_2020, gam_2020, lin_2020_to_XYZ, XYZ_to_lin_2020,
  
  // Chromatic adaptation
  D65_to_D50, D50_to_D65,
  
  // CIE Lab and LCH
  XYZ_to_Lab, Lab_to_XYZ, Lab_to_LCH, LCH_to_Lab,
  
  // OKLab and OKLCH
  XYZ_to_OKLab, OKLab_to_XYZ, OKLab_to_OKLCH, OKLCH_to_OKLab,
  
  // Premultiplication functions
  rectangular_premultiply, rectangular_un_premultiply,
  polar_premultiply, polar_un_premultiply, hsl_premultiply,
  
  // Matrix utilities
  multiplyMatrices
} from './conversion.js';

const EPSILON = 0.0001; // Tolerance for floating point comparisons

class TestRunner {
  constructor() {
    this.passed = 0;
    this.failed = 0;
    this.tests = [];
  }

  assert(condition, message) {
    if (condition) {
      this.passed++;
      console.log(`✓ ${message}`);
    } else {
      this.failed++;
      console.error(`✗ ${message}`);
      this.tests.push({ passed: false, message });
    }
  }

  assertArraysClose(arr1, arr2, message) {
    const close = arr1.length === arr2.length && 
                  arr1.every((val, i) => Math.abs(val - arr2[i]) < EPSILON);
    this.assert(close, message + ` [${arr1.map(v => v.toFixed(4))}] ≈ [${arr2.map(v => v.toFixed(4))}]`);
  }

  summary() {
    console.log(`\n${'='.repeat(50)}`);
    console.log(`Tests: ${this.passed + this.failed} | Passed: ${this.passed} | Failed: ${this.failed}`);
    console.log(`${'='.repeat(50)}`);
  }
}

const test = new TestRunner();

// Test sRGB gamma functions
console.log('\n=== sRGB Gamma Conversion Tests ===');

// Test lin_sRGB with known values
test.assertArraysClose(
  lin_sRGB([0, 0, 0]),
  [0, 0, 0],
  'lin_sRGB: Black remains black'
);

test.assertArraysClose(
  lin_sRGB([1, 1, 1]),
  [1, 1, 1],
  'lin_sRGB: White remains white'
);

test.assertArraysClose(
  lin_sRGB([0.5, 0.5, 0.5]),
  [0.2140, 0.2140, 0.2140],
  'lin_sRGB: Mid-gray conversion'
);

// Test roundtrip sRGB
const srgbTest = [0.25, 0.5, 0.75];
test.assertArraysClose(
  gam_sRGB(lin_sRGB(srgbTest)),
  srgbTest,
  'sRGB roundtrip'
);

// Test negative values (extended transfer function)
test.assertArraysClose(
  lin_sRGB([-0.5, 0, 0.5]),
  [-0.2140, 0, 0.2140],
  'lin_sRGB: Negative values'
);

// Test XYZ conversions
console.log('\n=== sRGB <-> XYZ Conversion Tests ===');

const white_sRGB = [1, 1, 1];
const white_XYZ = lin_sRGB_to_XYZ(lin_sRGB(white_sRGB));
test.assertArraysClose(
  white_XYZ,
  [0.9505, 1.0000, 1.0890],
  'sRGB white to XYZ (D65)'
);

// Test XYZ roundtrip
const rgb_test = [0.5, 0.3, 0.8];
test.assertArraysClose(
  XYZ_to_lin_sRGB(lin_sRGB_to_XYZ(lin_sRGB(rgb_test))),
  lin_sRGB(rgb_test),
  'sRGB -> XYZ -> sRGB roundtrip'
);

// Test Display P3
console.log('\n=== Display P3 Tests ===');

test.assertArraysClose(
  lin_P3([0.5, 0.5, 0.5]),
  [0.2140, 0.2140, 0.2140],
  'lin_P3: Uses same gamma as sRGB'
);

const p3_test = [0.6, 0.4, 0.7];
test.assertArraysClose(
  XYZ_to_lin_P3(lin_P3_to_XYZ(lin_P3(p3_test))),
  lin_P3(p3_test),
  'P3 -> XYZ -> P3 roundtrip'
);

// Test ProPhoto RGB
console.log('\n=== ProPhoto RGB Tests ===');

test.assertArraysClose(
  lin_ProPhoto([0, 0, 0]),
  [0, 0, 0],
  'lin_ProPhoto: Black'
);

const prophoto_test = [0.5, 0.6, 0.7];
test.assertArraysClose(
  gam_ProPhoto(lin_ProPhoto(prophoto_test)),
  prophoto_test,
  'ProPhoto roundtrip'
);

test.assertArraysClose(
  XYZ_to_lin_ProPhoto(lin_ProPhoto_to_XYZ(lin_ProPhoto(prophoto_test))),
  lin_ProPhoto(prophoto_test),
  'ProPhoto -> XYZ -> ProPhoto roundtrip'
);

// Test Adobe RGB (a98-rgb)
console.log('\n=== Adobe RGB Tests ===');

const a98_test = [0.4, 0.5, 0.6];
test.assertArraysClose(
  gam_a98rgb(lin_a98rgb(a98_test)),
  a98_test,
  'Adobe RGB roundtrip'
);

test.assertArraysClose(
  XYZ_to_lin_a98rgb(lin_a98rgb_to_XYZ(lin_a98rgb(a98_test))),
  lin_a98rgb(a98_test),
  'Adobe RGB -> XYZ -> Adobe RGB roundtrip'
);

// Test Rec. 2020
console.log('\n=== Rec. 2020 Tests ===');

const rec2020_test = [0.3, 0.5, 0.7];
test.assertArraysClose(
  gam_2020(lin_2020(rec2020_test)),
  rec2020_test,
  'Rec. 2020 roundtrip'
);

test.assertArraysClose(
  XYZ_to_lin_2020(lin_2020_to_XYZ(lin_2020(rec2020_test))),
  lin_2020(rec2020_test),
  'Rec. 2020 -> XYZ -> Rec. 2020 roundtrip'
);

// Test chromatic adaptation
console.log('\n=== Chromatic Adaptation Tests ===');

const xyz_d65 = [0.5, 0.6, 0.7];
test.assertArraysClose(
  D50_to_D65(D65_to_D50(xyz_d65)),
  xyz_d65,
  'D65 -> D50 -> D65 roundtrip',
  0.001
);

// Test Lab conversion
console.log('\n=== CIE Lab Tests ===');

const xyz_d50 = [0.5, 0.6, 0.7];
const lab = XYZ_to_Lab(xyz_d50);
test.assert(
  lab[0] >= 0 && lab[0] <= 100,
  `Lab L* in range [0,100]: ${lab[0].toFixed(2)}`
);

test.assertArraysClose(
  Lab_to_XYZ(XYZ_to_Lab(xyz_d50)),
  xyz_d50,
  'XYZ -> Lab -> XYZ roundtrip'
);

// Test Lab <-> LCH
console.log('\n=== Lab <-> LCH Tests ===');

const lab_test = [50, 25, -50];
const lch = Lab_to_LCH(lab_test);
test.assert(
  lch[0] === 50,
  `LCH: L preserved (${lch[0]})`
);
test.assert(
  lch[1] >= 0,
  `LCH: Chroma non-negative (${lch[1].toFixed(2)})`
);
test.assert(
  lch[2] >= 0 && lch[2] < 360,
  `LCH: Hue in [0, 360) (${lch[2].toFixed(2)})`
);

test.assertArraysClose(
  LCH_to_Lab(Lab_to_LCH(lab_test)),
  lab_test,
  'Lab -> LCH -> Lab roundtrip'
);

// Test achromatic colors (zero chroma)
const gray_lab = [50, 0, 0];
const gray_lch = Lab_to_LCH(gray_lab);
test.assert(
  isNaN(gray_lch[2]),
  'LCH: Achromatic color has NaN hue'
);

// Test OKLab
console.log('\n=== OKLab Tests ===');

const xyz_test = [0.4, 0.5, 0.6];
const oklab = XYZ_to_OKLab(xyz_test);
test.assert(
  oklab[0] >= 0 && oklab[0] <= 1,
  `OKLab L in range [0,1]: ${oklab[0].toFixed(4)}`
);

test.assertArraysClose(
  OKLab_to_XYZ(XYZ_to_OKLab(xyz_test)),
  xyz_test,
  'XYZ -> OKLab -> XYZ roundtrip'
);

// Test OKLab <-> OKLCH
console.log('\n=== OKLab <-> OKLCH Tests ===');

const oklab_test = [0.5, 0.1, -0.05];
const oklch = OKLab_to_OKLCH(oklab_test);
test.assert(
  oklch[0] === 0.5,
  `OKLCH: L preserved (${oklch[0]})`
);
test.assert(
  oklch[1] >= 0,
  `OKLCH: Chroma non-negative (${oklch[1].toFixed(4)})`
);

test.assertArraysClose(
  OKLCH_to_OKLab(OKLab_to_OKLCH(oklab_test)),
  oklab_test,
  'OKLab -> OKLCH -> OKLab roundtrip'
);

// Test premultiplication
console.log('\n=== Premultiplication Tests ===');

const color_rect = [0.5, 0.3, 0.8];
const alpha = 0.7;

const premul = rectangular_premultiply(color_rect, alpha);
test.assertArraysClose(
  premul,
  [0.35, 0.21, 0.56],
  'Rectangular premultiply'
);

test.assertArraysClose(
  rectangular_un_premultiply(premul, alpha),
  color_rect,
  'Rectangular un-premultiply'
);

// Test zero alpha edge case
test.assertArraysClose(
  rectangular_un_premultiply([0, 0, 0], 0),
  [0, 0, 0],
  'Rectangular un-premultiply with alpha=0'
);

// Test polar premultiplication
const color_polar = [50, 30, 180]; // L, C, H
const premul_polar = polar_premultiply(color_polar, alpha, 2);
test.assertArraysClose(
  premul_polar,
  [35, 21, 180],
  'Polar premultiply (hue preserved)'
);

test.assertArraysClose(
  polar_un_premultiply(premul_polar, alpha, 2),
  color_polar,
  'Polar un-premultiply'
);

// Test HSL convenience function
test.assertArraysClose(
  hsl_premultiply([180, 50, 30], alpha),
  [180, 35, 21],
  'HSL premultiply convenience function'
);

// Test matrix multiplication
console.log('\n=== Matrix Multiplication Tests ===');

const matrix2x2 = [[1, 2], [3, 4]];
const vector2 = [5, 6];
test.assertArraysClose(
  multiplyMatrices(matrix2x2, vector2),
  [17, 39],
  'Matrix * vector multiplication'
);

const matrix2x3 = [[1, 2, 3], [4, 5, 6]];
const vector3 = [1, 2, 3];
test.assertArraysClose(
  multiplyMatrices(matrix2x3, vector3),
  [14, 32],
  'Matrix * vector (2x3 * 3x1)'
);

// Test identity matrix
const identity = [[1, 0, 0], [0, 1, 0], [0, 0, 1]];
test.assertArraysClose(
  multiplyMatrices(identity, vector3),
  vector3,
  'Identity matrix multiplication'
);

// Test edge cases
console.log('\n=== Edge Cases ===');

// Very small values
test.assertArraysClose(
  gam_sRGB(lin_sRGB([0.001, 0.001, 0.001])),
  [0.001, 0.001, 0.001],
  'sRGB: Very small values roundtrip'
);

// Near-white values
test.assertArraysClose(
  gam_sRGB(lin_sRGB([0.999, 0.999, 0.999])),
  [0.999, 0.999, 0.999],
  'sRGB: Near-white values roundtrip',
);

// Summary
test.summary();
