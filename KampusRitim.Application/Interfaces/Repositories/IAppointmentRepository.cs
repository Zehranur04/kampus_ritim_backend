using KampusRitim.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampusRitim.Application.Interfaces.Repositories
{
    // : IGenericRepository<Appointment> kısmını SİLDİK.
    public interface IAppointmentRepository
    {
        // Standart CRUD işlemleri (Generic olmadığı için elle ekliyoruz)
        Task AddAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(int id);

        // Bizim özel metotlarımız
        Task<bool> IsSlotAvailableAsync(int professorId, DateTime date);
        Task<List<Appointment>> GetByProfessorIdAsync(int professorId);
        Task<List<Appointment>> GetByUserIdAsync(int userId);
    }
}
