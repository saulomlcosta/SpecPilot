import type { Config } from 'tailwindcss';

export default {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        brand: {
          50: '#f4f7f2',
          100: '#e7eee0',
          200: '#ccdabc',
          300: '#adc28f',
          400: '#8cab69',
          500: '#6b8f4f',
          600: '#536f3d',
          700: '#405631',
          800: '#344529',
          900: '#2c3924'
        },
        accent: {
          50: '#fff6ed',
          100: '#ffead5',
          200: '#fed3aa',
          300: '#fdb774',
          400: '#fb9140',
          500: '#f97316',
          600: '#ea580c'
        }
      },
      boxShadow: {
        soft: '0 18px 45px rgba(31, 41, 55, 0.08)'
      },
      fontFamily: {
        sans: ['Space Grotesk', 'Segoe UI', 'sans-serif'],
        serif: ['"Source Serif 4"', 'Georgia', 'serif']
      }
    }
  },
  plugins: []
} satisfies Config;
