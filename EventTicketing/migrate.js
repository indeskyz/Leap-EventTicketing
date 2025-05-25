require("dotenv").config();
const sqlite3 = require("sqlite3").verbose();
const path = require("path");
const magicDBName = `skillsAssessmentEvents.db`;

// Configuration
const DB_PATH = process.env.DB_PATH || path.join(__dirname, `${magicDBName}`);
const MIGRATIONS_TABLE = "__migrations";

// Database connection
const db = new sqlite3.Database(DB_PATH);

// Utility functions
const runQuery = (query, params = []) => {
  return new Promise((resolve, reject) => {
    db.run(query, params, function (err) {
      if (err) reject(err);
      else resolve(this);
    });
  });
};

const isMigrationApplied = async (migrationName) => {
  return new Promise((resolve, reject) => {
    db.get(
      `SELECT 1 FROM ${MIGRATIONS_TABLE} WHERE name = ?`,
      [migrationName],
      (err, row) => {
        if (err) reject(err);
        else resolve(!!row);
      }
    );
  });
};

const markMigrationApplied = async (migrationName) => {
  await runQuery(
    `INSERT INTO ${MIGRATIONS_TABLE} (name, applied_at) VALUES (?, datetime('now'))`,
    [migrationName]
  );
};

const initMigrationsTable = async () => {
  try {
    await runQuery(`
      CREATE TABLE IF NOT EXISTS ${MIGRATIONS_TABLE} (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT UNIQUE NOT NULL,
        applied_at TEXT NOT NULL
      )
    `);
  } catch (err) {
    console.error("Error initializing migrations table:", err);
    process.exit(1);
  }
};

// Migration functions
const addIndexes = async () => {
  console.log("Applying migration: add_indexes");

  await runQuery(
    "CREATE INDEX IF NOT EXISTS idx_events_starts_on ON Events(StartsOn)"
  );
  await runQuery(
    "CREATE INDEX IF NOT EXISTS idx_events_location ON Events(Location)"
  );
  await runQuery(
    "CREATE INDEX IF NOT EXISTS idx_ticket_sales_event_id ON TicketSales(EventId)"
  );
  await runQuery(
    "CREATE INDEX IF NOT EXISTS idx_ticket_sales_user_id ON TicketSales(UserId)"
  );
  await runQuery(
    "CREATE INDEX IF NOT EXISTS idx_ticket_sales_purchase_date ON TicketSales(PurchaseDate)"
  );

  await markMigrationApplied("add_indexes");
};

const addEventsColumns = async () => {
  console.log("Applying migration: add_events_columns");

  const columns = [
    { name: "Description", type: "TEXT" },
    { name: "CreatedAt", type: "TEXT DEFAULT CURRENT_TIMESTAMP" },
    { name: "UpdatedAt", type: "TEXT DEFAULT CURRENT_TIMESTAMP" },
  ];

  for (const column of columns) {
    try {
      await runQuery(
        `ALTER TABLE Events ADD COLUMN ${column.name} ${column.type}`
      );
    } catch (e) {
      console.log(`${column.name} column already exists`);
    }
  }

  await markMigrationApplied("add_events_columns");
};

const addTicketSalesColumns = async () => {
  console.log("Applying migration: add_ticketsales_columns");

  const columns = [
    { name: "Quantity", type: "INTEGER NOT NULL DEFAULT 1" },
    { name: "Status", type: 'TEXT NOT NULL DEFAULT "completed"' },
    { name: "PaymentMethod", type: "TEXT" },
    { name: "CreatedAt", type: "TEXT DEFAULT CURRENT_TIMESTAMP" },
  ];

  for (const column of columns) {
    try {
      await runQuery(
        `ALTER TABLE TicketSales ADD COLUMN ${column.name} ${column.type}`
      );
    } catch (e) {
      console.log(`${column.name} column already exists`);
    }
  }

  await markMigrationApplied("add_ticketsales_columns");
};

// Main migration function
const applyMigrations = async () => {
  try {
    await initMigrationsTable();

    const migrations = [
      { name: "add_indexes", action: addIndexes },
      { name: "add_events_columns", action: addEventsColumns },
      { name: "add_ticketsales_columns", action: addTicketSalesColumns },
    ];

    for (const migration of migrations) {
      if (!(await isMigrationApplied(migration.name))) {
        await migration.action();
      }
    }

    console.log("All migrations applied successfully");
  } catch (err) {
    console.error("Migration failed:", err);
    process.exit(1);
  } finally {
    db.close();
  }
};

// Run migrations
applyMigrations();
