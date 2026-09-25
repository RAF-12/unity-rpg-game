using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    //asigenamos el nombre de un personaje por defecto para pruebas de los disponibles en tiposPersonaje
    public string characterName = "Guerrero";
    public int levelActual;
    public int experienciaActual;
    public int[] sigienteNivel;
    // descaramos 6 estadisticas con sus nombres, sus valores iniciales en 3 y  dentro del array
    public string[] nombresEstadisticas = { "Fuerza", "Destreza", "Vitalidad", "Inteligencia", "Suerte", "Defensa" };

    // asignamos valores iniciales a las estadisticas
    public float[] valoresEstadisticas = { 3, 3, 3, 3, 3, 3 };

    // creamos un array de tipos de personaje para que tenga diferentes bonificaciones CON 5 TIPOS
    public string[] tiposPersonaje = { "Guerrero", "Arquero", "Mago", "Ladron", "Paladin" };
    // definimos un array de bonificaciones por tipo de con sus respectivas bonificaciones
    public float[,] bonificacionesPorTipo = {
        { 0.7f, 0.5f, 0.01f, 0.1f, 0.01f, 0.4f }, // Guerrero
        { 1, 2, 1, 0, 1, 0 }, // Arquero
        { 0, 0, 1, 2, 2, 0 }, // Mago
        { 1, 2, 0, 1, 0, 1 }, // Ladron
        { 2, 1, 2, 1, 1, 1 }  // Paladin
    };

    // unimos nombres y valores en un diccionario
    public System.Collections.Generic.Dictionary<string, float> estadisticas;

    //llamanos a vida manager para gestionar la vida del personaje
    private VidaManager vidaManager;

    // Awake se asegura de inicializar antes que cualquier Update y cachear referencias
    void Awake()
    {
        vidaManager = GetComponent<VidaManager>();
        if (vidaManager == null)
        {
            Debug.LogWarning("VidaManager no encontrado en el GameObject.");
        }

        inicializarEstadisticas();
    }

    // Se ejecuta en editor cuando modificas parámetros en el Inspector
    private void OnValidate()
    {
        // Asegurar que valoresEstadisticas tenga al menos el tamaño de nombresEstadisticas
        if (nombresEstadisticas == null)
            return;

        if (valoresEstadisticas == null || valoresEstadisticas.Length != nombresEstadisticas.Length)
        {
            int target = nombresEstadisticas.Length;
            float[] nuevos = new float[target];
            if (valoresEstadisticas != null)
            {
                int copy = Mathf.Min(valoresEstadisticas.Length, target);
                Array.Copy(valoresEstadisticas, nuevos, copy);
            }

            // Valores por defecto (si no existían) a 3
            for (int i = 0; i < target; i++)
            {
                if (nuevos[i] == 0f)
                    nuevos[i] = 3f;
            }

            valoresEstadisticas = nuevos;
        }
    }

    // inicializa/repuebla el diccionario de estadisticas
    private void inicializarEstadisticas()
    {
        estadisticas = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

        if (nombresEstadisticas == null || nombresEstadisticas.Length == 0)
        {
            Debug.LogWarning("nombresEstadisticas vacío. No se crearán estadísticas.");
            return;
        }

        if (valoresEstadisticas == null)
        {
            Debug.LogWarning("valoresEstadisticas es null. Se crearán valores por defecto.");
            valoresEstadisticas = new float[nombresEstadisticas.Length];
            for (int i = 0; i < valoresEstadisticas.Length; i++)
                valoresEstadisticas[i] = 3f;
        }

        int cantidad = Mathf.Min(nombresEstadisticas.Length, valoresEstadisticas.Length);
        int tipoIndex = Array.IndexOf(tiposPersonaje, characterName);
        int bonifCols = bonificacionesPorTipo.GetLength(1);

        for (int i = 0; i < cantidad; i++)
        {
            string key = nombresEstadisticas[i]?.Trim() ?? $"Stat_{i}";
            float valor = valoresEstadisticas[i];

            if (tipoIndex >= 0 && tipoIndex < bonificacionesPorTipo.GetLength(0) && i < bonifCols)
            {
                valor += bonificacionesPorTipo[tipoIndex, i];
            }

            estadisticas[key] = valor;
        }

        if (cantidad < nombresEstadisticas.Length)
        {
            Debug.LogWarning("Los arrays 'nombresEstadisticas' y 'valoresEstadisticas' tienen longitudes diferentes. Algunas estadísticas no se inicializaron.");
        }

        // Diagnóstico: comprobar que 'Vitalidad' existe
        if (!estadisticas.ContainsKey("Vitalidad"))
        {
            string keys = string.Join(", ", new List<string>(estadisticas.Keys).ToArray());
            Debug.LogError($"Inicialización: 'Vitalidad' no encontrada en estadisticas. Claves creadas: [{keys}]. nombresEstadisticas.Length={nombresEstadisticas?.Length ?? 0}, valoresEstadisticas.Length={valoresEstadisticas?.Length ?? 0}");
        }
        else
        {
            Debug.Log("Inicialización: 'Vitalidad' encontrada en estadisticas con valor: " + estadisticas["Vitalidad"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // comprobamos que sigienteNivel esté definido y que levelActual esté dentro del rango
        if (sigienteNivel == null || sigienteNivel.Length == 0)
        {
            return;
        }

        if (levelActual >= sigienteNivel.Length)
        {
            return;
        }

        if (experienciaActual >= sigienteNivel[levelActual])
        {
            //actualizamos las estadisticas
            experienciaActual = 0;
            levelActual++;

            if (vidaManager == null)
            {
                vidaManager = GetComponent<VidaManager>();
                if (vidaManager == null)
                {
                    Debug.LogWarning("Imposible actualizar vida: VidaManager no encontrado.");
                    return;
                }
            }

            // Asegurarse de que el diccionario está inicializado
            if (estadisticas == null || estadisticas.Count == 0)
            {
                inicializarEstadisticas();
            }

            // Usamos TryGetValue para evitar KeyNotFoundException si la clave no existe
            if (!estadisticas.TryGetValue("Vitalidad", out float vitalidad))
            {
                // Antes de abandonar hacemos logs de apoyo
                Debug.LogError("Clave 'Vitalidad' no encontrada en estadisticas al subir de nivel. Claves disponibles: " + string.Join(", ", new List<string>(estadisticas.Keys).ToArray()));
                return;
            }

            int vida = (int)vidaManager.maximaVida;
            float nuevaVida = vida * vitalidad;
            // actualizamos la vida maxima en vida manager pero convertiendola a int
            vidaManager.updateMaxVida(Mathf.FloorToInt(nuevaVida));

            // aumentamos todas las estadisticas por sus bonificaciones
            int tipoIndex = Array.IndexOf(tiposPersonaje, characterName);
            int bonifCols = bonificacionesPorTipo.GetLength(1);
            for (int i = 0; i < nombresEstadisticas.Length; i++)
            {
                float bonificacion = 0;
                if (tipoIndex >= 0 && tipoIndex < bonificacionesPorTipo.GetLength(0) && i < bonifCols)
                {
                    bonificacion = bonificacionesPorTipo[tipoIndex, i];
                }

                // Aseguramos que la clave existe antes de incrementar, si no existe la añadimos
                if (estadisticas.ContainsKey(nombresEstadisticas[i]))
                {
                    estadisticas[nombresEstadisticas[i]] += bonificacion;
                }
                else
                {
                    estadisticas[nombresEstadisticas[i]] = bonificacion;
                    Debug.LogWarning($"Se añadió la estadística faltante '{nombresEstadisticas[i]}' con valor inicial {bonificacion}.");
                }
            }
        }
    }

    public void addExperiencia(int exp)
    {
        experienciaActual += exp;
    }
}