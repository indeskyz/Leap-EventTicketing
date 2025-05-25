# Database Migration Script - migrate.js

## 📋 Features  
✅ **Adds indexes** for faster queries on:  
   - `Events.StartsOn`  
   - `Events.Location`  
   - `TicketSales.EventId`  
   - `TicketSales.UserId`  
   - `TicketSales.PurchaseDate`  

✅ **Adds new columns**:  
   - **Events table**: `Description`, `CreatedAt`, `UpdatedAt`  
   - **TicketSales table**: `Quantity`, `Status`, `PaymentMethod`, `CreatedAt`  

✅ **Safe and idempotent**:  
   - Skips already-applied migrations  
   - Preserves existing table names and data  

✅ **Cross-platform**: Works on Linux, macOS, and Windows (with manual setup).  

---

## 🛠️ Installation  

1. **Clone the repository** (or download the files):  
   ```bash
   git clone https://github.com/indeskyz/db-migrations.git
   cd db-migrations
   ```

2. **Initialize npm** (if `node_modules` doesn’t exist):  
   ```bash
   npm init -y
   ```

---

## 🚀 Usage  

### 1. **Run the migration tool**:  
```bash
./run-migration.sh
```  

### 2. **When prompted**:  
- Enter your database path (e.g., `./data/events.db`) or press **Enter** to use the default (`./skillsAssesmentEvents.db`).  
- The script will create/update a `.env` file with your path.  

### Example:  
```bash
$ ./run-migration.sh
Current database path configuration:
Default: ./skillsAssesmentEvents.db
Enter database path [default: ./skillsAssesmentEvents.db]: ./data/prod.db
Updated .env file with DB_PATH=./data/prod.db
Running database migrations...
```

---

## ⚙️ Configuration  

### Option 1: **Environment file (recommended)**  
Edit `.env` to set your database path:  
```ini
DB_PATH=./path/to/your-database.db
```

### Option 2: **Prompt during execution**  
The script will prompt you if no `.env` exists.  

---

## 🖥️ **Platform Support**  

| OS       | Node.js Installation | SQLite Installation  |  
|----------|----------------------|----------------------|  
| **Linux**  | Auto-installed       | Auto-installed       |  
| **macOS**  | Auto-installed (via Homebrew) | Auto-installed |  
| **Windows** | [Manual install](https://nodejs.org/) | [Manual install](https://sqlite.org/download.html) |  

---

## 🔍 **Troubleshooting**  

- **Permission denied?** Run:  
  ```bash
  chmod +x run-migration.sh
  ```

- **Missing dependencies?**  
  - Windows users: Install Node.js and SQLite manually (links above).  
  - Linux/macOS: Let the script install them automatically.  

- **Rollback changes?**  
  SQLite doesn’t fully support `ALTER TABLE` rollbacks. Backup your database first:  
  ```bash
  cp your-database.db your-database.backup.db
  ```

---

## 📂 **File Structure**  
```
.
├── README.md           # This guide  
├── run-migration.sh    # Bash wrapper (installs dependencies)  
├── migrate.js          # Migration logic  
├── .env                # Auto-generated config file  
└── node_modules/       # Dependencies (auto-created)  
```

--- 

### 🔄 **Need to modify migrations?**  
Edit the `migrate.js` file:  
- Change `DEFAULT_DB_PATH` in `run-migration.sh` for a different default path.  
- Update column names or indexes in the `addIndexes()`, `addEventsColumns()`, or `addTicketSalesColumns()` functions.  
