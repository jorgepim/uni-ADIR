using AppClinica.Models;
using AppClinica.Models.ViewModels;
using AppClinica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AppClinica.Controllers
{
    public class EvaluacionesController : Controller
    {
        private readonly AppDbContext _context;


        public EvaluacionesController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult evaluar()
        {
            return View();
        }
        public IActionResult Ados2()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SeleccionarTest(int idPaciente)
        {
            var paciente = await _context.Pacientes.FindAsync(idPaciente);
            if (paciente == null)
                return NotFound();

            return View(paciente); // ← se envía todo el objeto Paciente a la vista
        }

        public async Task<IActionResult> Adir(int idPaciente)
        {
            var paciente = await _context.Pacientes.FindAsync(idPaciente);
            if (paciente == null)
                return NotFound();

            var secciones = await _context.SeccionesTest
                .Where(s => s.IdTest == 1)
                .Include(s => s.Preguntas)
                .OrderBy(s => s.Orden)
                .ToListAsync();

            var model = new SeleccionarModuloAdirViewModel
            {
                IdPaciente = idPaciente,
                NombreEncriptado = paciente.Nombres,
                ApellidoEncriptado = paciente.Apellidos,
                Secciones = secciones
            };

            return View(model);
        }

    

        [HttpGet]
        public async Task<IActionResult> Preguntas(int idPaciente, int idSeccion)
        {
            var seccion = await _context.SeccionesTest
                .Include(s => s.Preguntas)
                .FirstOrDefaultAsync(s => s.IdSeccion == idSeccion);

            if (seccion == null)
                return NotFound();

            var preguntas = seccion.Preguntas
                .OrderBy(p => p.Orden)
                .Select(p => new PreguntaRespuestaViewModel
                {
                    IdPregunta = p.IdPregunta,
                    Orden = (int)p.Orden,
                    TextoPregunta = p.TextoPregunta,
                    Puntuacion = null,
                    Comentario = ""
                }).ToList();

            var model = new EvaluarModuloAdirViewModel
            {
                IdPaciente = idPaciente,
                IdSeccion = idSeccion,
                NombreSeccion = seccion.NombreSeccion ?? $"Sección {seccion.Orden}",
                Preguntas = preguntas
            };

            return View("Preguntas", model); // Asegúrate que así se llama la vista
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarRespuestas(EvaluarModuloAdirViewModel model)
        {
            if (!ModelState.IsValid || model.Preguntas == null || !model.Preguntas.Any())
            {
                ModelState.AddModelError("", "Datos incompletos o inválidos.");
                return View("Preguntas", model);
            }

            var especialista = await _context.Especialistas
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Usuario.Correo == User.Identity.Name);

            if (especialista == null)
            {
                ModelState.AddModelError("", "Especialista no encontrado.");
                return View("Preguntas", model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var resultado = new ResultadoTest
                {
                    IdPaciente = model.IdPaciente,
                    IdTest = 1,
                    IdEspecialista = especialista.IdEspecialista,
                    FechaRealizacion = DateTime.Today,
                    Observaciones = $"Evaluación realizada el {DateTime.Today:dd/MM/yyyy}"
                };

                _context.ResultadosTest.Add(resultado);
                await _context.SaveChangesAsync();

                foreach (var pregunta in model.Preguntas)
                {
                    var respuesta = new RespuestaPaciente
                    {
                        IdResultado = (int)resultado.IdResultado,
                        IdPregunta = pregunta.IdPregunta,
                        Puntuacion = pregunta.Puntuacion ?? 8,
                        Comentario = pregunta.Comentario
                    };

                    _context.RespuestasPaciente.Add(respuesta);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Respuestas guardadas correctamente.";
                return RedirectToAction("Adir", new { idPaciente = model.IdPaciente });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return View("Preguntas", model);
            }
        }





    }
}
