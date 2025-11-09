using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnetCoreInterviewPrepDemo.DAL;
using dotnetCoreInterviewPrepDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace dotnetCoreInterviewPrepDemo.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0", Deprecated =true)] // Mark version 2.0 as deprecated
   
    [ApiVersion("3.0")]
    public class PatientController : ControllerBase
    {
        private PatientDbContext _db = null;
        private ILogger _logger;

        public PatientController(PatientDbContext db, ILogger<PatientController> logger)
        {
            _db = db;
            _logger = logger;
            db.Database.EnsureCreated();
        }

        [HttpPost("/api/patient")]
        public IActionResult AddPatient(Patient patientObj)
        {
            _logger.LogInformation("Add Patient");

            // Set versioning metadata for new patient
            patientObj.Version = 1;
            patientObj.CreatedAt = DateTime.UtcNow;
            patientObj.IsLatestVersion = true;
            patientObj.PreviousVersionId = null;

            _db.Patients.Add(patientObj);
            _db.SaveChanges();

            return Ok(patientObj);
        }

        [HttpPost("/api/v{version:ApiVersion}/patient")]
        [MapToApiVersion("2.0")]
        [Produces("application/json", "application/xml")]
        public IActionResult AddPatientV2(Patient patientObj)
        {
            _logger.LogInformation("Add Patient");

            // Set versioning metadata for new patient
            patientObj.Version = 1;
            patientObj.CreatedAt = DateTime.UtcNow;
            patientObj.IsLatestVersion = true;
            patientObj.PreviousVersionId = null;

            _db.Patients.Add(patientObj);
            _db.SaveChanges();

            return Ok(patientObj);
        }

        [HttpPost("/api/v{version:ApiVersion}/patient")]
        [MapToApiVersion("3.0")]
        public IActionResult AddPatientV3(Patient patientObj)
        {
            _logger.LogInformation("Add Patient 3");

            // Set versioning metadata for new patient
            patientObj.Version = 1;
            patientObj.CreatedAt = DateTime.UtcNow;
            patientObj.IsLatestVersion = true;
            patientObj.PreviousVersionId = null;

            _db.Patients.Add(patientObj);
            _db.SaveChanges();

            return Ok(patientObj);
        }

        [HttpPut("/api/patient/{id}")]
        public IActionResult UpdatePatient(int id, Patient updatedPatient)
        {
            var existingPatient = _db.Patients.FirstOrDefault(p => p.id == id && p.IsLatestVersion);

            if (existingPatient == null)
            {
                return NotFound($"Patient with ID {id} not found or not the latest version");
            }

            // Create new version
            var newVersion = new Patient
            {
                patientName = updatedPatient.patientName,
                Problems = updatedPatient.Problems,
                Version = existingPatient.Version + 1,
                CreatedAt = existingPatient.CreatedAt,
                ModifiedAt = DateTime.UtcNow,
                IsLatestVersion = true,
                PreviousVersionId = existingPatient.id,
            };

            // Mark previous version as not latest
            existingPatient.IsLatestVersion = false;

            _db.Patients.Add(newVersion);
            _db.SaveChanges();

            return Ok(newVersion);
        }

        [HttpGet("/api/patient/{id}/versions")]
        [Produces("application/json", "application/xml")]
        public IActionResult GetPatientVersions(int id)
        {
            var patientVersions = _db
                .Patients.Where(p => p.id == id || p.PreviousVersionId == id)
                .OrderByDescending(p => p.Version)
                .ToList();

            if (!patientVersions.Any())
            {
                return NotFound($"No patient versions found for ID {id}");
            }

            return Ok(patientVersions);
        }

        [HttpGet("/api/patient/{id}/version/{version}")]
        public IActionResult GetPatientVersion(int id, int version)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.id == id && p.Version == version);

            if (patient == null)
            {
                return NotFound($"Patient with ID {id} and version {version} not found");
            }

            return Ok(patient);
        }
    }
}
