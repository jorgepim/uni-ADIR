using AppClinica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppClinica.Models.ViewModels;
using Newtonsoft.Json;

namespace AppClinica.Controllers
{
    public class PreguntasController : Controller
    {
        private readonly AppDbContext _context;

        public PreguntasController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ADI_R()
        {
            var preguntas = _context.Preguntas
           .Include(p => p.SeccionTest)
               .ThenInclude(s => s.Test) // necesitas una propiedad de navegación en SeccionTest hacia Tests
           .Where(p => p.SeccionTest.Test.NombreTest == "ADI-R")
           .OrderBy(p => p.Orden)
           .ToList();

            return View(preguntas);
        }



        [HttpGet]
        public IActionResult ADOS2(string modulo, int idPaciente)
        {
            if (string.IsNullOrEmpty(modulo))
                return BadRequest("Debe proporcionar un módulo válido (T, 1, 2, 3, 4).");

            var preguntas = _context.Preguntas
                .Include(p => p.SeccionTest)
                    .ThenInclude(s => s.Test)
                .Include(p => p.OpcionesRespuestaPregunta)
                .Where(p =>
                    p.SeccionTest.Test.NombreTest == "ADOS2" &&
                    p.SeccionTest.modulo == modulo
                )
                .OrderBy(p => p.Orden)
                .Select(p => new PreguntaConOpcionesViewModel
                {
                    IdPregunta = p.IdPregunta,
                    TextoPregunta = p.TextoPregunta,
                    TipoRespuesta = p.TipoRespuesta,
                    Orden = p.Orden,
                    Opciones = p.OpcionesRespuestaPregunta.Select(o => new OpcionRespuestaViewModel
                    {
                        IdOpcion = o.IdOpcion,
                        Codigo = o.Codigo,
                        Descripcion = o.Descripcion
                    }).ToList()
                })
                .ToList();

            ViewBag.Modulo = modulo;
            ViewBag.IdPaciente = idPaciente;

            return View(preguntas);
        }



        //Ver resumern
        [HttpPost]
        public IActionResult GuardarRespuestas(List<RespuestaPreguntaViewModel> respuestas)
        {
            // Puedes guardar las respuestas temporalmente en TempData o en sesión, o reenviar al resumen con ellas
            TempData["Respuestas"] = JsonConvert.SerializeObject(respuestas);
            return RedirectToAction("ResumenRespuesta");
        }

        public IActionResult ResumenRespuesta()
        {
            if (TempData["Respuestas"] == null)
                return RedirectToAction("Index");

            var respuestasJson = TempData["Respuestas"].ToString();
            var respuestas = JsonConvert.DeserializeObject<List<RespuestaPreguntaViewModel>>(respuestasJson);

            // Recuperar datos completos para mostrar en resumen
            var resumen = respuestas
                .Select(r =>
                {
                    var pregunta = _context.Preguntas
                        .Include(p => p.OpcionesRespuestaPregunta)
                        .FirstOrDefault(p => p.IdPregunta == r.IdPregunta);

                    var opcionSeleccionada = pregunta?.OpcionesRespuestaPregunta
                        .FirstOrDefault(o => o.IdOpcion == r.IdOpcionSeleccionada);

                    return new ResumenRespuestaViewModel
                    {
                        TextoPregunta = pregunta?.TextoPregunta,
                        OpcionSeleccionada = opcionSeleccionada?.Descripcion,
                        CodigoSeleccionado = opcionSeleccionada?.Codigo ?? 0
                    };
                }).ToList();

            TempData["RespuestasJson"] = respuestasJson; // Guardamos para confirmar luego

            return View(resumen);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmarEvaluacion()
        {
            var respuestasJson = TempData["RespuestasJson"]?.ToString();
            if (string.IsNullOrEmpty(respuestasJson))
                return RedirectToAction("Index");

            var respuestas = JsonConvert.DeserializeObject<List<RespuestaPreguntaViewModel>>(respuestasJson);

            // Aquí deberías guardar las respuestas en la base de datos
            foreach (var r in respuestas)
            {
                var opcion = _context.OpcionesRespuestaPregunta
                    .FirstOrDefault(o => o.IdOpcion == r.IdOpcionSeleccionada);
                var respuesta = new RespuestaPaciente
                {
                    IdPregunta = r.IdPregunta,
                    IdRespuestaOpcion = opcion?.Codigo ?? 0, // ✅ Aquí va el Código, no el Id
                    RespuestaTexto = null, // Si no hay comentario
                   /* IdResultado = idResultado*/ // Debes establecer esto también

                };
                _context.RespuestasPaciente.Add(respuesta);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ConfirmacionFinal");
        }




    }
}
