# Event Ticketing Client

A frontend Vue 3 application for browsing and managing event ticket sales. Built using:

- **Vite** for lightning-fast development
- **Vue 3 + Composition API**
- **PrimeVue** for UI components
- **Tailwind CSS** for utility-first styling
- **Pinia** for state management

---

## Features

- Ticket lookup by Event ID
- Paginated table with sorting
- Responsive UI using PrimeVue
- Tailwind CSS utility-first styling
- Modular component structure

---

## Project Setup

### Prerequisites

This project requires:

- **Node.js** `v18.0.0` or later (recommended: latest LTS version)
- **Yarn** (preferred) or **npm**

#### 🔧 Recommended: Install Node via NVM

To avoid issues with global Node installations and to easily switch between versions, we recommend using **Node Version Manager (NVM)**:

- **macOS/Linux:**
  - Follow the instructions on the [official NVM GitHub page](https://github.com/nvm-sh/nvm)
  - After installing, run:

    ```bash
    nvm install --lts
    nvm use --lts
    ```

- **Windows:**
  - Use [`nvm-windows`](https://github.com/coreybutler/nvm-windows)
  - After installation, open a new terminal and run:

    ```bash
    nvm install lts
    nvm use lts
    ```

Once installed, confirm your Node version:

```bash
node -v   # Should be >= 18.0.0

### Install dependencies

```bash
# Install with Yarn
yarn install

# Or with npm
npm install
```

### Running the App - Scripts are found in package.json under the client folder if you wish to invoke via your IDE

```
yarn dev
# or
npm run dev

```


### Project Structure

├── public/                 # Static assets
├── src/
│   ├── assets/            # Styles, images
│   ├── components/        # Vue components (e.g., EventTicketLookup.vue)
│   ├── models/            # TypeScript types
│   ├── stores/            # Pinia stores (e.g., eventStore.ts)
│   ├── views/             # App views/pages
│   ├── App.vue
│   └── main.ts
├── tailwind.config.js     # Tailwind setup
├── postcss.config.js      # Tailwind + PostCSS config
└── vite.config.ts         # Vite config



