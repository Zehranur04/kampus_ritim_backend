using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Persistence
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- Standart Metotlar ---
        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        // --- Özel Metotlar ---

        public async Task<bool> IsSlotAvailableAsync(int professorId, DateTime date)
        {
            // Microsoft.EntityFrameworkCore ekli olduğu için artık hata vermez
            bool isBusy = await _context.Appointments
                .AnyAsync(x => x.ProfessorId == professorId && x.Date == date);

            return !isBusy;
        }

        public async Task<List<Appointment>> GetByProfessorIdAsync(int professorId)
        {
            return await _context.Appointments
                .Where(x => x.ProfessorId == professorId)
                .Include(x => x.Professor)
                .ToListAsync(); // include Professor for mapping
        }

        public async Task<List<Appointment>> GetByUserIdAsync(int userId)
        {
            return await _context.Appointments
                .Where(x => x.UserId == userId)
                .Include(x => x.Professor)
                .ToListAsync();
        }
    }
}
