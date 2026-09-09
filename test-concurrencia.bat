@echo off
echo =======================================================
echo TEST DE CONCURRENCIA OPTIMISTA - SUBASTAYA (UNAJ)
echo Disparando 2 pujas simultaneas al mismo milisegundo...
echo =======================================================

start "Peticion 1 - Comprador 2" curl -i -X POST http://localhost:5120/api/auctions/1/bids -H "Content-Type: application/json" -d "{\"subastaId\":1,\"compradorId\":3,\"monto\":50000}"
start "Peticion 2 - Comprador 1" curl -i -X POST http://localhost:5120/api/auctions/1/bids -H "Content-Type: application/json" -d "{\"subastaId\":1,\"compradorId\":2,\"monto\":50000}"

echo =======================================================
echo Peticiones disparadas. Revisa las respuestas:
echo Una debe retornar HTTP 200 OK y la otra HTTP 409 Conflict.
echo =======================================================
pause