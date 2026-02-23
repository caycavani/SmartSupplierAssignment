-- down.sql (PostgreSQL)  —  revertir esquema
BEGIN;

DROP TABLE IF EXISTS idempotency_records;
DROP TABLE IF EXISTS outbox_messages;
DROP TABLE IF EXISTS assistance_requests;
DROP TABLE IF EXISTS provider_capabilities;
DROP TABLE IF EXISTS providers;

COMMIT;