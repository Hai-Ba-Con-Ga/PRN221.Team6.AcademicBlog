const { fontFamily } = require('tailwindcss/defaultTheme')
const colors = require('tailwindcss/colors')
module.exports = {
    purge: {
        enabled: true,
        content: ["./Pages/**/*.cshtml", "./Views/**/*.cshtml"],
    },
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {
            lineHeight: {
                11: '2.75rem',
                12: '3rem',
                13: '3.25rem',
                14: '3.5rem',
            },
            fontFamily: {
                sans: ['var(--font-space-grotesk)', ...fontFamily.sans],
            },
            colors: {
                primary: colors.pink,
                gray: colors.gray,
            },
            typography: ({ theme }) => ({
                DEFAULT: {
                    css: {
                        a: {
                            color: theme('colors.primary.500'),
                            '&:hover': {
                                color: `${theme('colors.primary.600')}`,
                            },
                            code: { color: theme('colors.primary.400') },
                        },
                        'h1,h2': {
                            fontWeight: '700',
                            letterSpacing: theme('letterSpacing.tight'),
                        },
                        h3: {
                            fontWeight: '600',
                        },
                        code: {
                            color: theme('colors.indigo.500'),
                        },
                    },
                },
                invert: {
                    css: {
                        a: {
                            color: theme('colors.primary.500'),
                            '&:hover': {
                                color: `${theme('colors.primary.400')}`,
                            },
                            code: { color: theme('colors.primary.400') },
                        },
                        'h1,h2,h3,h4,h5,h6': {
                            color: theme('colors.gray.100'),
                        },
                    },
                },
            }),
        },
    },
    plugins: [require('@tailwindcss/forms'), require('@tailwindcss/typography')],
    variants: {
        extend: {},
    },
};
