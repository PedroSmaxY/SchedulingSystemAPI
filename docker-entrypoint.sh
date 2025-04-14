#!/bin/bash
set -e

echo "Aguardando PostgreSQL..."
until PGPASSWORD=$DB_PASSWORD pg_isready -h "$DB_SERVER" -U "$DB_USER"; do
  echo "PostgreSQL indisponível - aguardando..."
  sleep 1
done

echo "PostgreSQL disponível!"

echo "Executando migrações..."
if ! dotnet SchedulingSystemAPI.dll --apply-migrations; then
  echo "ERRO: Falha ao aplicar migrações!"
  exit 1
fi

echo "Iniciando aplicação..."
exec dotnet SchedulingSystemAPI.dll