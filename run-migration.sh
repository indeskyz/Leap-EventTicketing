#!/bin/bash

# Configuration
DEFAULT_DB_PATH="./skillsAssessmentEvents.db"

# Detect operating system
OS=""
case "$(uname -s)" in
    Linux*)     OS="linux";;
    Darwin*)    OS="mac";;
    CYGWIN*|MINGW*|MSYS*) OS="windows";;
    *)          OS="unknown"
esac

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "Node.js is not installed. Installing..."
    
    case "$OS" in
        "linux")
            sudo apt-get update
            sudo apt-get install -y nodejs npm
            ;;
        "mac")
            if ! command -v brew &> /dev/null; then
                echo "Homebrew not found. Installing..."
                /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
            fi
            brew install node
            ;;
        "windows")
            echo "Please install Node.js from https://nodejs.org/"
            exit 1
            ;;
        *)
            echo "Unsupported operating system. Please install Node.js manually."
            exit 1
            ;;
    esac
fi

# Check if SQLite is installed
if ! command -v sqlite3 &> /dev/null; then
    echo "SQLite is not installed. Installing..."
    
    case "$OS" in
        "linux")
            sudo apt-get update
            sudo apt-get install -y sqlite3
            ;;
        "mac")
            brew install sqlite
            ;;
        "windows")
            echo "Please install SQLite from https://sqlite.org/download.html"
            exit 1
            ;;
        *)
            echo "Unsupported operating system. Please install SQLite manually."
            exit 1
            ;;
    esac
fi

# Install node dependencies if needed
if [ ! -d "node_modules" ]; then
    echo "Installing Node.js dependencies..."
    npm install sqlite3 dotenv uuid
fi

# Get database path from user or use default
echo "Current database path configuration:"
echo "Default: $DEFAULT_DB_PATH"
if [ -f ".env" ]; then
    CURRENT_PATH=$(grep -oP 'DB_PATH=\K.*' .env || echo "")
    [ -n "$CURRENT_PATH" ] && echo "Current: $CURRENT_PATH"
fi

read -p "Enter database path [default: $DEFAULT_DB_PATH]: " db_path
DB_PATH=${db_path:-$DEFAULT_DB_PATH}

# Create .env file if it doesn't exist
if [ ! -f ".env" ]; then
    echo "DB_PATH=$DB_PATH" > .env
    echo "Created .env file with DB_PATH=$DB_PATH"
else
    # Update existing .env file
    if grep -q "DB_PATH=" .env; then
        sed -i.bak "s|DB_PATH=.*|DB_PATH=$DB_PATH|" .env
        rm -f .env.bak 2>/dev/null
    else
        echo "DB_PATH=$DB_PATH" >> .env
    fi
    echo "Updated .env file with DB_PATH=$DB_PATH"
fi

# Run the migration script
echo "Running database migrations..."
node migrate.js

echo "Migration process completed."