const hexToRgbUtilsCharts = (hexValue) => {
    let hex;
    hexValue.indexOf("#") === 0 ? (hex = hexValue.substring(1)) : (hex = hexValue);

    const shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(
        hex.replace(shorthandRegex, (m, r, g, b) => r + r + g + g + b + b)
    );
    return result
        ? [parseInt(result[1], 16), parseInt(result[2], 16), parseInt(result[3], 16)]
        : null;
};

const rgbaColorUtilsCharts = (color = "#fff", alpha = 0.5) => `rgba(${hexToRgbUtilsCharts(color)}, ${alpha})`;

const getColorsUtilsCharts = (dom) => ({
    primary: getColorUtilsCharts("primary", dom),
    secondary: getColorUtilsCharts("secondary", dom),
    success: getColorUtilsCharts("success", dom),
    info: getColorUtilsCharts("info", dom),
    warning: getColorUtilsCharts("warning", dom),
    danger: getColorUtilsCharts("danger", dom),
    light: getColorUtilsCharts("light", dom),
    dark: getColorUtilsCharts("dark", dom),
});

const getGraysUtilsCharts = (t) => ({
    white: getColorUtilsCharts("white", t),
    100: getColorUtilsCharts("100", t),
    200: getColorUtilsCharts("200", t),
    300: getColorUtilsCharts("300", t),
    400: getColorUtilsCharts("400", t),
    500: getColorUtilsCharts("500", t),
    600: getColorUtilsCharts("600", t),
    700: getColorUtilsCharts("700", t),
    800: getColorUtilsCharts("800", t),
    900: getColorUtilsCharts("900", t),
    1000: getColorUtilsCharts("1000", t),
    1100: getColorUtilsCharts("1100", t),
    black: getColorUtilsCharts("black", t),
});

const getColorUtilsCharts = (name, dom = document.documentElement) =>
    getComputedStyle(dom).getPropertyValue(`--falcon-${name}`).trim();

const utilsCharts = {
    getColorUtilsCharts,
    rgbaColorUtilsCharts,
    getColorsUtilsCharts,
    getGraysUtilsCharts,
};
