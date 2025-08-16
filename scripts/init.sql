-- Initialization script for PostgreSQL database
-- This script runs when the Docker container starts for the first time

-- Create additional database for testing if needed
DO $$
BEGIN
   IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'e-commerce-course-test') THEN
      PERFORM dblink_exec('dbname=' || current_database(), 'CREATE DATABASE "e-commerce-course-test"');
   END IF;
END
$$;

-- Set timezone
SET timezone = 'UTC';

-- Create extensions if they don't exist
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";

-- Grant necessary permissions
GRANT ALL PRIVILEGES ON DATABASE "e-commerce-course" TO appuser;

-- Log initialization
DO $$
BEGIN
    RAISE NOTICE 'Database initialized successfully for e-commerce-course';
END
$$;
