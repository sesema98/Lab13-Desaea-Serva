#!/usr/bin/env bash
set -euo pipefail

api="${1:-http://127.0.0.1:5087}"
out="${2:-evidencias/resultados-pruebas.txt}"
mkdir -p "$(dirname "$out")"
: > "$out"

probar() {
  nombre="$1"; metodo="$2"; ruta="$3"; datos="${4:-}"
  temporal=$(mktemp)
  if [[ -n "$datos" ]]; then
    codigo=$(curl -sS -o "$temporal" -w '%{http_code}' -X "$metodo" "$api$ruta" -H 'Content-Type: application/json' -d "$datos")
  else
    codigo=$(curl -sS -o "$temporal" -w '%{http_code}' -X "$metodo" "$api$ruta")
  fi
  printf '%-46s HTTP %s\n' "$nombre" "$codigo" | tee -a "$out"
  if [[ -s "$temporal" ]]; then tr -d '\n' < "$temporal" | cut -c1-240 | tee -a "$out"; printf '\n' | tee -a "$out"; fi
  rm -f "$temporal"
}

probar "GET lista de observatorios" GET /api/observatorios
probar "GET observatorio inexistente" GET /api/observatorios/999
probar "POST DTO inválido" POST /api/observatorios '{"nombre":"X","ciudad":"","capacidad":0}'
probar "POST observatorio nuevo" POST /api/observatorios '{"nombre":"Ventana del Cosmos","ciudad":"Cusco","capacidad":30,"activo":true}'
probar "POST nombre duplicado" POST /api/observatorios '{"nombre":"Cielo Andino","ciudad":"Lima","capacidad":20,"activo":true}'
probar "PUT observatorio" PUT /api/observatorios/2 '{"nombre":"Mirador Sur Renovado","ciudad":"Arequipa","capacidad":28,"activo":true}'
probar "POST reserva" POST /api/reservasobservacion '{"observatorioId":1,"visitante":"María López","correo":"maria@example.com","fechaHora":"2099-06-20T20:30:00","cantidadPersonas":3}'
probar "POST reserva con horario duplicado" POST /api/reservasobservacion '{"observatorioId":1,"visitante":"Pedro Ruiz","correo":"pedro@example.com","fechaHora":"2099-06-20T20:30:00","cantidadPersonas":2}'
probar "GET reservas con filtros" GET '/api/reservasobservacion?estado=Pendiente&desde=2099-01-01'
probar "GET ruta anidada" GET /api/observatorios/1/reservas
probar "DELETE reserva" DELETE /api/reservasobservacion/1
probar "DELETE observatorio inexistente" DELETE /api/observatorios/999
