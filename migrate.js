require("dotenv").config();
const sqlite3 = require("sqlite3").verbose();
const path = require("path");
const { v4: uuidv4 } = require('uuid');

// Configuration
const DB_PATH = process.env.DB_PATH;
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

const getQuery = (query, params = []) => {
  return new Promise((resolve, reject) => {
    db.get(query, params, (err, row) => {
      if (err) reject(err);
      else resolve(row);
    });
  });
};

const allQuery = (query, params = []) => {
  return new Promise((resolve, reject) => {
    db.all(query, params, (err, rows) => {
      if (err) reject(err);
      else resolve(rows);
    });
  });
};

const isMigrationApplied = async (migrationName) => {
  try {
    const row = await getQuery(
      `SELECT 1 FROM ${MIGRATIONS_TABLE} WHERE name = ?`,
      [migrationName]
    );
    return !!row;
  } catch (e) {
    return false;
  }
};

const markMigrationApplied = async (migrationName) => {
  await runQuery(
    `INSERT INTO ${MIGRATIONS_TABLE} (name, applied_at) VALUES (?, datetime('now'))`,
    [migrationName]
  );
};

const initMigrationsTable = async () => {
  await runQuery(`
    CREATE TABLE IF NOT EXISTS ${MIGRATIONS_TABLE} (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      name TEXT UNIQUE NOT NULL,
      applied_at TEXT NOT NULL
    )
  `);
};

const transformEventsTable = async () => {
  console.log("Transforming Events table...");
  await runQuery(`BEGIN TRANSACTION`);
  
  try {
    await runQuery(`
      CREATE TABLE NewEvents (
        Id TEXT PRIMARY KEY NOT NULL,
        Name TEXT NOT NULL,
        StartDate TEXT NOT NULL,
        EndDate TEXT NOT NULL,
        Location TEXT NOT NULL,
        Description TEXT,
        CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP,
        UpdatedAt TEXT DEFAULT CURRENT_TIMESTAMP
      )
    `);

    const oldTableInfo = await allQuery("PRAGMA table_info(Events)");
    const hasOldColumns = oldTableInfo.some(col => col.name === "StartsOn");
    const rowCount = await getQuery("SELECT COUNT(*) as count FROM Events");

    if (rowCount.count > 0) {
      console.log(`Migrating ${rowCount.count} events...`);
      
      const events = await allQuery("SELECT * FROM Events");
      for (const event of events) {
        await runQuery(`
          INSERT INTO NewEvents 
          (Id, Name, StartDate, EndDate, Location, Description, CreatedAt, UpdatedAt)
          VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        `, [
          event.Id || uuidv4(),
          event.Name,
          hasOldColumns ? event.StartsOn : event.StartDate,
          hasOldColumns ? event.EndsOn : event.EndDate,
          event.Location,
          event.Description,
          event.CreatedAt,
          event.UpdatedAt || event.CreatedAt
        ]);
      }
    }

    await runQuery(`DROP TABLE Events`);
    await runQuery(`ALTER TABLE NewEvents RENAME TO Events`);
    await runQuery(`CREATE INDEX IF NOT EXISTS idx_events_start_date ON Events(StartDate)`);
    await runQuery(`CREATE INDEX IF NOT EXISTS idx_events_end_date ON Events(EndDate)`);
    await runQuery(`CREATE INDEX IF NOT EXISTS idx_events_location ON Events(Location)`);

    await markMigrationApplied("transform_events");
    await runQuery(`COMMIT`);
    console.log("Events table transformation complete");
  } catch (err) {
    await runQuery(`ROLLBACK`);
    console.error("Events table transformation failed:", err);
    throw err;
  }
};

const restructureTicketSalesTable = async () => {
  console.log("Restructuring TicketSales table...");
  await runQuery(`BEGIN TRANSACTION`);

  try {
    // Step 1: Create new table
    await runQuery(`
      CREATE TABLE NewTicketSales (
        Id TEXT PRIMARY KEY NOT NULL,
        EventId TEXT NOT NULL,
        UserId TEXT NOT NULL,
        PurchaseDate TEXT NOT NULL,
        PriceInCents INTEGER NOT NULL,
        FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE
      )
    `);

    // Step 2: Copy existing data
    const ticketSales = await allQuery("SELECT * FROM TicketSales");
    for (const sale of ticketSales) {
      await runQuery(`
        INSERT INTO NewTicketSales (Id, EventId, UserId, PurchaseDate, PriceInCents)
        VALUES (?, ?, ?, ?, ?)
      `, [
        sale.Id || uuidv4(),
        sale.EventId,
        sale.UserId,
        sale.PurchaseDate,
        sale.PriceInCents
      ]);
    }

    // Step 3: Replace old table
    await runQuery(`DROP TABLE TicketSales`);
    await runQuery(`ALTER TABLE NewTicketSales RENAME TO TicketSales`);
    await runQuery(`CREATE INDEX IF NOT EXISTS idx_ticket_sales_event_id ON TicketSales(EventId)`);

    await markMigrationApplied("restructure_ticket_sales");
    await runQuery(`COMMIT`);
    console.log("TicketSales table restructure complete");
  } catch (err) {
    await runQuery(`ROLLBACK`);
    console.error("TicketSales table restructure failed:", err);
    throw err;
  }
};

const applyMigrations = async () => {
  console.log("Running database migrations...");

  try {
    await initMigrationsTable();

    if (!(await isMigrationApplied("transform_events"))) {
      await transformEventsTable();
    }

    if (!(await isMigrationApplied("restructure_ticket_sales"))) {
      await restructureTicketSalesTable();
    }

    console.log("All migrations completed successfully");
  } catch (err) {
    console.error("Migration failed:", err);
    process.exit(1);
  } finally {
    db.close();
    console.log("Migration process completed.");
  }
};

applyMigrations();
