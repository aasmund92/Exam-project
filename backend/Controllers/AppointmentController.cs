using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public AppointmentController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

        /// <summary>
        /// Get all appointments
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            if(_ClinicDbContext.Appointments == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Appointments.ToListAsync();
        }

        /// <summary>
        /// Get one specific appointment
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Appointment>> GetAppointment(int Id)
        {
            
            if(_ClinicDbContext.Appointments == null)
            {
                return NotFound();
            }
            var appointment = await _ClinicDbContext.Appointments.FindAsync(Id);
            if(appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        /// <summary>
        /// Creates a new appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "AppointmentTime": "2025-12-12T09:00:00",
        ///         "PatientId": 1,
        ///         "CategoryId": 1,
        ///         "DoctorId": 1,
        ///         "Duration": "00:30:00"
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
        {

            var appointments = await _ClinicDbContext.Appointments
                .Where(a => a.DoctorId == appointment.DoctorId || a.PatientId == appointment.PatientId)
                .ToListAsync();

            DateTime newAppointmentEndTime = appointment.AppointmentTime.AddMinutes(appointment.Duration.TotalMinutes);

            var existingPatientAppointment = appointments.FirstOrDefault(a => a.PatientId == appointment.PatientId && a.AppointmentTime == appointment.AppointmentTime);

            if (existingPatientAppointment != null)
            {
                DateTime existingAppointmentStartTime = existingPatientAppointment.AppointmentTime;
                DateTime existingAppointmentEndTime = existingAppointmentStartTime.AddMinutes(existingPatientAppointment.Duration.TotalMinutes);

                return BadRequest($"Patient already have an appointment at this time: {existingAppointmentStartTime:yyyy-MM-dd HH:mm} - {existingAppointmentEndTime:HH:mm}");
            }

            var overlappingPatientAppointment = appointments.FirstOrDefault(a =>
                a.PatientId == appointment.PatientId &&
                a.AppointmentTime < newAppointmentEndTime &&
                a.AppointmentTime.AddMinutes(a.Duration.TotalMinutes) > appointment.AppointmentTime
            );

            if (overlappingPatientAppointment != null)
            {
                DateTime overlappingAppointmentStartTime = overlappingPatientAppointment.AppointmentTime;
                DateTime overlappingAppointmentEndTime = overlappingAppointmentStartTime.AddMinutes(overlappingPatientAppointment.Duration.TotalMinutes);

                return BadRequest($"Patient have an overlapping appointment at this time: {overlappingAppointmentStartTime:yyyy-MM-dd HH:mm} - {overlappingAppointmentEndTime:HH:mm}");
            }

            var overlappingDoctorAppointment = appointments.FirstOrDefault(a =>
                a.DoctorId == appointment.DoctorId &&
                a.AppointmentTime < newAppointmentEndTime &&
                a.AppointmentTime.AddMinutes(a.Duration.TotalMinutes) > appointment.AppointmentTime
            );

            if (overlappingDoctorAppointment != null)
            {
                DateTime overlappingAppointmentStartTime = overlappingDoctorAppointment.AppointmentTime;
                DateTime overlappingAppointmentEndTime = overlappingAppointmentStartTime.AddMinutes(overlappingDoctorAppointment.Duration.TotalMinutes);

                return BadRequest($"Doctor is already booked at this time: {overlappingAppointmentStartTime:yyyy-MM-dd HH:mm} - {overlappingAppointmentEndTime:HH:mm}");
            }

            _ClinicDbContext.Appointments.Add(appointment);
            await _ClinicDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        /// <summary>
        /// Updates a specific appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "AppointmentTime": "2025-12-12T10:00:00",
        ///         "PatientId": 1,
        ///         "CategoryId": 1,
        ///         "DoctorId": 1,
        ///         "Duration": "00:30:00"
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Appointment>> UpdateAppointment(int Id, Appointment appointment)
        {
            if(Id != appointment.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(appointment);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!AppointmentExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool AppointmentExist(int Id)
        {
            return (_ClinicDbContext.Appointments?.Any(a => a.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific appointment
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Appointment>> DeleteAppointment(int Id)
        {
            if(_ClinicDbContext.Appointments == null)
            {
                return NotFound();
            }
            var appointment = await _ClinicDbContext.Appointments.FindAsync(Id);
            if(appointment is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Appointments.Remove(appointment);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}