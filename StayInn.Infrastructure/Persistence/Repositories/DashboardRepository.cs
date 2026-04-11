using Microsoft.EntityFrameworkCore;
using StayInn.Application.DTOs.Dashboard;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Domain.Enums;
using StayInn.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace StayInn.Infrastructure.Persistence.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> ObtenerDatosDashboardAsync()
        {
            // Obtenemos la fecha actual en formato DateOnly
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var primerDiaMes = new DateOnly(hoy.Year, hoy.Month, 1);
            var haceSieteDias = hoy.AddDays(-7);

            // 1. Métricas
            var totalHab = await _context.Habitaciones.CountAsync();

            var ocupadas = await _context.Reservaciones
                .CountAsync(r => r.Estado == EstadoReservacion.Activa);

            var pendientes = await _context.Reservaciones
                .CountAsync(r => r.Estado == EstadoReservacion.Pendiente);

            var ingresos = await _context.Reservaciones
                .Where(r => r.FechaEntrada >= primerDiaMes && r.Estado != EstadoReservacion.Cancelada)
                .SumAsync(r => r.MontoTotal);

            // 2. Llegadas de hoy
            var llegadas = await _context.Reservaciones
                .Include(r => r.Habitacion)
                .Include(r => r.Usuario)
                .Where(r => r.FechaEntrada == hoy)
                .Select(r => new ReservacionDashboardDto
                {
                    NumeroHabitacion = r.Habitacion.Numero,
                    NombreHuesped = r.Usuario.NombreCompleto,
                    Estado = r.Estado.ToString()    // Convertir el Enum a String para el DTO
                }).ToListAsync();

            // 3. Salidas de hoy
            var salidas = await _context.Reservaciones
                .Include(r => r.Habitacion)
                .Include(r => r.Usuario)
                .Where(r => r.FechaSalida == hoy)
                .Select(r => new ReservacionDashboardDto
                {
                    NumeroHabitacion = r.Habitacion.Numero,
                    NombreHuesped = r.Usuario.NombreCompleto,
                    Estado = r.Estado.ToString()    // Convertir el Enum a String para el DTO
                }).ToListAsync();

            // 4. Tendencia (Últimos 7 días)
            var datosTendenciaRaw = await _context.Reservaciones
                .Where(r => r.FechaRegistro >= haceSieteDias)
                .GroupBy(r => r.FechaRegistro)
                .Select(g => new {
                    FechaKey = g.Key,
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.FechaKey)
                .ToListAsync();

            // Formatear la fecha en memoraia
            var tendencia = datosTendenciaRaw.Select(t => new TendenciaDto
            {
                Fecha = t.FechaKey.ToString("dd MMM"),
                Cantidad = t.Cantidad
            }).ToList();

            return new DashboardDto
            {
                TotalHabitaciones = totalHab,
                HabitacionesOcupadas = ocupadas,
                ReservasPendientes = pendientes,
                IngresosMesActual = ingresos,
                LlegadasHoy = llegadas,
                SalidasHoy = salidas,
                TendenciaReservas = tendencia
            };
        }
    }
}
